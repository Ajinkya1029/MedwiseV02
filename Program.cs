using MedwiseBackend.Data;
using MedwiseBackend.Repository;
using Microsoft.EntityFrameworkCore;
using MedwiseBackend.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MedwiseBackend.JwtHelper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRespository, DoctorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters =new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    option.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = async (context) =>
        {
            Console.WriteLine("Printing in the delegate OnAuthFailed");
        },
        OnChallenge = async (context) =>
        {
            context.HandleResponse();
            if (context.AuthenticateFailure != null)
            {
                context.Response.StatusCode = 401;
                var res = new { success = false, Message = "UnAuthorized Access, Access Denied" };
                await context.HttpContext.Response.WriteAsJsonAsync(res);   
            }
        }
    };
});
  
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        { 
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy(Roles.Patient, policy => policy.RequireRole(Roles.Patient));
    option.AddPolicy(Roles.Doctor, policy => policy.RequireRole(Roles.Doctor));
    option.AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
