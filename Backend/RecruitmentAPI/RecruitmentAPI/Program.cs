using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi;

using RecruitmentAPI.Data;
using RecruitmentAPI.Repositories;
using RecruitmentAPI.Repositories.Interfaces;
using RecruitmentAPI.Services;
using RecruitmentAPI.Services.Interfaces;

using System.Text;


var builder = WebApplication.CreateBuilder(args);


// =====================================
// Database
// =====================================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});




// =====================================
// Dependency Injection Services
// =====================================

// Common Services

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<SkillExtractionService>();

builder.Services.AddScoped<ResumeAnalysisService>();

builder.Services.AddScoped<CalendarService>();

builder.Services.AddScoped<IEmailService, EmailService>();




// Job Module

builder.Services.AddScoped<IJobRepository, JobRepository>();

builder.Services.AddScoped<IJobService, JobService>();




// Application Module

builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();

builder.Services.AddScoped<IApplicationService, ApplicationService>();




// Interview Module

builder.Services.AddScoped<IInterviewRepository, InterviewRepository>();

builder.Services.AddScoped<IInterviewService, InterviewService>();




// Evaluation Module

builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();

builder.Services.AddScoped<IEvaluationService, EvaluationService>();




// Admin Module

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IAdminService, AdminService>();




// AI Matching

builder.Services.AddScoped<IMatchScoreService, MatchScoreService>();
builder.Services.AddScoped<IAIService, AIService>();







// =====================================
// JWT Authentication
// =====================================


var jwtSettings = builder.Configuration.GetSection("Jwt");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;


    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;

})


.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
    new TokenValidationParameters
    {

        ValidateIssuer = true,

        ValidateAudience = true,

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,


        ValidIssuer =
            jwtSettings["Issuer"],


        ValidAudience =
            jwtSettings["Audience"],


        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    jwtSettings["Key"]!
                )
            ),


        RoleClaimType =
            System.Security.Claims.ClaimTypes.Role

    };

});







// =====================================
// Controllers
// =====================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();







// =====================================
// Swagger + JWT
// =====================================

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "Bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
            "Enter JWT Token like: Bearer {token}"
        });


    options.AddSecurityRequirement(
        document =>
        new OpenApiSecurityRequirement
        {
            [
                new OpenApiSecuritySchemeReference(
                    "Bearer",
                    document)
            ] = new List<string>()
        });

});





// =====================================
// CORS
// =====================================

builder.Services.AddCors(options =>
{

    options.AddPolicy(
        "AllowAll",

        policy =>
        {

            policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();

        });

});







var app = builder.Build();







// =====================================
// Middleware
// =====================================


app.UseSwagger();

app.UseSwaggerUI();



app.UseStaticFiles();



app.UseCors("AllowAll");



// app.UseHttpsRedirection();
    


app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();



app.Run();