using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LLM_eCommerce_RESTAPI.AuthModels;
using System.Diagnostics;
using System.Text;
using log4net;
using Microsoft.OpenApi.Models;
using LLM_eCommerce_RESTAPI.Models;
using System.Text.Json.Serialization;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace LLM_eCommerce_RESTAPI;

public class Program
{

    private static readonly ILog logger = LogManager.GetLogger("Program.main method");

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        //Inject Application Settings
        builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));

        builder.Services.AddDbContext<LLM_eCommerce_EFDBContext>(
           options =>
           {
               options.UseSqlServer(builder.Configuration.GetConnectionString("CRUDConnection"));
           });

        builder.Services.AddDbContext<AuthenticationContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        builder.Services.AddDefaultIdentity<ApplicationUser>().
            AddRoles<IdentityRole>().AddEntityFrameworkStores<AuthenticationContext>();


        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 12;
        });

        string tmpKeyIssuer = builder.Configuration.GetSection("ApplicationSettings:JWT_Site_URL").Value;
        string tmpKeySign = builder.Configuration.GetSection("ApplicationSettings:SigningKey").Value;
        var key = Encoding.UTF8.GetBytes(tmpKeySign);



        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidIssuer = tmpKeyIssuer,
                            ValidAudience = tmpKeyIssuer,
                            ClockSkew = TimeSpan.Zero,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };

                    });

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { 
            {
                new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
            }
            });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin",
                builder =>
                {
					builder.WithOrigins("http://localhost:4200")
										.AllowAnyHeader()
										.AllowAnyMethod();
					builder.WithOrigins("https://localhost:4200")
										.AllowAnyHeader()
										.AllowAnyMethod();
					builder.WithOrigins("https://localhost:7121")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
        });

        builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });


        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();

        

        app.UseCors();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseStaticFiles();
        
        app.UseAuthentication();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}