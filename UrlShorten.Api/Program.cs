using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using UrlShorten.Data;
using UrlShorten.Repository.Repositories.UrlRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connect to database
builder.Services.AddDbContext<ApplicationDbContext>(op => 
    op.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// map repository to dependency injection
builder.Services.AddScoped<IUrl, ShortenUrlRepository>();

builder.Services.AddHttpContextAccessor();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7130")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
