namespace Digest.Custom.Server.Application;

public interface IUsernameHashedSecretProvider
{
    /// <summary>
    /// Mechanism to get the hashed "A1" (username:realm:secret) associated with a given username when generating the digest challenge
    /// </summary>
    /// <returns>The "A1" MD5 hash associated with the username, or null if the username is invalid</returns>
    /// <remarks>The "A1" MD5 hash should be computed using <see cref="DigestAuthentication.ComputeA1Md5Hash"/></remarks>
    Task<string> GetA1Md5HashForUsernameAsync(string username, string realm);
}
