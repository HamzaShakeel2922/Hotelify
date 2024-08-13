using ReactApp1.Server.Services;
using ReactApp1.Server.Database;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<HotelDbContext>();
builder.Services.AddTransient<UserServices>();
builder.Services.AddTransient<RoomTypeServices>();
builder.Services.AddTransient<RoomService>();
builder.Services.AddTransient<AmenitiesService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:5173"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
