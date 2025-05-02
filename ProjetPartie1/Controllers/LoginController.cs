using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet.DataAccessLayer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetPartie1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        [AllowAnonymous]
        [HttpGet("{username}/{password}")]
        public ActionResult<string> Get(string username, string password)
        {

            DAL dal = new DAL();
            string apikey = dal.LoginFactory.Get(username, password);
            if(apikey == null)
            {
                return NotFound();
            }
            return apikey;
        }

        
    }
}
