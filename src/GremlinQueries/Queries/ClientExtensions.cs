using System.Linq.Expressions;
using ExRam.Gremlinq.Core;
using GremlinQueries.Abstract;
using GremlinQueries.Edges;
using GremlinQueries.Vertices;

namespace GremlinQueries.Queries;

// https://tinkerpop.apache.org/docs/current/recipes/
public static class ClientExtensions
{
    private static async Task<TEdge[]> AddEdge<TFromVertix, TToVertix, TEdge>(this IStartGremlinQuery g,
        Expression<Func<TFromVertix, bool>> matchFrom,
        Expression<Func<TToVertix, bool>> matchTo)
        where TFromVertix : Vertex
        where TToVertix : Vertex
        where TEdge : Edge, new()
    {
        var retval = await g
            .V<TFromVertix>()
            .Where(matchFrom)
            .AddE<TEdge>()
            .To(gremlinQuery => gremlinQuery.V<TToVertix>().Where(matchTo));

        return retval;
    }

    public static async Task<List<Knows[]>> Knows(this IGremlinQuerySource g, Person person, params Person[] people)
    {
        var tasks = people.Select(thatPerson =>
        {
            return g.AddEdge<Person, Person, Knows>(p => p.FullName == person.FullName,
                p => p.FullName == thatPerson.FullName);
        }).ToList();

        var retval = (await Task.WhenAll(tasks)).Where(knows => knows is not null).ToList();
        return retval;
    }


    public static async Task<List<IsParentOf[]>> Parents(this IGremlinQuerySource g, Person parent,
        params Person[] children)
    {
        var taskIsParentOf = children.Select(child =>
        {
            return g.AddEdge<Person, Person, IsParentOf>(p => p.FullName == parent.FullName,
                p => p.FullName == child.FullName);
        }).ToList();


        var results = await Task.WhenAll(taskIsParentOf);
        var retval = results.Where(parents => parents is not null).ToList();
        return retval;
    }
}