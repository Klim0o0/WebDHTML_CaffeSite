using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_server.Controllers
{
    public class CommentsController : Controller
    {
        [HttpGet]
        [Route("main/comments")]
        public async Task<IActionResult> GetComments(string id)
        {
            return Ok("comments");
        }
        
        
    }
}