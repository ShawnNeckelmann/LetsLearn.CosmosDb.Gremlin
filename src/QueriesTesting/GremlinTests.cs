using System.Configuration;
using ExRam.Gremlinq.Core;
using Gremlin.Net.Structure;
using GremlinQueries.Edges;
using GremlinQueries.Queries;
using GremlinQueries.Vertices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace QueriesTesting;

public class GremlinTests
{
    private readonly string _cosmosDbAuthKey;

    private readonly string _databaseName;

    private readonly IGremlinQuerySource _g;

    private readonly string _graphName;

    private readonly string _gremlinEndpointUrl;
    private readonly ITestOutputHelper _helper;

    public GremlinTests(ITestOutputHelper helper)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<CosmosSettings>()
            .Build();

        _cosmosDbAuthKey = configuration[nameof(_cosmosDbAuthKey)] ??
                           throw new SettingsPropertyNotFoundException(
                               $"Could not find setting value for {_cosmosDbAuthKey}.");
        _databaseName = configuration[nameof(_databaseName)] ??
                        throw new SettingsPropertyNotFoundException(
                            $"Could not find setting value for {_databaseName}.");
        _graphName = configuration[nameof(_graphName)] ??
                     throw new SettingsPropertyNotFoundException($"Could not find setting value for {_graphName}.");
        _gremlinEndpointUrl = configuration[nameof(_gremlinEndpointUrl)] ??
                              throw new SettingsPropertyNotFoundException(
                                  $"Could not find setting value for {_gremlinEndpointUrl}.");

        _helper = helper;
        _g = GetGremlinSource();
    }

    [Theory]
    [InlineData("Lilou")]
    [InlineData("Bijou")]
    [InlineData("Christi")]
    [InlineData("Shawn")]
    [InlineData("Erik")]
    public async Task FindParents(string childName)
    {
        var dictionary = GremlinData.AsDictionary;
        var lilou = dictionary[childName];

        var parents = await _g
            .V<Person>()
            .Where(person => person.FullName == lilou.FullName)
            .InE<IsParentOf>()
            .OutV<Person>();


        Assert.NotEmpty(parents);

        var json = JsonConvert.SerializeObject(parents, Formatting.Indented);

        _helper.WriteLine(json);
    }

    private IGremlinQuerySource GetGremlinSource()
    {
        return GremlinQuerySource.g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel
                    //Specify base classes for vertices(Vertex) and edges(Edge)
                    .FromBaseTypes<Vertex, Edge>(lookup =>
                    {
                        var current = AppDomain.CurrentDomain.GetAssemblies().ToList();
                        var types = lookup.IncludeAssemblies(current);
                        return types;
                    }))
                .UseCosmosDb(builder => builder
                    //Specify CosmosDb Gremlin endpoint URL, DB name and graph name. 
                    .At(new Uri(_gremlinEndpointUrl), _databaseName, _graphName)
                    //Specify CosmosDb access key
                    .AuthenticateBy(_cosmosDbAuthKey)
                ));
    }

    [Fact]
    public async Task PopulatePeople()
    {
        var tasks = GremlinData.People.Select(person => _g
                .AddV(person)
                .FirstAsync())
            .ToList();

        var results = tasks.Select(async task => await task);
        var persons = await Task.WhenAll(results);

        foreach (var person in persons)
        {
            var json = JsonConvert.SerializeObject(person, Formatting.Indented);
            _helper.WriteLine(json);
            _helper.WriteLine(string.Empty);
        }

        await RelatePeople();
    }

    [Fact]
    public async Task RelatePeople()
    {
        await _g.Parents(GremlinData.Christi, GremlinData.Lilou); // , GremlinData.Bijou);
        await _g.Parents(GremlinData.Shawn, GremlinData.Bijou, GremlinData.Lilou);

        await _g.Parents(GremlinData.Sherrye, GremlinData.Christi, GremlinData.ReubenIii);
        await _g.Parents(GremlinData.ReubenJr, GremlinData.Christi, GremlinData.ReubenIii);

        await _g.Parents(GremlinData.Mom, GremlinData.Shawn, GremlinData.Erik);
        await _g.Parents(GremlinData.Dad, GremlinData.Shawn, GremlinData.Erik);

        await _g.Knows(GremlinData.Sherrye, GremlinData.Mom, GremlinData.Erik, GremlinData.Shawn);
    }
}