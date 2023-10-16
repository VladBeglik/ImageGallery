namespace ImageGallery.App.Infrastructure.Exceptions;

public class ForbiddenException : Exception, ICustomExceptionMarker
{
    public ForbiddenException() : base("Access denied") { }

    public ForbiddenException(string message) : base(message) { }
}
