namespace Digest.Custom.Server.Application;

public interface IHashService
{
    string ToMd5Hash(string inputString);
}
