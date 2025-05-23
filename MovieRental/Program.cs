using MovieRental.Data;
using MovieRental.Extensions;
using MovieRental.Features.Movies;
using MovieRental.Features.Rentals;
using MovieRental.Interfaces.Movies;
using MovieRental.Interfaces.Rentals;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Movie Rental API",
        Version = "v1",
        Description = "API para sistema de aluguel de filmes com processamento de pagamento"
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<MovieRentalDbContext>();



builder.Services.AddScoped<IRentalFeatures, RentalFeatures>();
builder.Services.AddScoped<IMovieFeatures, MovieFeatures>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var client = new MovieRentalDbContext())
{
	client.Database.EnsureCreated();
}

app.Run();
