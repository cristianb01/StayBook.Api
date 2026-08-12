namespace StayBook.Domain.Conversations;

public class Conversation
{
    public int Id { get; private set; }
    public int PropertyId { get; private set; }
    public int GuestId { get; private set; }
    public int OwnerId { get; private set; }
    public DateTime CreatedAd { get; private set; }
    
    public Conversation(int propertyId, int guestId, int ownerId)
    {
        PropertyId = propertyId;
        GuestId = guestId;
        OwnerId = ownerId;
        CreatedAd = DateTime.Now;
    }
}