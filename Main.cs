using System.Text;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Data;
using FastTrackEServices.Implementation;
using Implementation.Concrete;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

//Dependency Injections
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddTransient<IGet, ShoeGet>();
builder.Services.AddTransient<IPost, ShoePost>();

//Routing
builder.Services.AddControllers();


var app = builder.Build();

// Adds enpoints to controller functions to the endpoint builder interface
app.MapControllers();
app.UseStaticFiles();
app.Run();
