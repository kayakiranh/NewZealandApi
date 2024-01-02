using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Mappings;
using NewZealandWalk.API.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<NzwDbContext>(dbOptions =>
{
    dbOptions.UseSqlServer(builder.Configuration.GetSection("ConnectionString").Value);
    dbOptions.EnableSensitiveDataLogging();
    dbOptions.UseLazyLoadingProxies(false);
    dbOptions.EnableDetailedErrors(true);
});
builder.Services.AddScoped<IRegionRepository, EfRegionRepository>();
builder.Services.AddScoped<IDifficultyRepository, EfDifficultyRepository>();
builder.Services.AddScoped<IWalkRouteRepository, EfWalkRouteRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

WebApplication app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();