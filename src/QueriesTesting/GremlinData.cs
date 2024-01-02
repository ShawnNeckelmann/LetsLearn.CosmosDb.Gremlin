using GremlinQueries.Vertices;

namespace QueriesTesting;

public static class GremlinData
{
    private const string NameBijou = "Bijou";
    private const string NameChristi = "Christi";
    private const string NameDad = "Dad";
    private const string NameErik = "Erik";
    private const string NameLilou = "Lilou";
    private const string NameMom = "Mom";
    private const string NameRubenIii = "Ruben III";
    private const string NameRubenJr = "Ruben Jr.";
    private const string NameShawn = "Shawn";
    private const string NameSherrye = "Sherrye";

    public static readonly List<Person> People =
    [
        new Person(76, NameDad),
        new Person(40, NameErik),
        new Person(43, NameShawn),
        new Person(2, NameBijou),
        new Person(0, NameLilou),
        new Person(40, NameChristi),
        new Person(55, NameSherrye),
        new Person(65, NameMom),
        new Person(43, NameRubenJr),
        new Person(43, NameRubenIii)
    ];

    public static readonly Person Bijou = People.First(person => person.FullName.Equals(NameBijou));
    public static readonly Person Christi = People.First(person => person.FullName.Equals(NameChristi));
    public static readonly Person Erik = People.First(person => person.FullName.Equals(NameErik));
    public static readonly Person Lilou = People.First(person => person.FullName.Equals(NameLilou));
    public static readonly Person Mom = People.First(person => person.FullName.Equals(NameMom));
    public static readonly Person Dad = People.First(person => person.FullName.Equals(NameDad));


    public static readonly Person ReubenIii = People.First(person => person.FullName.Equals(NameRubenIii));
    public static readonly Person ReubenJr = People.First(person => person.FullName.Equals(NameRubenJr));
    public static readonly Person Shawn = People.First(person => person.FullName.Equals(NameShawn));
    public static readonly Person Sherrye = People.First(person => person.FullName.Equals(NameSherrye));

    public static Dictionary<string, Person> AsDictionary => People.ToDictionary(person => person.FullName);
}