namespace JwtAuth.Custom.BackendOnly.Server.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("User already exists.")
        {

        }

        public DuplicateUserException(string message) : base(message)
        {

        }

        public DuplicateUserException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
