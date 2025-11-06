using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace IT_asserts_Claim.Models
{
    public class Accessory
    {

        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string AccessoryType { get; set; }
        public string AccessoryName { get; set; }
        public string SerialNo { get; set; }
        public DateTime IssueDate { get; set; }

        [ForeignKey("Employee")]
        [JsonIgnore]
        public int EmpId { get; set; }
        [JsonIgnore]
        public Employee? Employee { get; set; }
    }

}

