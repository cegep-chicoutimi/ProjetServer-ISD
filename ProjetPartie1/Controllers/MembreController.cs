/*
 Jordy Oswald Ngankou Ngassam 
    2331055
 */

using Microsoft.AspNetCore.Mvc;
using Projet.DataAccessLayer;
using ProjetPartie1.Models;
using Projet.Security.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetPartie1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembreController : ControllerBase
    {
        private DAL dal;

        public MembreController()
        {
            dal = new DAL();
            dal.MembreRoleFactory.GetRolesFromApiKey("admin_apikey");
        }


        // POST api/<MembreController>
        [HttpPost]
        [CustomAuthorize(Roles.Admin)]
        public ActionResult<Membre> Post([FromBody] Membre membre)
        {
            membre.Id = 0;

            if (membre.Nom == "" || membre.Nom is null)
                return BadRequest("Aucun nom entrer");

            if (membre.Courriel == "" || membre.Courriel is null)
                return BadRequest("Aucune adresse entrer");

            if (!Membre.EstEmailValide(membre.Courriel))
                return BadRequest("Adresse mail invalide");

            membre.DateCreation = DateTime.Now;
            membre.ApiKey = Membre.GenerateApiKey(membre.Nom);

            dal.MembreFactory.Save(membre);

            return membre;
        }

    }
}
