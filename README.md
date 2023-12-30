#package 
dotnet add package Microsoft.VisualStudio.web.codegeneration.design
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package MySql.Data.EntityFramework

dùng migration 
dotnet ef migrations add Init
dotnet ef database update

tạo area 
dotnet aspnet-codegenerator area Database
controller 
dotnet aspnet-codegenerator controller -name DbManage -outDir Areas/Database/Controllers/ -namespace AppMvcNet.Areas.Database.Controllers

 dotnet aspnet-codegenerator controller -name Contact -namespace AppMvcNet.Areas.Contact.
Controllers -m AppMvcNet.Models.Contacts.Contact -udl -dc AppMvcNet.Models.AppDbContext -outDir Areas/Contact/Controllers

# aspnetcore cài biên dịch cho scss 
npm init 
npm install --global gulp-cli
npm install gulp 
npm install node-sass postcss sass
npm install gulp-sass gulp-less gulp-concat gulp-cssmin gulp-uglify rimraf gulp-postcss gulp-rename
#cài đặt pacakage 
dotnet add package System.Data.SqlClient
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add package Microsoft.Extensions.Logging.Console

dotnet add package Microsoft.AspNetCore.Identity
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.AspNetCore.Identity.UI
dotnet add package Microsoft.AspNetCore.Authentication
dotnet add package Microsoft.AspNetCore.Http.Abstractions
dotnet add package Microsoft.AspNetCore.Authentication.Cookies
dotnet add package Microsoft.AspNetCore.Authentication.Facebook
dotnet add package Microsoft.AspNetCore.Authentication.Google
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
dotnet add package Microsoft.AspNetCore.Authentication.oAuth
dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
dotnet add package Microsoft.AspNetCore.Authentication.Twitter

dotnet add package MailKit
dotnet add package MimeKit