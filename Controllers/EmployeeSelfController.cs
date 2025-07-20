using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;

namespace minVagtPlan.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeSelfController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeSelfController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyShifts()
        {
            // Get current user's ID
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            // Find the employee entity by userId
            var employee = await dbContext.Employees
                .Include(e => e.ShiftEmployees)
                .ThenInclude(se => se.Shift)
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (employee == null)
                return NotFound();

            var shifts = employee.ShiftEmployees
                .Select(se => se.Shift)
                .OrderBy(s => s.StartTime)
                .ToList();

            return View(shifts);
        }
    }
}
