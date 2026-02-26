using BillingSystem.Application.DTOs.Requests.Products;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Domain.Entities.Products;
using BillingSystem.Domain.Interfaces.Repositories.Products;
using MediatR;
using System.Linq.Expressions;

namespace BillingSystem.Application.Queries.Products.GetProductsPaged
{
    public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, PagedResponseDto<ProductResponseDto>>
    {
        public readonly IProductReadRepository _productReadRepository;

        public GetProductsPagedQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<PagedResponseDto<ProductResponseDto>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
        {
            var filter = ApplyFilters(request.RequestDto.Filters);

            var pagedResult = await _productReadRepository.GetPagedAsync(
                request.RequestDto.Page,
                request.RequestDto.PageSize,
                filter,
                cancellationToken
            );

            var dtos = pagedResult.Items.Select(p => new ProductResponseDto(p));

            return new PagedResponseDto<ProductResponseDto>(dtos, request.RequestDto.Page, request.RequestDto.PageSize, pagedResult.TotalCount);
        }

        private static Expression<Func<Product, bool>>? ApplyFilters(ProductRequestDto? requestDto)
        {
            if (requestDto == null)
                return null;

            var nameFilter = requestDto.Name?.ToLower();

            Expression<Func<Product, bool>> filter = p =>
                (string.IsNullOrEmpty(nameFilter) || p.Name.ToLower().Contains(nameFilter));

            return filter;
        }
    }
}
