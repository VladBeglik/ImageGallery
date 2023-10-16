using BookStore.App.Infrastructure.Exceptions;

namespace ImageGallery.App.Infrastructure.Exceptions;

public class CustomException : Exception, ICustomExceptionMarker
{
    public CustomException() : base("Server error") { }

    public CustomException(string message) : base(message) { }
}