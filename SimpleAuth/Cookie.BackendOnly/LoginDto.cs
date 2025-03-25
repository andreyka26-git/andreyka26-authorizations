using System.ComponentModel;

namespace Cookie.BackendOnly;

public class LoginDto 
{
    [DefaultValue("andreyka26_")]
    public string UserName { get; set; } = "andreyka26_";
     
    [DefaultValue("Mypass1*")]
    public string Password { get; set; } = "Mypass1*";
}
