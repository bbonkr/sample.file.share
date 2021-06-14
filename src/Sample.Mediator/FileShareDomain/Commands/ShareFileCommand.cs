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
using Sample.Services;

namespace Sample.Mediator.FileShareDomain.Commands
{
    public class ShareFileCommand : IRequest<ShareFileResult>
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public Guid FileId { get; set; }

        public Guid To { get; set; }

        public DateTimeOffset ExpiresOn { get; set; }
    }

    public class ShareFileResult
    {
        public string Token { get; set; }

        public long ExpiresOn { get; set; }
    }

    public class ShareFileCommandHandler : IRequestHandler<ShareFileCommand, ShareFileResult>
    {
        public ShareFileCommandHandler(
            DefaultDbContext dbContext,
            ITokenService tokenService,
            ILogger<UploadFileCommandHander> logger)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        public async Task<ShareFileResult> Handle(ShareFileCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Where(x => x.Email == request.UserImpersonate)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new Exception("Access deny");
            }

            var to = await dbContext.Users
                .Where(x => x.Id == request.To)
                .FirstOrDefaultAsync(cancellationToken);

            if (to == null)
            {
                throw new Exception("Could not find a user who shares with.");
            }

            var file = await dbContext.Files
                .Where(x => x.Id == request.FileId && x.CreatedBy == user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var token = string.Empty;
            var exists = false;
            var length = 4;
            var tries = 0;
            do
            {
                token = tokenService.GetToken(length);

                exists = dbContext.Access
                    .Where(x => EF.Functions.Collate(x.Token, "Korean_Wansung_CS_AS_KS_WS") == token)
                    .Any();

                if (exists)
                {
                    if (++tries >= 5)
                    {
                        length += 1;
                        tries = 0;
                    }
                }

                if (tries > 100)
                {
                    throw new Exception("Could not generate file token.");
                }

            } while (exists);

            dbContext.Access.Add(new Entities.UserFileAccessControl
            {

                FileId = file.Id,
                UserId = to.Id,
                ExpiresOn = request.ExpiresOn,
                Token = token,
            });

            await dbContext.SaveChangesAsync(cancellationToken);

            return new ShareFileResult
            {
                Token = token,
                ExpiresOn = request.ExpiresOn.Ticks,
            };
        }

        private readonly DefaultDbContext dbContext;
        private readonly ITokenService tokenService;
        private readonly ILogger logger;
    }
}
