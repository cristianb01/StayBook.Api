namespace StayBook.Application.Interfaces;

public interface IMySqlPropertyLock
{
    Task ExecuteAsync(int propertyId, CancellationToken cancellationToken);
}