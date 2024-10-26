using System.ComponentModel.DataAnnotations;

namespace minVagtPlan.Models.ViewModels
{
    public class EditShiftViewModel
    {
        [Required]
        public Guid ShiftId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
    }
}
