using AutoMapper;

namespace ImageGallery.App.Infrastructure.Mapping.Interfaces;

public interface IHaveCustomMapping
{
    void CreateMappings(Profile configuration);
}