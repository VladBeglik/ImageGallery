using System.Reflection;
using ImageGallery.App.Infrastructure.Mapping.Infrastructure;

namespace ImageGallery.App.Infrastructure.Mapping;

public class AutoMapperProfile : BaseAutoMapperProfile
{
    protected override Assembly RootAssembly => Assembly.GetExecutingAssembly();
}