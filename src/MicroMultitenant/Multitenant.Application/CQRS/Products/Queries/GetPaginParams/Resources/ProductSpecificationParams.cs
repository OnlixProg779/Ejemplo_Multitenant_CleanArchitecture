using System.Linq.Expressions;
using Multitenant.Application.CQRS.Products.Queries.GetPaginParams;

namespace Multitenant.Application.CQRS.Products.Queries.GetPaginParams.Resources
{
    public class ProductSpecificationParams
    {
        public ProductPaginParams _ProductPaginParams { get; set; }
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

        public ProductSpecificationParams(ProductPaginParams contactoPaginParams)
        {
            _ProductPaginParams = contactoPaginParams ??
                throw new ArgumentNullException(nameof(contactoPaginParams));
        }

        public ProductSpecificationParams()
        {
            _ProductPaginParams = new ProductPaginParams();
        }

        public List<Expression<Func<Domain.Bussines.Products, bool>>> GetCriteria()
        {
            var listCriteria = GetStandarCriteria(_ProductPaginParams);

            if (!string.IsNullOrEmpty(_ProductPaginParams.Duration))
            {
                listCriteria.Add(x => x.Duration == _ProductPaginParams.Duration);
            }
            if (!string.IsNullOrEmpty(_ProductPaginParams.Name))
            {
                listCriteria.Add(x => x.Name == _ProductPaginParams.Name);
            }
            if (!string.IsNullOrEmpty(_ProductPaginParams.Description))
            {
                listCriteria.Add(x => x.Description == _ProductPaginParams.Description);
            }
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                listCriteria.Add(x =>
                                     x.Duration.ToLower().Contains(SearchQuery.Trim().ToLower())
                                    || x.Name.ToLower().Contains(SearchQuery.Trim().ToLower())
                                    || x.Description.ToLower().Contains(SearchQuery.Trim().ToLower())
                                  );
            }

            return listCriteria;
        }

        public List<Expression<Func<Domain.Bussines.Products, bool>>> GetStandarCriteria(ProductPaginParams standarBaseQuery)
        {
            List<Expression<Func<Domain.Bussines.Products, bool>>> list = new List<Expression<Func<Domain.Bussines.Products, bool>>>();
            if (standarBaseQuery.Active.HasValue)
            {
                list.Add((x) => x.Active == standarBaseQuery.Active);
            }

            if (!string.IsNullOrWhiteSpace(standarBaseQuery.CreatedBy))
            {
                list.Add((x) => x.CreatedBy.ToLower().Contains(standarBaseQuery.CreatedBy.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(standarBaseQuery.LastModifiedBy))
            {
                list.Add((x) => x.LastModifiedBy.ToLower().Contains(standarBaseQuery.LastModifiedBy.ToLower()));
            }

            if ((standarBaseQuery.CreatedDateFrom.HasValue || Convert.ToString(standarBaseQuery.CreatedDateFrom) != "") && (standarBaseQuery.CreatedDateTo.HasValue || Convert.ToString(standarBaseQuery.CreatedDateTo) != ""))
            {
                list.Add((a) => a.CreatedDate >= standarBaseQuery.CreatedDateFrom && a.CreatedDate < standarBaseQuery.CreatedDateTo.Value.AddDays(1.0));
            }

            if ((standarBaseQuery.CreatedDateFrom.HasValue || Convert.ToString(standarBaseQuery.CreatedDateFrom) != "") && (!standarBaseQuery.CreatedDateTo.HasValue || Convert.ToString(standarBaseQuery.CreatedDateTo) == ""))
            {
                list.Add((a) => a.CreatedDate >= standarBaseQuery.CreatedDateFrom && a.CreatedDate < DateTime.Now.AddDays(1.0));
            }

            if ((!standarBaseQuery.CreatedDateFrom.HasValue || Convert.ToString(standarBaseQuery.CreatedDateFrom) == "") && (standarBaseQuery.CreatedDateTo.HasValue || Convert.ToString(standarBaseQuery.CreatedDateTo) != ""))
            {
                list.Add((a) => a.CreatedDate >= standarBaseQuery.CreatedDateTo && a.CreatedDate <= standarBaseQuery.CreatedDateTo.Value.AddDays(1.0));
            }

            if ((standarBaseQuery.LastModifiedDateFrom.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateFrom) != "") && (standarBaseQuery.LastModifiedDateTo.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateTo) != ""))
            {
                list.Add((a) => a.LastModifiedDate >= standarBaseQuery.LastModifiedDateFrom && a.LastModifiedDate < standarBaseQuery.LastModifiedDateTo.Value.AddDays(1.0));
            }

            if ((standarBaseQuery.LastModifiedDateFrom.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateFrom) != "") && (!standarBaseQuery.LastModifiedDateTo.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateTo) == ""))
            {
                list.Add((a) => a.LastModifiedDate >= standarBaseQuery.LastModifiedDateFrom && a.LastModifiedDate < DateTime.Now.AddDays(1.0));
            }

            if ((!standarBaseQuery.LastModifiedDateFrom.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateFrom) == "") && (standarBaseQuery.LastModifiedDateTo.HasValue || Convert.ToString(standarBaseQuery.LastModifiedDateTo) != ""))
            {
                list.Add((a) => a.LastModifiedDate >= standarBaseQuery.LastModifiedDateTo && a.LastModifiedDate <= standarBaseQuery.LastModifiedDateTo.Value.AddDays(1.0));
            }

            return list;
        }


    }
}
