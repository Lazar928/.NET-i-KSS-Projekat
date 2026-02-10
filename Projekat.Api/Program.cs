using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Projekat.Api.Data;

var builder = WebApplication.CreateBuilder(args); //Inicijalizacija i ucitavanje servisa

// Add services
builder.Services.AddControllers(); // Dozvoljava rad svih API kontrolera

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Unesi: Bearer {tvoj JWT token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});  //Swagger je konfigurisan da podrzava JWT autentikacuju

// DbContext - MySQL (veza sa bazom)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //ko je izdao token
        ValidateAudience = true, //kome je namenjen
        ValidateLifetime = true, //da li je istekao
        ValidateIssuerSigningKey = true, //da li je validan

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ) // JWT se validira pomocu definisanog kljuca u konfiguraciji
    };
});

builder.Services.AddAuthorization(); // Omogucava autorizaciju, tj. bez ovoga role ne bi radile

// CORS (Vue), veza sa VUE frontendom, ovo dozvoljava da frontend poziva backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

//ovde se gradi aplikacija sa svim servisima 
var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVue");

app.UseAuthentication(); //PRolazi prvo autentikaciju pa tek onda autorizciju
app.UseAuthorization(); //Autentikacija proverava ko je u pitanu a autozacija da li ima pravo pristupa

app.MapControllers(); //Sluzi za povezivanje ruta
app.Run(); //pokrece server
