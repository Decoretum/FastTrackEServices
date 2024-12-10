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
// builder.Services.AddSingleton<IGet, ShoeGet>();
// builder.Services.AddSingleton<IPost, ShoePost>();
// builder.Services.AddSingleton<IPut, ShoePut>();
// builder.Services.AddSingleton<IDelete, ShoeDelete>();

// builder.Services.AddSingleton<IGet, ClientGet>();
// builder.Services.AddSingleton<IPost, ClientPost>();
// builder.Services.AddSingleton<IPut, ClientPut>();
// builder.Services.AddSingleton<IDelete, ClientDelete>();
builder.Services.AddSingleton<ITransform, CollectionToStringArray>();

// builder.Services.AddTransient<IGet, ShoeRepairGet>();
// builder.Services.AddTransient<IPost, ShoeRepairPost>();
// builder.Services.AddTransient<IPut, ShoeRepairPut>();
// builder.Services.AddTransient<IDelete, ShoeRepairDelete>();

//Routing
builder.Services.AddControllers();


var app = builder.Build();

// Adds enpoints to controller functions to the endpoint builder interface
app.MapControllers();
app.UseStaticFiles();
app.Run();
