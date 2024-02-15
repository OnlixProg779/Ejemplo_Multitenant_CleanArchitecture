using AutoMapper;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct;
using Multitenant.Application.CQRS.Products.Commands.CreateProduct.Resources;
using Multitenant.Application.CQRS.Products.Commands.PatchProduct.Resources;

namespace Multitenant.Application.CQRS.Products.Queries.Vms
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {

            CreateMap<CreateProductCommand, Domain.Bussines.Products>();
            CreateMap<Domain.Bussines.Products, ProductPropertyPatch>();
            CreateMap<Domain.Bussines.Products, CreateProductResponse>();
            CreateMap<Domain.Bussines.Products, PatchProductResponse>();
            CreateMap<Domain.Bussines.Products, ProductVm>();
            CreateMap<ProductPropertyPatch, Domain.Bussines.Products>();


        }
    }
}
