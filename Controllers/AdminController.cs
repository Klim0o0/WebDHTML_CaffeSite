using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Caffe.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<MongoRole> _roleManager;
        private readonly IMongoCollection<MongoTable> _mongoTableCollection;
        private readonly IMongoCollection<MenuItemMongoDto> _mongoMenuCollection;

        public AdminController(RoleManager<MongoRole> roleManager, MongoClient mongoClient)
        {
            _roleManager = roleManager;
            _mongoTableCollection = mongoClient.GetDatabase("booking").GetCollection<MongoTable>("tables");
            _mongoMenuCollection = mongoClient.GetDatabase("menu").GetCollection<MenuItemMongoDto>("menu");
        }

        public async Task<IActionResult> CreateRole([Required] string roleName)
        {
            var result = await _roleManager.CreateAsync(new MongoRole { Name = roleName });
            if (result.Succeeded)
                return Ok("Role Created Successfully");
            return BadRequest(result.Errors.Select(x => x.Description).ToList());
        }

        [HttpPost]
        [Route("/api/tables")]
        public async Task<IActionResult> AddTable([FromBody] TableDto table)
        {
            await _mongoTableCollection.InsertOneAsync(new MongoTable()
            {
                SeatsCount = table.SeatsCount,
                TableNumber = table.TableNumber,
            });
            return Ok();
        }

        [HttpPost]
        [Route("/api/menu")]
        public async Task<IActionResult> AddFood([FromBody] MenuItemDto menuItemDto)
        {
            await _mongoMenuCollection.InsertOneAsync(new MenuItemMongoDto()
            {
                Name = menuItemDto.Name,
                Description = menuItemDto.Description,
                ImgUrl = menuItemDto.ImgUrl,
                Price = menuItemDto.Price
            });
            return Ok();
        }
    }
}