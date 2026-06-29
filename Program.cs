
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using WatchmanDevotional.Data;
//using WatchmanDevotional.Interfaces;
//using WatchmanDevotional.Repositories;
//using WatchmanDevotional.Services;

//namespace WatchmanDevotional
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//            builder.Services.AddDbContext<WatchmanDevotionDbContext>(options =>
//                options.UseNpgsql(connectionString));
//            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
//            builder.Services.AddScoped<IDevotionalRepository, DevotionalRepository>();
//            builder.Services.AddScoped<DevotionalService>();
//            builder.Services.AddScoped<WatchmanDevotional.Services.AuthService>();
//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowReactApp",
//                    policy => policy.WithOrigins("http://localhost:5173")
//                                    .AllowAnyMethod()
//                                    .AllowAnyHeader());
//            });



//            var jwtKey = builder.Configuration["Jwt:Key"]
//                ?? throw new InvalidOperationException("JWT:Key is missing in appsettings.json");

//            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(options =>
//                {
//                    options.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuerSigningKey = true,

//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

//                        ValidateIssuer = true,
//                        ValidateAudience = true,

//                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                        ValidAudience = builder.Configuration["Jwt:Audience"],

//                        ValidateLifetime = true,
//                        ClockSkew = TimeSpan.Zero
//                    };
//                });



//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.UseCors("AllowReactApp");

//            app.MapControllers();



//            app.Run();
//        }
//    }
//}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WatchmanDevotional.Data;
using WatchmanDevotional.Interfaces;
using WatchmanDevotional.Models; // Added to access the User model
using WatchmanDevotional.Repositories;
using WatchmanDevotional.Services;


namespace WatchmanDevotional
{
    public class Program
    {
        public static async Task Main(string[] args) // Changed void to async Task to allow clean async seeding
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<WatchmanDevotionDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<IDevotionalRepository, DevotionalRepository>();
            builder.Services.AddScoped<DevotionalService>();
            builder.Services.AddScoped<WatchmanDevotional.Services.AuthService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy.WithOrigins("http://localhost:5173")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var jwtKey = builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT:Key is missing in appsettings.json");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Watchman Devotional API", Version = "v1" });

                // Define the Bearer Auth scheme
                var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token directly. Example: `eyJhbGciOi...`",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Id = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ?? CRITICAL FIX: CORS must execute BEFORE Authentication/Authorization pipelines
            app.UseCors("AllowReactApp");

            app.UseAuthentication(); // Ensure authentication middleware is explicit
            app.UseAuthorization();

            app.MapControllers();

            // ==========================================
            // ?? INDUSTRY-STANDARD AUTOMATED DATA SEEDER
            // ==========================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<WatchmanDevotionDbContext>();
                    var config = services.GetRequiredService<IConfiguration>();

                    // 1. Automatically run migrations against PostgreSQL on startup
                    if ((await context.Database.GetPendingMigrationsAsync()).Any())
                    {
                        await context.Database.MigrateAsync();
                    }

                    // 2. Check if a SuperAdmin account already exists in the database
                    var superAdminExists = await context.Users
                        .AnyAsync(u => u.Role.ToLower() == "superadmin");

                    if (!superAdminExists)
                    {
                        // 3. Extract credentials out of your safe configuration providers
                        var email = config["SuperAdminSettings:Email"];
                        var rawPassword = config["SuperAdminSettings:Password"];

                        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(rawPassword))
                        {
                            throw new InvalidOperationException("SuperAdminSettings credentials are missing from configuration variables.");
                        }

                        // 4. Instantiate the database record
                        var superAdmin = new User
                        {
                            Id = Guid.NewGuid(),
                            FullName = "Master Super Admin",
                            Email = email.Trim().ToLower(),
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword),
                            Role = "SuperAdmin"
                        };

                        // 5. Commit explicitly to PostgreSQL
                        context.Users.Add(superAdmin);
                        await context.SaveChangesAsync();

                        Console.WriteLine("--> PostgreSQL Bootstrapping: Master SuperAdmin account seeded successfully.");
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database migration or system bootstrapping onboarding sequences.");
                }
            }

            await app.RunAsync(); // Changed to modern async execution matching our Task signature
        }
    }
}