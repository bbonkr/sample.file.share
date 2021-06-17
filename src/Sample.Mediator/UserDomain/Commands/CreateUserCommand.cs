using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Mediator.UserDomain.Models;
using Sample.Mediator.UserDomain.Queries;

namespace Sample.Mediator.UserDomain.Commands
{
    public class CreateUserCommand : UserBaseModel, IRequest<UserModel>
    {
        
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserModel>
    {
        public CreateUserCommandHandler(
            DefaultDbContext dbContext,
            ILogger<CreateUserCommandHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<UserModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (dbContext.Users.Where(x => x.Email == request.Email.Trim()).Any())
            {
                //throw new Exception($"Email could not use. It's registered already.");
                throw new HttpStatusException<object>(System.Net.HttpStatusCode.BadRequest, $"Email could not use. It's registered already.", null);
            }

            var user = new Entities.User
            {
                UserName = request.Email.Trim(),
                DisplayName = request.DisplayName.Trim(),
                Email = request.Email.Trim(),
            };

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync(cancellationToken);

            var result = await dbContext.Users
                .Where(x => x.Email == request.Email.Trim())
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    UserName = x.UserName,
                    DisplayName = x.DisplayName,
                    Email = x.Email,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }
}
