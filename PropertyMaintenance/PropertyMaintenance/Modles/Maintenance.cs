using PropertyMaintenance.DB;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyMaintenance.Modles
{
    public class Maintenance
    {

        public int Id { get; set; }

        [Required]
        public string EventName { get; set; }
        [Required]
        public string PropertyName { get; set; }

        public string? Description { get; set; }

        public byte[]? Image { get; set; }

        public StatusEnum Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
