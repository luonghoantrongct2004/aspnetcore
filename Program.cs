using App.Data;
using App.Models;
using App.Services;
using AppMvcNet.Models;
using AppMvcNet.Status;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add builder.Services to the container.
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

//Đăng kí mail
builder.Services.AddOptions();
var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

//Đăng kí identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true; // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true; // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false; // Xác thực số điện thoại

});

// Cấu hình Cookie
builder.Services.ConfigureApplicationCookie(options => {
    // options.Cookie.HttpOnly = true;  
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = $"/login/";                                 // Url đến trang đăng nhập
    options.LogoutPath = $"/logout/";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";   // Trang khi User bị cấm truy cập
});
builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
    // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
    options.ValidationInterval = TimeSpan.FromSeconds(5);
});
//Xác thực GG
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var gConfig = builder.Configuration.GetSection("Authentication:Google");
        options.ClientId = gConfig["ClientId"];
        options.ClientSecret = gConfig["ClientSecret"];
        options.CallbackPath = "/dang-nhap-tu-Google";
    })
//Add fb 
  .AddFacebook(options =>
  {
      var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
      options.AppId = fconfig["AppId"];
      options.AppSecret = fconfig["AppSecret"];
      options.CallbackPath = "/dang-nhap-tu-facebook";
  });

//Thay thế error identity 
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

//Thêm chính sách policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ViewManageMenu", builders =>
    {
        builders.RequireAuthenticatedUser();
        builders.RequireRole(RoleName.Administrator);
    });
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

//app.AddStatusCodePage(); // tuy bien cac loi 400 - 599

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();