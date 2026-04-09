
using Course_Selling_System.Models;
using Course_Selling_System.Service.Implementations;
using Course_Selling_System.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Course_Selling_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<CourseSellingDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
            
            //DI
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
