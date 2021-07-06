using System.Threading.Tasks;
using Application.ParticipationExecution;
using AutoMapper;
using CodingChainApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodingChainApi.Controllers
{
    public class ExecutionsController : ApiControllerBase
    {
        public ExecutionsController(ISender mediator, IMapper mapper, IPropertyCheckerService propertyCheckerService) :
            base(mediator, mapper, propertyCheckerService)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post(RunParticipationTestsCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok();
        }
    }
}