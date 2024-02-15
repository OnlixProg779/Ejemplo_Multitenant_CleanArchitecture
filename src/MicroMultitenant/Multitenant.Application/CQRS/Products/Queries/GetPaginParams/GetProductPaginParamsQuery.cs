using MediatR;
using Multitenant.Application.CQRS.Products.Queries.Vms;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams

{
    public class GetProductPaginParamsQuery : IRequest<PaginationVm<ProductVm>>
    {
        public ProductPaginParams ProductPaginParams { get; set; }

        private int _pageSize = 3;

        private int MaxPageSize = 50;

        public string? SearchQuery { get; set; }

        public int PageIndex { get; set; } = 1;


        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }

        public string? Token { get; set; }

        public GetProductPaginParamsQuery()
        {
            ProductPaginParams = new ProductPaginParams();
        }
    }

    public class ProductPaginParams
    {

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }

        public bool? Active { get; set; }

        public DateTime? CreatedDateFrom { get; set; }

        public DateTime? CreatedDateTo { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDateFrom { get; set; }

        public DateTime? LastModifiedDateTo { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
