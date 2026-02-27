namespace BillingSystem.Application.DTOs.Requests.Base
{
    public class PaginationRequestDto<T> where T : BaseRequestDto, new()
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public T Filters { get; set; } = new T();

    }
}
