namespace StayBook.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base (message)
    {
        
    }

    public InvalidCredentialsException()
    {
        
    }
}