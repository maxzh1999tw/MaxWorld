using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MaxWorld.Data;
using MaxWorld.Web.Controllers;
using MaxWorld.Web.Models;
using MaxWorld.Web.Repositories;
using MaxWorld.Web.Services;
using MaxWorld.Web.Utilities.MailSenders;
using System.Data;

namespace MaxWorld.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Main");
            builder.Services.AddDbContext<MaxWorldDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDbConnection>(x =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            });
            builder.Services.AddScoped<Repository>();
            builder.Services.AddScoped<BaseControllerArgument>();
            builder.Services.AddScoped<BaseServiceArgument>();
            builder.Services.AddSingleton<IMailSender, GmailSender>(x => 
                new GmailSender(
                    builder.Configuration["SMTP:Host"] ?? throw new ApplicationException(),
                    int.Parse(builder.Configuration["SMTP:Port"] ?? throw new ApplicationException()),
                    builder.Configuration["SMTP:SendFrom"] ?? throw new ApplicationException(),
                    builder.Configuration["SMTP:SendName"] ?? throw new ApplicationException(),
                    builder.Configuration["SMTP:Password"] ?? throw new ApplicationException()));
            builder.Services.AddSingleton<MailHelper>();
            builder.Services.AddScoped<AuthService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MaxWorldDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}