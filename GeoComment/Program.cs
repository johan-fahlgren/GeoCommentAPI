using GeoComment.Data;
using GeoComment.Models;
using GeoComment.Services;
using GeoComment.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<GeoCommentService>();
builder.Services.AddScoped<GeoUserService>();
builder.Services.AddScoped<JwtManager>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader =
        new QueryStringApiVersionReader("api-version");
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(0, 1);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v0.1", new OpenApiInfo
        {
            Title = "GeoComments API",
            Version = "0.1",
            Description = "GeoCommenter allows users to add and find comments based on geographic location using latitude and longitude"
        });
        options.SwaggerDoc("v0.2", new OpenApiInfo());
        options.OperationFilter<AddApiVersionExampleValueOperationFilter>();
        //JWT
        options.AddSecurityDefinition("JwtAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
        });
        options.OperationFilter<SecurityRequirementsOperationFilter>();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);

    });


builder.Services.AddDbContext<GeoCommentsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<GeoUser>()
    .AddEntityFrameworkStores<GeoCommentsDBContext>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key =
            Encoding.ASCII.GetBytes(
                builder.Configuration["JwtConfig:Secret"]);

        options.SaveToken = true;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
            };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v0.1/swagger.json", "v0.1");
        options.SwaggerEndpoint($"/swagger/v0.2/swagger.json", "v0.2");

    });

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider
        .GetRequiredService<GeoCommentsDBContext>();

    ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();
}

app.Run();
