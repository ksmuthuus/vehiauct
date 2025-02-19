using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class ActionUpdatedConsumer : IConsumer<AuctionUpdated>
{
  private readonly IMapper _mapper;

  public ActionUpdatedConsumer(IMapper mapper)
  {
    _mapper = mapper;
  }
  public async Task Consume(ConsumeContext<AuctionUpdated> context)
  {
    Console.WriteLine($"Consuming search-auction-updated: {context.Message.Id}");
    var item = _mapper.Map<Item>(context.Message);
    Console.WriteLine("Before Update\n" + item);
    var result = await DB.Update<Item>()
      .Match(a => a.ID == context.Message.Id)
      .ModifyOnly(x => new
      {
        x.Color,
        x.Make,
        x.Model,
        x.Year,
        x.Mileage
      }, item)
      .ExecuteAsync();
    Console.WriteLine(result);
    if (!result.IsAcknowledged)
      throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
  }
}
