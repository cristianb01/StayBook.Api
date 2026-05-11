using StayBook.Domain.Common;

namespace StayBook.Domain.Properties;

public class Property : AggregateRoot
{
    public int Id { get; private set; }
    public int HostId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public Property(int hostId, string name, string description)
    {
        HostId = hostId;
        Name = name;
        Description = description;
    }
}