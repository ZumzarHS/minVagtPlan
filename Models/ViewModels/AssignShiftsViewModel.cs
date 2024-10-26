using Microsoft.AspNetCore.Mvc.Rendering;

namespace minVagtPlan.Models.ViewModels
{
    public class AssignShiftsViewModel
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<Guid> AssignedShiftIds { get; set; } = new List<Guid>();
        public List<SelectListItem> AvailableShifts { get; set; } = new List<SelectListItem>();
    }
}
