using Microsoft.EntityFrameworkCore;
using CourseDesk.Data;
using CourseDesk.Filters;

namespace CourseDesk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddDbContext<DbConnection>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException("Connection string 'DbConnection' not found.")));
            /*builder.Services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizationAttribute(UserType.Admin)); // Apply the filter as needed
            });*/

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

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}