using EmprenderTucumanWebApi.Custom;
using EmprenderTucumanWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmprenderTucumanWebApi.Infrastructure.Repositories;
using EmprenderTucumanWebApi.Interfaces.Repositories;
using EmprenderTucumanWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DBemprendedoresContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configurar JwtSettings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<PublicacionRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<EmprendimientoRepository>();
builder.Services.AddScoped<IEmprendimientoRepository,EmprendimientoRepository>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<CalificacionRepository>();
builder.Services.AddScoped<RolRepository>();
builder.Services.AddSingleton<S3Service>();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administradores", policy =>
        policy.RequireAssertion(context =>
        {
            var nivelClaim = context.User.FindFirst("Nivel");
            if (nivelClaim == null) return false;

            var nivel = int.Parse(nivelClaim.Value);
            return nivel >= 3; 
        }));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Emprendedores", policy =>
        policy.RequireAssertion(context =>
        {
            var nivelClaim = context.User.FindFirst("Nivel");
            if (nivelClaim == null) return false;

            var nivel = int.Parse(nivelClaim.Value);
            return nivel >= 2;
        }));
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseCors("PermitirFrontend"); // Aplica la política de CORS
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
