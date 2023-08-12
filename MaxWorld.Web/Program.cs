using MaxWorld.Data;
using MaxWorld.Web.Controllers;
using MaxWorld.Web.Repositories;
using MaxWorld.Web.Services;
using MaxWorld.Web.Utilities.MailSenders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MaxWorld.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ServicesAddDb(builder);

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = nameof(MaxWorld);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region === Singleton ===
            builder.Services.AddSingleton<IMailSender, GmailSender>();
            builder.Services.AddSingleton<MailHelper>();
            #endregion

            #region === Scoped ===
            builder.Services.AddScoped<Repository>();
            builder.Services.AddScoped<BaseControllerArgument>();
            builder.Services.AddScoped<BaseServiceArgument>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<ExerciseService>();
            #endregion

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

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void ServicesAddDb(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Main");
            builder.Services.AddDbContext<MaxWorldDbContext>(
                options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IDbConnection>(x =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        }
    }
}