namespace Sample.Services
{
    public interface ITokenService
    {
        string GetToken(int length = 20);
    }
}