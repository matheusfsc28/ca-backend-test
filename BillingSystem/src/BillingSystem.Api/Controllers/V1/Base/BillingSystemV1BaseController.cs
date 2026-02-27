using BillingSystem.Application.DTOs.Responses.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BillingSystem.Api.Controllers.V1.Base
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
    public class BillingSystemV1BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        public BillingSystemV1BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
