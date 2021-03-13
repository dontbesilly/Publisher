namespace Publisher.Models
{
    public class PublishProject
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public  PublishProjectType ProjectType { get; set; }
    }

    public enum PublishProjectType
    {
        Web, Worker, Lib, Other
    }
}
