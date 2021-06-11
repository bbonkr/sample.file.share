using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using kr.bbon.AspNetCore;
using kr.bbon.AspNetCore.Mvc;
using kr.bbon.AspNetCore.Filters;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Sample.Mediator.FileShareDomain.Queries;

namespace Sample.App.Controllers
{
    [ApiController]
    [Area(DefaultValues.AreaName)]
    [Route(DefaultValues.RouteTemplate)]
    [ApiVersion("1.0")]
    [ApiExceptionHandlerFilter]
    public class FilesController:ApiControllerBase
    {
        public FilesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{token}")]
        public async Task<IActionResult> FileByToken(
            [FromRoute] string token,
            [FromHeader(Name = "X-Api-Key")] string auth)
        {
            var qeury = new FileByTokenQuery()
            {
                FileToken = token,
                UserImpersonate = auth,
            };

            var result = await mediator.Send(qeury);

            return File(result.Buffer, result.ContentType, result.FileName);
        }

        private readonly IMediator mediator;
    }
}
