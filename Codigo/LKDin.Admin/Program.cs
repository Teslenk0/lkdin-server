using AutoMapper;
using LKDin.Admin.Internal.Filters;
using LKDin.Admin.Internal.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ErrorHandlerFilter());
});

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AdminMappingProfile());
});

var mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
