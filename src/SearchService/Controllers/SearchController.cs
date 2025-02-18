using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> SearchItems([FromQuery] SearchParams searchParams)
        {
            var query = DB.PagedSearch<Item, Item>();

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }
            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(y => y.Make)),
                "new" => query.Sort(x => x.Descending(y => y.CreatedAt)),
                _ => query.Sort(x => x.Ascending(y => y.AuctionEnd))
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x => x.AuctionEnd > DateTime.UtcNow
                && x.AuctionEnd < DateTime.UtcNow.AddHours(6)),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
            };

            query = !string.IsNullOrEmpty(searchParams.Seller) ? query.Match(x => x.Seller == searchParams.Seller) : query;
            query = !string.IsNullOrEmpty(searchParams.Winner) ? query.Match(x => x.Winner == searchParams.Winner) : query;
            query.PageNumber(searchParams.PageNumber).PageSize(searchParams.PageSize);
            var result = await query.ExecuteAsync();
            return Ok(new
            {
                items = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount

            });
        }
    }
}
