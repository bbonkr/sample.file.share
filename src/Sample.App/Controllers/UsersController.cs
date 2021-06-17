using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using kr.bbon.AspNetCore;
using kr.bbon.AspNetCore.Filters;
using kr.bbon.AspNetCore.Models;
using kr.bbon.AspNetCore.Mvc;
using kr.bbon.EntityFrameworkCore.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Sample.Mediator.UserDomain.Commands;
using Sample.Mediator.UserDomain.Models;
using Sample.Mediator.UserDomain.Queries;

namespace Sample.App.Controllers
{
    /// <summary>
    /// Users api
    /// </summary>
    [ApiController]
    [Area(DefaultValues.AreaName)]
    [Route(DefaultValues.RouteTemplate)]
    [ApiVersion("1.0")]
    [ApiExceptionHandlerFilter]
    public class UsersController : ApiControllerBase
    {
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get user list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseModel<IPagedModel<UserModel>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
        public async Task<IActionResult> GetUsers(
            [FromHeader(Name = "X-Api-Key")] string auth, 
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string keyword = "")
        {
            var query = new GetUsersQuery
            {
                UserImpersonate= auth,
                Page = page,
                Limit = limit,
                Keyword = keyword,
            };
            var result = await mediator.Send(query);

            return StatusCode(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{email}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseModel<UserModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
        public async Task<IActionResult> FindByEmail(string email)
        {
            var query = new FindByEmailQuery
            {
                Email = email,
            };

            var result = await mediator.Send(query);

            if (result == null)
            {
                return StatusCode(HttpStatusCode.NotFound, "Please check your email address");
            }

            return StatusCode(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ApiResponseModel<UserModel>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand model)
        {
            var result = await mediator.Send(model);

            return StatusCode(HttpStatusCode.Created, result);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{userId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseModel))]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var command = new DeleteUserCommand
            {
                UserId = userId,
            };

            await mediator.Send(command);

            return StatusCode(HttpStatusCode.OK);
        }

        private readonly IMediator mediator;
    }
}
