using AppMvcNet;
using AppMvcNet.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectString = builder.Configuration.GetConnectionString("AppMVCConnectionStrings");
    if (connectString != null)
    {
        // Sử dụng connectString nếu nó không null
        options.UseSqlServer(connectString);
    }
    else
    {
        // Xử lý trường hợp chuỗi kết nối không tồn tại
        throw new Exception("Chuỗi kết nối không được tìm thấy");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.AddStatusCodePage(); // tuy bien cac loi 400 - 599

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();