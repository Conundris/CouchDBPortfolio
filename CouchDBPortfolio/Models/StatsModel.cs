using System.Runtime.Serialization;

namespace CouchDBPortfolio.Models
{
    [DataContract]
    public class StatsModel
    {
        public string Section { get; set; }
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}