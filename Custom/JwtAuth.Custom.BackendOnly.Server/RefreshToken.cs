using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Custom.BackendOnly.Server
{
    public class RefreshToken
    {
        public string Id { get; private set; }

        public RefreshToken(string value, TimeSpan duration, string userId)
        {
            Id = Guid.NewGuid().ToString();
            Value = value;
            Expiration = DateTime.UtcNow.Add(duration);
            ApplicationUserId = userId;
        }

        private RefreshToken()
        {

        }

        public string Value { get; private set; }
        public DateTime Expiration { get; private set; }
        public string ApplicationUserId { get; private set; }
        public IdentityUser User { get; set; }
    }
}
