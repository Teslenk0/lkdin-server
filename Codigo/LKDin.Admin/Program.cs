using AutoMapper;
using LKDin.Admin.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
