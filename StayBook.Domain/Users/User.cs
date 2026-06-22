namespace StayBook.Domain.Users;

public class User
{
    public int Id { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }
    public UserRole Role { get; private set; }

    public User(string userName, string passwordHash, string email, UserRole role)
    {
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        Role = role;
    }
}