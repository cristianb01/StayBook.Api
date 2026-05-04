namespace StayBook.Application.Models;

public record OutboxEvent(
    string Type,
    string Payload,
    DateTime OccurredOn);