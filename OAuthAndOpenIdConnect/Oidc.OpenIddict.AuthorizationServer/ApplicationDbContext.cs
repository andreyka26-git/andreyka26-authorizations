using Microsoft.EntityFrameworkCore;

namespace Oidc.OpenIddict.AuthorizationServer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
