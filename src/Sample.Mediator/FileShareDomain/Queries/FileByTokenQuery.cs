using System;

using MediatR;

namespace Sample.Mediator.FileShareDomain.Queries
{
    public class FileByTokenQuery : IRequest<FileByTokenQueryResult>
    {
        /// <summary>
        /// User authentication
        /// 
        /// <para>Use your email address Temporarily. ;)</para>
        /// </summary>
        public string UserImpersonate { get; set; }

        public string FileToken { get; set; }
    }
}
