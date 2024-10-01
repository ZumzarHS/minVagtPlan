using Microsoft.AspNetCore.Mvc;
using minVagtPlan.Data;

namespace minVagtPlan.Controllers
{
    public class ShiftsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ShiftsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
