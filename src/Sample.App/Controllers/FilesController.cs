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
using Microsoft.AspNetCore.Http;
using Sample.Mediator.FileShareDomain.Commands;

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
        public async Task<IActionResult> MyFiles(
            [FromHeader(Name = "X-Api-Key")] string auth,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string keyword = "")
        {
            var query = new MyFilesQuery
            {
                Page = page,
                Limit = limit,
                Keyword = keyword,
                UserImpersonate = auth,
            };

            var result = await mediator.Send(query);

            return StatusCode(System.Net.HttpStatusCode.OK, result);
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

        [HttpPost]
        public async Task<IActionResult> Upload(
            [FromForm] IList<IFormFile> files,
            [FromHeader(Name = "X-Api-Key")] string auth)
        {
            var results = new List<UploadFileResult>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    stream.Position = 0;

                    var command = new UploadFileCommand
                    {
                        Name = file.Name,
                        ContentType = file.ContentType,
                        Size = stream.Length,
                        Stream = stream,
                        UserImpersonate = auth,
                    };

                    var result = await mediator.Send(command);
                    results.Add(result);
                    stream.Close();
                }
            }

            return StatusCode(System.Net.HttpStatusCode.Accepted, results);
        }

        [HttpPost]
        [Route("{fileId}")]
        public async Task<IActionResult> GenerateToken(
            [FromRoute] Guid fileId,
            [FromBody] GenerateTokenRequest model,
            [FromHeader(Name = "X-Api-Key")] string auth)
        {
            var command = new ShareFileCommand
            {
                FileId = fileId,
                To = model.To,
                ExpiresOn = DateTimeOffset.FromUnixTimeSeconds(model.ExpiresOn),
                UserImpersonate = auth,
            };

            var result = await mediator.Send(command);

            return StatusCode(System.Net.HttpStatusCode.Accepted, result);
        }

        [HttpDelete]
        [Route("{fileId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid fileId,
            [FromHeader(Name = "X-Api-Key")] string auth)
        {
            var command = new DeleteFileCommand
            {
                FileId = fileId,
                UserImpersonate = auth,
            };

            await mediator.Send(command);

            return StatusCode(System.Net.HttpStatusCode.OK);
        }



        private readonly IMediator mediator;
    }

    public class GenerateTokenRequest
    {
        /// <summary>
        /// Who
        /// </summary>
        public Guid To { get; set; }

        /// <summary>
        /// Until
        /// </summary>
        public long ExpiresOn { get; set; }
    }
}
