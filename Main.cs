using System.Text;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Data;
using FastTrackEServices.Implementation;
using Implementation.Concrete;
using FastTrackEServices.HelperAlgorithms;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

//Dependency Injections
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddTransient<IGet, ShoeGet>();
builder.Services.AddTransient<IPost, ShoePost>();
builder.Services.AddTransient<IPut, ShoePut>();
builder.Services.AddTransient<IDelete, ShoeDelete>();

builder.Services.AddTransient<IGet, ClientGet>();
builder.Services.AddTransient<IPost, ClientPost>();
builder.Services.AddTransient<IPut, ClientPut>();
builder.Services.AddTransient<IDelete, ClientDelete>();
builder.Services.AddSingleton<ITransform, CollectionToStringArray>();

builder.Services.AddTransient<IGet, ShoeRepairGet>();
builder.Services.AddTransient<IPost, ShoeRepairPost>();

//Routing
builder.Services.AddControllers();


var app = builder.Build();

// Adds enpoints to controller functions to the endpoint builder interface
app.MapControllers();
app.UseStaticFiles();
app.Run();
