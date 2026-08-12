namespace StayBook.Domain.Conversations;

public class Message
{
    public int Id { get; private set; }
    public int ConversationId { get; private set; }
    public int SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    public Message(int conversationId, int senderId, string content)
    {
        ConversationId = conversationId;
        SenderId = senderId;
        Content = content;
        CreatedAt = DateTime.Now;
    }

    public void MarkAsRead(DateTime readAt)
    {
        ReadAt = readAt;
    }
}