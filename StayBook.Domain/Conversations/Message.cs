namespace StayBook.Domain.Conversations;

public class Message
{
    public int Id { get; private set; }
    public int SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    public Message(int senderId, string content)
    {
        SenderId = senderId;
        Content = content;
        CreatedAt = DateTime.Now;
    }

    public void MarkAsRead(DateTime readAt)
    {
        ReadAt = readAt;
    }
}