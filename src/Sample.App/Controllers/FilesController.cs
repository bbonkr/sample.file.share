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
using System.Net;
using kr.bbon.AspNetCore.Models;
using kr.bbon.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;
using Sample.Mediator.FileShareDomain.Models;

namespace Sample.App.Controllers
{
    /// <summary>
    /// Files api
    /// </summary>
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

        /// <summary>
        /// Get owned file list 
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseModel<IPagedModel<FileItemModel>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
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

            return StatusCode(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Download file with token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{token}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type= typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
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

        /// <summary>
        /// Upload files
        /// </summary>
        /// <param name="files"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponseModel<IList<FileItemModel>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
        public async Task<IActionResult> Upload(
            [FromForm] IList<IFormFile> files,
            [FromHeader(Name = "X-Api-Key")] string auth)
        {
            var results = new List<FileItemModel>();

            foreach (var file in files)
            {
                using (var stream = file.OpenReadStream())
                {
                    stream.Position = 0;

                    var command = new UploadFileCommand
                    {
                        Name = file.FileName,
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

            return StatusCode(HttpStatusCode.Created, results);
        }

        /// <summary>
        /// Generate share file information
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="model"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{fileId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponseModel<ShareFileResult>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
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

            return StatusCode(HttpStatusCode.Created, result);
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="auth"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{fileId}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
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

            return StatusCode(HttpStatusCode.Accepted);
        }



        private readonly IMediator mediator;
    }

    public class GenerateTokenRequest
    {
        /// <summary>
        /// Who shares with
        /// </summary>
        [Required]
        public Guid To { get; set; }

        /// <summary>
        /// Shares until.
        /// </summary>
        public long ExpiresOn { get; set; }
    }
}
