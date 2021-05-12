using System.Threading.Tasks;
using Application.Write;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CodingChainApi.Services;

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