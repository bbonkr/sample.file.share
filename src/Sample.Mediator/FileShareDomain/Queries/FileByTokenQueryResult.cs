namespace Sample.Mediator.FileShareDomain.Queries
{
    public class FileByTokenQueryResult
    {

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public byte[] Buffer { get; set; }
    }
}
