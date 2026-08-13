namespace StayBook.Domain.Conversations;

public class Conversation
{
    public int Id { get; private set; }
    public int BookingId { get; private set; }
    private readonly List<Message> _messages = [];
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
    public DateTime CreatedAt { get; private set; }
    
    public Conversation(int bookingId)
    {
        BookingId = bookingId;
        CreatedAt = DateTime.Now;
    }

    public void AddMessage(int senderId, string content)
    {
        var message = new Message(Id, senderId, content);
        _messages.Add(message);
    }
}