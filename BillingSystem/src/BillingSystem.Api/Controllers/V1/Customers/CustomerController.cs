using BillingSystem.Api.Controllers.V1.Base;
using BillingSystem.Application.Commands.Customers.DeleteCustomer;
using BillingSystem.Application.Commands.Customers.RegisterCustomer;
using BillingSystem.Application.Commands.Customers.UpdateCustomer;
using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Customers;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Customers;
using BillingSystem.Application.Queries.Customers.GetCustomerById;
using BillingSystem.Application.Queries.Customers.GetCustomersPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingSystem.Api.Controllers.V1.Customers
{
    public class CustomerController : BillingSystemV1BaseController
    {
        public CustomerController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestDto<CustomerRequestDto> request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetCustomersPagedQuery(request), cancellationToken));
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetCustomerByIdQuery(Id), cancellationToken));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { Id = id }, id);
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] Guid Id, [FromBody] CustomerRequestDto customerRequestDto, CancellationToken cancellationToken)
        {
            await _mediator.Send(new UpdateCustomerCommand(Id, customerRequestDto), cancellationToken);
            return NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCustomerCommand(Id), cancellationToken);
            return NoContent();
        }
    }
}
