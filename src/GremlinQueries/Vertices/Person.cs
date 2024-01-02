using GremlinQueries.Abstract;

namespace GremlinQueries.Vertices;

public class Person : Vertex
{
    private Person()
    {
        Id = Guid.NewGuid().ToString();
        Label = nameof(Person);
    }

    public Person(int age, string fullName) : this()
    {
        Age = age;
        FullName = fullName;
    }

    public int Age { get; set; }
    public string FullName { get; set; }
}