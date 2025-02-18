using System;
using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  public static async Task InitDb(WebApplication app)
  {
    await DB.InitAsync("SearchDb", MongoClientSettings
    .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

    await DB.Index<Item>()
    .Key(x => x.Make, KeyType.Text)
    .Key(x => x.Model, KeyType.Text)
    .Key(x => x.Color, KeyType.Text)
    .CreateAsync();

    var count = await DB.CountAsync<Item>();
    // if (count == 0)
    // {
    //   Console.WriteLine("Data Seeding in progress....");
    //   var itemData = await File.ReadAllTextAsync("Data/auctions.json");


    //   var items = JsonSerializer.Deserialize<List<Item>>(itemData, _serializerOptions);
    //   await DB.SaveAsync(items);
    // }
    using var scope = app.Services.CreateScope();
    var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();
    var items = await httpClient.GetItemsForSearchDb();
    Console.WriteLine(items.Count + " items fetched from AuctionService");
    if (items.Count > 0) await DB.SaveAsync(items);
  }
}
