using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.UOW.Implementations;
using TodoAPI.DAL.UOW.Interfaces;
using TodoAPI.Middlewares.AppBuilderExtensions;
using TodoAPI.Services.Interfaces;
using TodoAPI.Services.UOW;

namespace TodoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var currentDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(currentDirectory + "\\Attachments"))
                Directory.CreateDirectory(currentDirectory + "\\Attachments");

            builder.Host.UseSerilog((hbc, lc) => lc.WriteTo.Console());

            builder.Services.AddDbContext<AppDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("StrConnection")!);
            });

            builder.Services.AddIdentityCore<Account>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddUserManager<UserManager<Account>>()
            .AddEntityFrameworkStores<AppDBContext>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(l =>
            {
                l.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.AddScoped<IDBRepository, DBRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            app.UseExceptionMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(currentDirectory + "\\Attachments"),
                RequestPath = new PathString("/Attachments")
            });

            app.UseCors(t => t.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}