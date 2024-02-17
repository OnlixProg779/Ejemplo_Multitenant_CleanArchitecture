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

            CreateMap<CreateProductCommand, Domain.Bussines.Product>();
            CreateMap<Domain.Bussines.Product, ProductPropertyPatch>();
            CreateMap<Domain.Bussines.Product, CreateProductResponse>();
            CreateMap<Domain.Bussines.Product, PatchProductResponse>();
            CreateMap<Domain.Bussines.Product, ProductVm>();
            CreateMap<ProductPropertyPatch, Domain.Bussines.Product>();


        }
    }
}
