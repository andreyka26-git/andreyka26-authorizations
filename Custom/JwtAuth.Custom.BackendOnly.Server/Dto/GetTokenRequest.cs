namespace JwtAuth.Custom.BackendOnly.Server.Dto
{
    public class GetTokenRequest
    {
        public string UserName { get; set; } = Consts.UserName; 
        public string Password { get; set; } = Consts.Password;
    }
}
