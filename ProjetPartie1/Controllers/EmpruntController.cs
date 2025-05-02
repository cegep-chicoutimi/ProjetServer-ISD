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
    [CustomAuthorize(Roles.Admin)]
    [ApiController]
    public class EmpruntController : ControllerBase
    {

        private DAL dal;

        public EmpruntController()
        {
            dal = new DAL();
        }

        // POST api/<EmpruntController>
        [HttpPost]
        public ActionResult<Emprunt> Post([FromBody] Emprunt emprunt)
        {
            emprunt.Id = 0;
            emprunt.DateEmprunt = DateTime.Now;

            if (emprunt.LivreId > dal.LivreFactory.GetAll().Length || emprunt.LivreId <= 0)
                return NotFound("L'id du livre n'existe pas");

            if (emprunt.MembreId > dal.MembreFactory.GetAll().Length || emprunt.MembreId <= 0)
                return NotFound("L'id du membre n'existe pas");

            if(emprunt.DateRetour <= emprunt.DateEmprunt)
                return BadRequest("La date de retour ne peut pas etre avant la date d'emprunt");

            return emprunt;


        }

        // DELETE api/<EmpruntController>/5
        [HttpDelete("{id}")]
        public ActionResult<Emprunt> Delete(int id)
        {
            Emprunt? emprunt = dal.EmpruntFactory.Get(id);

            if (emprunt == null)
                return NotFound("id inexistant");

            dal.EmpruntFactory.Delete(id);
            return emprunt;
        }
    }
}
