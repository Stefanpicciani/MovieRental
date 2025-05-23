using MovieRental.Data.Context;
using MovieRental.Extensions;
using MovieRental.Features.Customers;
using MovieRental.Features.Movies;
using MovieRental.Features.Rentals;
using MovieRental.Interfaces.Customers;
using MovieRental.Interfaces.Movies;
using MovieRental.Interfaces.Payment;
using MovieRental.Interfaces.Rentals;
using MovieRental.PaymentProviders;
using MovieRental.Services;



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
builder.Services.AddScoped<ICustomerFeatures, CustomerFeatures>();


builder.Services.AddScoped<IPaymentProvider, PayPalProvider>();
builder.Services.AddScoped<IPaymentProvider, MbWayProvider>();
builder.Services.AddScoped<IPaymentProvider, CreditCardProvider>();

builder.Services.AddScoped<IPaymentService, PaymentService>();


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
