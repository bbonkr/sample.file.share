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

namespace Sample.Mediator.FileShareDomain.Commands
{
    public class DeleteShareCommand : IRequest
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public Guid ShareId { get; set; }
    }

    public class DeleteShareCommandHandler : IRequestHandler<DeleteShareCommand>
    {
        public DeleteShareCommandHandler(
            DefaultDbContext dbContext,
            ILogger<DeleteShareCommandHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteShareCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
               .Where(x => x.Email == request.UserImpersonate)
               .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new HttpStatusException<object>(System.Net.HttpStatusCode.Unauthorized, "Access deny", default);
            }

            var shareItem = await dbContext.Access.Where(x => x.UserId == user.Id && x.Id == request.ShareId)
                .FirstOrDefaultAsync(cancellationToken);

            if(shareItem == null)
            {
                throw new HttpStatusException<object>(System.Net.HttpStatusCode.BadRequest, "Does not find sharing information", default);
            }

            dbContext.Access.Remove(shareItem);
            await dbContext.SaveChangesAsync();

            return Unit.Value;
        }
        
        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }
}
