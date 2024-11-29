using System.ComponentModel.DataAnnotations;

namespace minVagtPlan.Models.ViewModels
{
    public class AddShiftViewModel
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

    }
}
