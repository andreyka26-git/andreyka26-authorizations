using Digest.Custom.Server.Application;

namespace Digest.Custom.Server.Infrastructure;

internal class UsernameHashedSecretProvider : IUsernameHashedSecretProvider
{
    public Task<string> GetA1Md5HashForUsernameAsync(string username, string realm)
    {
        if (username == "andreyka26_" && realm == "some-realm")
        {
            // The hash value below would have been pre-computed & stored in the database.
            // var hash = _hashService.ToMd5Hash("andreyka26_:some-realm:mypass1");
            // so get digested md5("andreyka26_:some-realm:mypass1") by username and realm
            const string hash = "3173b15af925576b8b6eb4c65edb9e84";

            return Task.FromResult(hash);
        }

        // User not found
        throw new Exception("Username is not found");
    }
}
