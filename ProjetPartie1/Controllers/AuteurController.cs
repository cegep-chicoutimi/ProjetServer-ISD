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
    public class AuteurController : ControllerBase
    {

        private DAL dal;

        public AuteurController()
        {
            dal = new DAL();
        }


        // POST: api/<AuteurController>
        [HttpPost]
        [CustomAuthorize(Roles.Admin)]
        public ActionResult<Auteur> Post([FromBody] Auteur auteur)
        {
            auteur.Id = 0;

            if (auteur.Nom == "" || auteur.Nom == null)
                return BadRequest("Il faut un nom a l'auteur");

            dal.AuteurFactory.Save(auteur);

            return auteur;
        }

        // PUT: api/<AuteurController>/
        [HttpPut("{id}")]
        [CustomAuthorize(Roles.Admin, Roles.Editor)]
        public ActionResult<Auteur> Put(int id, [FromBody] Auteur auteur)
        {

            if (dal.AuteurFactory.Get(id) == null)
                return StatusCode(400, "id inexistant");

            auteur.Id = id;

            dal.AuteurFactory.Save(auteur);
            return auteur;
        }

        // DELETE api/<AuteurController>
        [HttpDelete("{id}")]
        [CustomAuthorize(Roles.Admin)]
        public ActionResult<Auteur> Delete(int id)
        {
            Auteur? auteur = dal.AuteurFactory.Get(id);

            if (auteur == null)
                return NotFound("id inexistant");

            if (dal.LivreFactory.GetAllByAutorId(id).Any())
                return StatusCode(409, "Cette auteur possede un livre");

            dal.AuteurFactory.Delete(id);
            return auteur;
        }

        [HttpGet("{id}")]
        public ActionResult<Auteur> Get(int id)
        {
            Auteur? auteur = dal.AuteurFactory.Get(id);

            if (auteur == null)
                return NotFound("id inexistant");

            return auteur;
        }

        [HttpGet]
        [CustomAuthorize(Roles.Admin)]
        public ActionResult<Auteur[]> GetAll()
        {
            Auteur[] auteurs = dal.AuteurFactory.GetAll();

            if (auteurs == null)
                return NotFound("Liste d'auteur inexistante");

            if (!auteurs.Any())
                return StatusCode(204, "Aucun Contenue Trouver");

            return auteurs;
        }
    }
}
