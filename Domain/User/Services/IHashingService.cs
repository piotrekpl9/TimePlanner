namespace Domain.User.Services;

public interface IHashingService
{
    string Encrypt(string value);
}