using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_end.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public int? TemperatureWarning { get; set; }
        public bool? WarningWhenOverHeat { get; set; }
        public int? AutoRunFanWhenOverHeat { get; set; }
    }
}
