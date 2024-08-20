using finalproj.BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace finalproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<Article>> Get()
        {
            Article article = new Article();
            return article.Read();
        }
    }
}
