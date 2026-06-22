namespace StayBook.Application.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool Verify(string hash, string password);
}