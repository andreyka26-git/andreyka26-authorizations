using System.Security.Cryptography;
using System.Text;
using Digest.Custom.Server.Application;

namespace Digest.Custom.Server.Infrastructure;

public class HashService : IHashService
{
    public string ToMd5Hash(string inputString)
    {
        var bytes = Encoding.UTF8.GetBytes(inputString);
        var hashBytes = MD5.Create().ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
    }
}
