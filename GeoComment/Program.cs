using GeoComment.Data;
using GeoComment.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddResponseCaching();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader =
        new QueryStringApiVersionReader("api-version");
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 1);
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
});

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

    });


builder.Services.AddDbContext<GeoCommentsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider
        .GetRequiredService<GeoCommentsDBContext>();

    ctx.Database.EnsureCreated();
}

app.Run();
