using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;

namespace Sample.Mediator.UserDomain.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string Email { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        public DeleteUserCommandHandler(
            DefaultDbContext dbContext,
            ILogger<DeleteUserCommandHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var deleteCandidate = await dbContext.Users.Where(x => x.Email == request.Email.Trim()).FirstOrDefaultAsync(cancellationToken);
            if (deleteCandidate == null)
            {
                throw new Exception("Could not find the user.");
            }

            dbContext.Users.Remove(deleteCandidate);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }
}
