using System.Threading.Tasks;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Caffe.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<MongoUser> _userManager;

        public UserController(UserManager<MongoUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        [Route("api/Account/IsAuth")]
        public async Task<IActionResult> R()
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/Account/info")]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(new UserInfoDto { Login = user.Email }.ToJson());
        }
    }
}