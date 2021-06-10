using System;

using MediatR;

namespace Sample.Mediator.FileShareDomain.Queries
{
    public class FileByTokenQuery : IRequest<FileByTokenQueryResult>
    {
        public Guid UserId { get; set; }

        public string FileToken { get; set; }
    }
}
