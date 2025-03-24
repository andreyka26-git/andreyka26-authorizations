
namespace Cookie.BackendOnly;

public class User
{
    public string Email { get; set; }
    public string FullName { get; set; }

    public static async Task<User?> AuthenticateUser(string email, string password)
    {
        // For demonstration purposes, authenticate a user
        // with a static email address. Ignore the password.
        // Assume that checking the database takes 500ms

        await Task.Delay(500);

        if (email == "andreyka26_" && password == "Mypass1*")
        {
            return new User()
            {
                Email = "andreyka26_",
                FullName = "Andrii Bui"
            };
        }
        else
        {
            return null;
        }
    }
}
