namespace StayBook.Application.Exceptions;

public class ResourceNotFoundException(string message) : Exception(message)
{
    public ResourceNotFoundException(string resourceType, int id) 
        : this($"{resourceType} with id {id} not found")
    {
    }
}
