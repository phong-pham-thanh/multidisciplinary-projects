using Back_end.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.DataModel
{
    [Table("TemperatureRecord")]
    public class TemperatureRecord
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Temperature { get; set; }
        [ForeignKey("UserId")]

        public Users UserRef { get; set; }
        public DateTime DateRecord { get; set; }
    }
}
