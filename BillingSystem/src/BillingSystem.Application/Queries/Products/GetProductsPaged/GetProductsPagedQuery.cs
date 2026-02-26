using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Products;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Products;
using MediatR;

namespace BillingSystem.Application.Queries.Products.GetProductsPaged
{
    public record GetProductsPagedQuery(PaginationRequestDto<ProductRequestDto> RequestDto) : IRequest<PagedResponseDto<ProductResponseDto>>;
}
