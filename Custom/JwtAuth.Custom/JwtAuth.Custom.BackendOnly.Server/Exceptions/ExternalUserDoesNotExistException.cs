namespace JwtAuth.Custom.BackendOnly.Server.Exceptions
{
    public class ExternalUserDoesNotExistException : Exception
    {
        public ExternalUserDoesNotExistException() : base("External user does not exist.")
        {

        }

        public ExternalUserDoesNotExistException(string message) : base(message)
        {

        }

        public ExternalUserDoesNotExistException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
