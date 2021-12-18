namespace InterOP.Core.InterfaceApplications
{
    public interface IPasswordService
    {
        bool PasswordCheck(string hash, string spassword);
        string PasswordHash(string password);
    }
}