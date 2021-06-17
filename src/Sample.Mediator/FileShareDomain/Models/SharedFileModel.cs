namespace Sample.Mediator.FileShareDomain.Models
{
    public class SharedFileModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Token { get; set; }

        public long? ExpiresOn { get; set; }
    }
}
