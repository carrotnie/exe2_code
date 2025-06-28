using System.Security.Claims;
using System.Text;
using Influencerhub.DAL.Contract;
using Influencerhub.DAL.Data;
using Influencerhub.DAL.Implementation;
using Influencerhub.DAL.Repository;
using Influencerhub.Services.Contract;
using Influencerhub.Services.HubService.Service;
using Influencerhub.Services.Implement;
using Influencerhub.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// JWT AUTH
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? "")),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("https://influencerhub.id.vn")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Influencerhub API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Just paste your token below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] { }
        }
    });
});

// DbContext
builder.Services.AddDbContext<InfluencerhubDBContext>();

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IInfluRepository, InfluRepository>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IBusinessFieldRepository, BusinessFieldRepository>();
builder.Services.AddScoped<IRepresentativeRepository, RepresentativeRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IFreelanceJobRepository, FreelanceJobRepository>();
builder.Services.AddScoped<IMembershipTypeRepository, MembershipTypeRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

// Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInfluService, InfluService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IFieldService, FieldService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddHostedService<JobStatusCheckerService>();
builder.Services.AddScoped<IFreelanceJobService, FreelanceJobService>();
builder.Services.AddScoped<IMembershipTypeService, MembershipTypeService>();
builder.Services.AddScoped<IMembershipRegistrationService, MembershipRegistrationService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();

// Chat services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IConversationPartnersService, ConversationPartnersService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IPartnerShipService, PartnerShipService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IHubService, HubService>();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// REDIRECT ROOT (và các route không phải API/Swagger/static) về FE
app.Use(async (context, next) =>
{
    // Cho phép /swagger, /api, /signalr, hoặc file tĩnh
    var path = context.Request.Path.Value?.ToLower();
    if (
        path == "/" ||
        (path != null &&
            !path.StartsWith("/api")
            && !path.StartsWith("/swagger")
            && !path.StartsWith("/signalr")
            && !System.IO.Path.HasExtension(path))
       )
    {
        // Redirect về FE
        context.Response.Redirect("https://influencerhub.id.vn");
        return;
    }
    await next();
});

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Influencerhub API v1");
});
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Global error handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            "{\"error\": \"An unexpected error occurred. Please try again later.\"}");
    });
});

app.MapControllers();

app.Run();
