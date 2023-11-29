namespace Curacaru.Backend.Core.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string message)
        : base(message)
    {
    }
}