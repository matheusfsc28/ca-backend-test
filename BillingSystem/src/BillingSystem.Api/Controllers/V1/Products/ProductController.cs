using BillingSystem.Api.Controllers.V1.Base;
using BillingSystem.Application.Commands.Products.DeleteProduct;
using BillingSystem.Application.Commands.Products.RegisterProduct;
using BillingSystem.Application.Commands.Products.UpdateProduct;
using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Products;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Products;
using BillingSystem.Application.Queries.Products.GetProductById;
using BillingSystem.Application.Queries.Products.GetProductsPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingSystem.Api.Controllers.V1.Products
{
    public class ProductController : BillingSystemV1BaseController
    {
        public ProductController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Get paged products response.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseDto<ProductResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestDto<ProductRequestDto> requestDto, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetProductsPagedQuery(requestDto), cancellationToken));
        }

        /// <summary>
        /// Get product by id.
        /// </summary>
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetProductByIdQuery(Id), cancellationToken));
        }

        /// <summary>
        /// Register a new product.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterProductCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { Id = id }, id);
        }

        /// <summary>
        /// Update a product by id.
        /// </summary>
        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid Id, [FromBody] ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateProductCommand(Id, productRequestDto), cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete a product by id.
        /// </summary>
        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteProductCommand(Id), cancellationToken);
            return NoContent();
        }
    }
}
