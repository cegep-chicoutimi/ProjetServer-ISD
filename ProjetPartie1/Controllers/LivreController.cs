/*
 Jordy Oswald Ngankou Ngassam 
    2331055
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projet.DataAccessLayer;
using ProjetPartie1.Models;
using Projet.Security.Authentication;
using Projet.Security.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjetPartie1.Controllers
{
    [Route("api/[controller]")]
    [ApiKeyAuthHandler]
    [ApiController]
    public class LivreController : ControllerBase
    {
        private DAL dal;

        public LivreController()
        {
            dal = new DAL();
        }

        [HttpGet]
        public ActionResult<Livre[]> GetAllLivre()
        {
            Livre[] livres = dal.LivreFactory.GetAll();
            return livres;
        }


        // GET: api/<LivreController>
        [HttpGet]
        [AllowAnonymous]
        [Route("LivresAuteur/{auteurId}")]
        public ActionResult<Livre[]> GetLivresAuteur(int auteurId)
        {
            Livre[] livres = dal.LivreFactory.GetAllByAutorId(auteurId);

            if (livres == null)
                return NotFound("Auteur inexistant");

            if (livres.Length == 0)
                return StatusCode(204, "Cette auteur n'a aucun livre");

            return livres;
        }

        // GET: api/<LivreController>
        [HttpGet]
        [AllowAnonymous]
        [Route("LivresCategorie/{categorieId}")]
        public ActionResult<Livre[]> GetLivresCategory(int categorieId)
        {
            Livre[] livres = dal.LivreFactory.GetAllByCategoryId(categorieId);

            if (livres == null)
                return NotFound("Catégorie inexistante");

            if (livres.Length == 0)
                return StatusCode(204, "Cette categorie n'a aucun livre");

            return livres;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LivresEmprunté")]
        public ActionResult<Livre[]> GetLivresEmprunts()
        {
            Livre[] livres = dal.LivreFactory.GetAllByEmrprunts(1);

            if (livres == null)
                return NotFound("Liste inexistante");

            if (livres.Length == 0)
                return StatusCode(204, "Tous les livres sont empruntés");

            return livres;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LivresNonEmprunté")]
        public ActionResult<Livre[]> GetLivresNonEmprunts()
        {
            Livre[] livres = dal.LivreFactory.GetAllByEmrprunts(0);

            if (livres == null)
                return NotFound("Liste inexistante");

            if (livres.Length == 0)
                return StatusCode(204, "Aucun livres emprunté");

            return livres;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LivresMembres/{membreId}")]
        public ActionResult<Livre[]> GetLivresMembre(int membreId)
        {
            Livre[] livres = dal.LivreFactory.GetAllByMembre(1, membreId);

            if (livres == null)
                return NotFound("Membre inexistante");

            if (livres.Length == 0)
                return StatusCode(204, "Ce membre n'a aucun livre");

            return livres;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("AllLivresMembres/{membreId}")]
        public ActionResult<Livre[]> GetAllLivresMembre(int membreId)
        {
            Livre[] livres = dal.LivreFactory.GetAllByMembre(0, membreId);

            if (livres == null)
                return NotFound("Membre inexistante");

            if (livres.Length == 0)
                return StatusCode(204, "Ce membre n'a jamais emprunté le livre");

            return livres;
        }

        // GET api/<LivreController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Livre> Get(int id)
        {
            Livre? livre = dal.LivreFactory.Get(id);

            if(livre == null)
                return NotFound("Aucun livre trouver");

            return livre;
        }

        // POST api/<LivreController>
        [HttpPost]
        public ActionResult<Livre> Post([FromBody] Livre livre)
        {
            livre.Id = 0;
            Auteur? auteur = dal.AuteurFactory.Get(livre.AuteurId);
            Catégorie? catégorie = dal.CatégorieFactory.Get(livre.CategorieId);

            if (auteur == null)
                return NotFound("Aucun auteur trouver pour cette id");

            if (catégorie == null)
                return NotFound("Aucune catégorie correspondante trouvée pour cette id");

            livre.Auteur = auteur;
            livre.Catégorie = catégorie;

            dal.LivreFactory.Save(livre);

            return livre;
        }

        // PUT api/<LivreController>/5
        [HttpPut("{id}")]
        [CustomAuthorize(Roles.Admin, Roles.Editor)]
        public ActionResult<Livre> Put(int id, [FromBody] Livre livre)
        {
            Auteur? auteur = dal.AuteurFactory.Get(livre.AuteurId);
            Catégorie? catégorie = dal.CatégorieFactory.Get(livre.CategorieId);

            if (dal.LivreFactory.Get(id) == null)
                return NotFound("Aucun livre trouver");

            if (auteur == null)
                return NotFound("Aucun auteur trouver pour cette id");

            if (catégorie == null)
                return NotFound("Aucune catégorie correspondante trouvée pour cette id");

            livre.Id = id;
            livre.Auteur = auteur;
            livre.Catégorie = catégorie;

            dal.LivreFactory.Save(livre);

            return livre;
        }

        // DELETE api/<LivreController>/5
        [HttpDelete("{id}")]
        public ActionResult<Livre> Delete(int id)
        {
            Livre? livre = dal.LivreFactory.Get(id);

            if (livre == null)
                return NotFound("Aucun livre trouver");

            if (dal.CatégorieFactory.GetByLivreId(id) != null)
                return StatusCode(409, "Ce livre est actuellement emprunter");

            dal.LivreFactory.Delete(id);

            return livre;
        }
    }
}
