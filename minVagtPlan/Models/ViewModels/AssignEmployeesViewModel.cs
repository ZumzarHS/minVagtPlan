using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace minVagtPlan.Models.ViewModels
{
    public class AssignEmployeesViewModel
    {
        public Guid ShiftId { get; set; }
        public string ShiftDetails { get; set; }
        public List<Guid> AssignedEmployeeIds { get; set; } = new List<Guid>();
        public List<SelectListItem> AvailableEmployees { get; set; } = new List<SelectListItem>();
    }
}
