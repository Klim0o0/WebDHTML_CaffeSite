using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Caffe.Controllers
{
    public class BookingController : Controller
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoCollection<Table> _mongoCollection;

        public BookingController(MongoClient mongoClient)
        {
            _mongoClient = mongoClient;
            _mongoCollection = _mongoClient.GetDatabase("booking").GetCollection<Table>("tables");
        }

        [HttpGet]
        [Route("api/booking")]
        public async Task<IActionResult> GetBookings([FromQuery] DateTime from, [FromQuery] int count)
        {
            if (from < DateTime.Now)
            {
                return BadRequest(Array.Empty<TableDto>());
            }

            var to = from.Add(TimeSpan.FromHours(count));
            var tables = _mongoCollection.FindSync(x => true).ToList()
                .Where(x => x.Bookings.Length == 0 ||
                            x.Bookings.All(t =>
                                (from <= t.From &&
                                 to <= t.From) ||
                                (from >= t.To && to >= t.To))).ToList();
            return Ok(tables.Select(x => new TableDto()
            {
                SeatsCount = x.SeatsCount,
                TableNumber = x.TableNumber
            }).ToList().ToJson());
        }

        [HttpGet]
        [Authorize]
        [Route("api/user/bookings")]
        public async Task<IActionResult> GetUserBookings([FromQuery] DateTime now)
        {
            var tables = _mongoCollection.FindSync(x => true).ToList()
                .Where(x =>
                    x.Bookings.Any(b => b.User == User.Identity.Name)).Select(t =>
                    (new TableDto()
                    {
                        TableNumber = t.TableNumber,
                        SeatsCount = t.SeatsCount
                    }, t.Bookings.Where(x => x.User == User.Identity.Name && x.From > now).Select(x => new BookingDto()
                    {
                        User = x.User,
                        DateFrom = x.From.ToString(CultureInfo.InvariantCulture),
                        DateTo = x.To.ToString(CultureInfo.InvariantCulture)
                    }).ToArray())).ToArray();

            return Ok(tables.ToJson());
        }

        [HttpPost]
        [Authorize]
        [Route("api/booking")]
        public async Task<IActionResult> SetBooking([FromQuery] int seatsCount, [FromQuery] DateTime from,
            [FromQuery] int count)
        {
            if (from < DateTime.Now)
            {
                return BadRequest();
            }

            var to = from.Add(TimeSpan.FromHours(count));
            if (to.Subtract(from) < TimeSpan.FromHours(2))
            {
                return BadRequest("Минимальное время брони 2 часа");
            }

            var tables = _mongoCollection.FindSync(x => true).ToList().FirstOrDefault(x => x.Bookings.Length == 0 ||
                x.Bookings.All(t =>
                    from <= t.From &&
                    to <= t.From ||
                    from >= t.To && to >= t.To));
            if (tables == null)
            {
                return BadRequest("Нет свободных столов");
            }

            var b = new Booking()
            {
                User = User.Identity.Name,
                From = from,
                To = to
            };


            await _mongoCollection.UpdateOneAsync(x => x.Id == tables.Id,
                Builders<Table>.Update.Set(u => u.Bookings, tables.Bookings.Append(b).ToArray()));

            return Ok("Заброниррованно");
        }
    }
}