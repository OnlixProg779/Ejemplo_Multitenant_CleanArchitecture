using AutoMapper;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct;
using Multitenant.Application.CQRS.Commands.Products.CreateProduct.Resources;
using Multitenant.Application.CQRS.Commands.Products.PatchProduct.Resources;

namespace Multitenant.Application.CQRS.Queries.Products.Vms
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() {

            CreateMap<CreateProductCommand, Domain.Bussines.Products>();
            CreateMap<Domain.Bussines.Products, CreateProductResponse>();
            CreateMap<Domain.Bussines.Products, PatchProductResponse>();
            CreateMap<Domain.Bussines.Products, ProductVm>();
            CreateMap<ProductPropertyPatch, Domain.Bussines.Products>();


        }
    }
}
