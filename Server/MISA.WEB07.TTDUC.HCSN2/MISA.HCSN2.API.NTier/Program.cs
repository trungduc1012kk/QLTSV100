using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MISA.HCSN2.API.NTier.Controllers;
using MISA.HCSN2.BL;
using MISA.HCSN2.BL.AuthBL;
using MISA.HCSN2.BL.BudgetBL;
using MISA.HCSN2.BL.DepartmentBL;
using MISA.HCSN2.BL.FileBL;
using MISA.HCSN2.BL.IncrementPropertyBL;
using MISA.HCSN2.BL.PropertyBL;
using MISA.HCSN2.BL.PropertyTypeBL;
using MISA.HCSN2.DL;
using MISA.HCSN2.DL.AuthBL;
using MISA.HCSN2.DL.AuthDL;
using MISA.HCSN2.DL.BudgetDL;
using MISA.HCSN2.DL.DepartmentDL;
using MISA.HCSN2.DL.IncrementPropertyDL;
using MISA.HCSN2.DL.PropertyDL;
using MISA.HCSN2.DL.PropertyTypeDL;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// dependency injection
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));

builder.Services.AddScoped<IPropertyDL, PropertyDL>();
builder.Services.AddScoped<IPropertyBL, PropertyBL>();

builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();

builder.Services.AddScoped<IPropertyTypeDL, PropertyTypeDL>();
builder.Services.AddScoped<IPropertyTypeBL, PropertyTypeBL>();

builder.Services.AddScoped<IIncrementPropertyDL, IncrementPropertyDL>();
builder.Services.AddScoped<IIncrementPropertyBL, IncrementPropertyBL>();

builder.Services.AddScoped<IBudgetDL, BudgetDL>();
builder.Services.AddScoped<IBudgetBL, BudgetBL>();

builder.Services.AddScoped<IAuthDL, AuthDL>();
builder.Services.AddScoped<IAuthBL, AuthBL>();

builder.Services.AddScoped<IFileBL, FileBL>();

// Lấy dữ liệu connection string từ file appsettings
DatabaseContext.ConnectionString = builder.Configuration.GetConnectionString("MySqlConnection");

AuthsController.configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// cấu hình swagger authorize
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

// authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app cors
app.UseCors("corsapp");

app.UseAuthentication();
app.UseAuthorization();

//app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
