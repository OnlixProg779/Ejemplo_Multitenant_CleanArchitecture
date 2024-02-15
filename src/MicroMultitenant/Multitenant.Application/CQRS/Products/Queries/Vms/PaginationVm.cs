namespace Multitenant.Application.CQRS.Products.Queries.Vms
{
    public class PaginationVm<T> : List<T>
    {
        public int TotalCount { get; private set; }

        public int PageSize { get; private set; }

        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public IList<string>? ResponseMessages { get; set; }

        public int ResponseAction { get; set; } = 0;


        public PaginationVm(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            AddRange(items);
            ResponseAction = 1;
        }

        public PaginationVm(List<string>? responseMessages)
        {
            ResponseMessages = responseMessages;
        }
    }

}
