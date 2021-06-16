namespace Sample.Mediator.FileShareDomain.Models
{
    public class FileItemModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string Uri { get; set; }

        public long CreatedAt { get; set; } 
    }
}
