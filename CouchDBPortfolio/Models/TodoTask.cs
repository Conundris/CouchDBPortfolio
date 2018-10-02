using System.Runtime.Serialization;

namespace CouchDBPortfolio.Models
{
    [DataContract]
    public class TodoTask
    {
        public string _id { get; set; }
        public string _rev { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool isDone { get; set; }
        [DataMember]    
        public string tag { get; set; }
    }
}