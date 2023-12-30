using App.Data;
using App.Models;
using AppMvcNet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMvcNet.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManageController(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<IActionResult> SeedDataAsync()
        {
            var roleNames = typeof(RoleName).GetFields().ToList();
            foreach (var roleName in roleNames)
            {
                var roleNameValue = (string)roleName.GetRawConstantValue();
                var rFound = await _roleManager.FindByIdAsync(roleNameValue);
                if(rFound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleNameValue));
                }
            }
            var userAdmin = await _userManager.FindByEmailAsync("Admin");
            if(userAdmin == null)
            {
                userAdmin = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(userAdmin, "admin123");
                await _userManager.AddToRoleAsync(userAdmin, RoleName.Administrator);

            }
            StatusMethod = "Vừa seed database";
            return RedirectToAction("Index");
        }
    }
}
