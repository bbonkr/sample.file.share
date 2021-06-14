
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.EntityFrameworkCore.Extensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Sample.Data;
using Sample.Mediator.UserDomain.Models;

namespace Sample.Mediator.UserDomain.Queries
{
    public class GetUsersQuery : IRequest<IPagedModel<UserModel>>
    {
        public int Page { get; set; }

        public int Limit { get; set; }

        public string Keyword { get; set; }
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IPagedModel<UserModel>>
    {
        public GetUsersQueryHandler(
            DefaultDbContext dbContext,
            ILogger<FindByEmailQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }
        public async Task<IPagedModel<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var keyword = request.Keyword.Trim();
            var query = dbContext.Users
                .WhereDependOn(!string.IsNullOrEmpty(keyword), x => EF.Functions.Like(x.DisplayName, $"%{keyword }%") || EF.Functions.Like(x.Email, $"%{keyword }%"));

            var result = await query
                .OrderBy(x => x.DisplayName)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    UserName = x.UserName,
                    DisplayName = x.DisplayName,
                    Email = x.Email,
                }).ToPagedModelAsync(request.Page, request.Limit, cancellationToken);

            return result;
        }


        private readonly DefaultDbContext dbContext;
        private readonly ILogger logger;
    }

}
