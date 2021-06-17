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
using Sample.Mediator.UserDomain.Models;

namespace Sample.Mediator.UserDomain.Queries
{
    public class FindByEmailQuery : IRequest<UserModel>
    {
        public string Email { get; set; }
    }


    public class FindByEmailQueryHandler : IRequestHandler<FindByEmailQuery, UserModel>
    {
        public FindByEmailQueryHandler(
            DefaultDbContext dbContext,
            ILogger<FindByEmailQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<UserModel> Handle(FindByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.Where(x => x.Email == request.Email)
               .FirstOrDefaultAsync(cancellationToken);

            UserModel result = null;
            if (user != null)
            {
                result = new UserModel
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                };
            }

            return result;
        }

        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }


}
