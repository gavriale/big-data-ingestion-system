using BigDataApiAndIngestion.Services.IngestionService.Consumers;
using BigDataApiAndIngestion.Services.IngestionService.Models;
using BigDataApiAndIngestion.Services.IngestionService.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register IMessageProcessor for StockTransaction
builder.Services.AddSingleton<IMessageProcessor<StockTransaction>, StockTransactionMessageProcessor>();

// Register KafkaConsumerService as a hosted service
builder.Services.AddHostedService<KafkaConsumerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
