using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Employee_Utility
{
    public class Employee
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("salary")]
        public int Salary { get; set; }
    }
}