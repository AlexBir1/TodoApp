using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using TodoAPI.DAL.DBContext;
using TodoAPI.DAL.Entities;
using TodoAPI.DAL.Repositories.Implementations;
using TodoAPI.DAL.Repositories.Interfaces;
using TodoAPI.Hubs;
using TodoAPI.Middlewares.AppBuilderExtensions;
using TodoAPI.Services.Implementations;
using TodoAPI.Services.Interfaces;

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

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
            builder.Services.AddScoped<IGoalRepository, GoalRepository>();

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICollectionService, CollectionService>();
            builder.Services.AddScoped<IGoalService, GoalService>();

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

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(l =>
            {
                l.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                l.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/Notification")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddSignalR();

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/Notification");
            });


            app.Run();
        }
    }
}