using System.Net;
using Microsoft.AspNetCore.Http;

namespace AppMvcNet.Status{
    public static class AppExtends{
        public static void AddStatusCodePage(this IApplicationBuilder app){
            app.UseStatusCodePages(appError => {
    appError.Run(async context => {
        var response = context.Response;
        var code = response.StatusCode;
        var content = @$"<html>
        <head>
            <meta charset='UTF-8'/>
            <title>Lỗi {code} </title>
        </head>
        <body>
            <p style='color:red; font-size: 30px'>
                Có lỗi xảy ra : {code} - {(HttpStatusCode)code}
            </p>
        </body>
        </html";
        await response.WriteAsync(content);
    });
});
        }
    }
}