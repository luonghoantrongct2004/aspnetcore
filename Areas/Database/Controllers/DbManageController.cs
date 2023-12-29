using AppMvcNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMvcNet.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DbManageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: DbManage
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteDb(){
            return View();
        }

        [TempData]
        public string StatusMethod {get; set;}

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync(){
            var success= await _dbContext.Database.EnsureDeletedAsync();
            StatusMethod = success ? "Xóa thành công" : "Không xóa được";
            return RedirectToAction(nameof(Index));
        }

         [HttpPost]
        public async Task<IActionResult> Migrate(){
           await _dbContext.Database.MigrateAsync();
            StatusMethod = "Cập nhật thành công";
            return RedirectToAction(nameof(Index));
        }

    }
}
