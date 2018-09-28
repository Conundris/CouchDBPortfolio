namespace CouchDBPortfolio.Models
{
    public class TodoTask
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool isDone { get; set; }
        public string tag { get; set; }
    }
}