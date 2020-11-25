using AutoMapper;
using NerdStore.Catalog.Application.ViewModels;
using NerdStore.Catalog.Domain;

namespace NerdStore.Catalog.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(p =>
                    new Product(p.Name, p.Description, p.Active,
                        p.Price, p.CategoryId, p.RegisterDate,
                        p.Image, new Dimensions(p.Hight, p.Width, p.Depth)));

            CreateMap<CategoryViewModel, Category>()
                .ConstructUsing(c => new Category(c.Name, c.Code));
        }
    }
}
