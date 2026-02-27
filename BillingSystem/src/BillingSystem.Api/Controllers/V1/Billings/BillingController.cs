using BillingSystem.Api.Controllers.V1.Base;
using BillingSystem.Application.Commands.Billings.SyncBillings;
using BillingSystem.Application.DTOs.Requests.Base;
using BillingSystem.Application.DTOs.Requests.Billings;
using BillingSystem.Application.DTOs.Responses.Base;
using BillingSystem.Application.DTOs.Responses.Billings;
using BillingSystem.Application.Queries.Billings.GetBillingById;
using BillingSystem.Application.Queries.Billings.GetBillingsPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingSystem.Api.Controllers.V1.Billings
{
    public class BillingController : BillingSystemV1BaseController
    {
        public BillingController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponseDto<BillingResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginationRequestDto<BillingRequestDto> requestDto, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetBillingsPagedQuery(requestDto), cancellationToken));
        }

        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(BillingResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid Id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetBillingByIdQuery(Id), cancellationToken));
        }

        [HttpPost]
        [Route("sync")]
        [ProducesResponseType(typeof(SyncBillingsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorOnSyncBillingResponseDto), StatusCodes.Status207MultiStatus)]
        public async Task<IActionResult> SyncBillings(CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new SyncBillingsCommand(), cancellationToken));
        }
    }
}
