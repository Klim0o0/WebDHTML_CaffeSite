using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Caffe.Controllers
{
    public class MenuController : Controller
    {
        private readonly MongoClient _mongoClient;
        private readonly IMapper _mapper;

        public MenuController(MongoClient mongoClient, IMapper mapper)
        {
            _mongoClient = mongoClient;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/menu")]
        public async Task<IActionResult> GetMenu()
        {
            var db = _mongoClient.GetDatabase("menu").GetCollection<MenuItemMongoDto>("menu");
            return Ok(db.FindSync(x => true).ToList().Select(x => _mapper.Map<MenuItemDto>(x)));
        }
    }
}