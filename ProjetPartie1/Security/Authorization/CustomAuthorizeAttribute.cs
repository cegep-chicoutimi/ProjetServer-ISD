using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Projet.DataAccessLayer;
using ProjetPartie1.Models;

namespace Projet.Security.Authorization
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private List<string> _roles;

        public CustomAuthorizeAttribute(params string[] Roles)
        {
            _roles = new List<string>(Roles);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (hasAllowAnonymous) return;

            string apikey = context.HttpContext.Request.Headers["X-API-Key"].FirstOrDefault() ?? "";
            // Verification de la recuperation de la clef d'api
            if (string.IsNullOrWhiteSpace(apikey))
            {
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "Il faut fournir la cle d'API."
                };

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

                return;
            }

            //instanciation du dal pour avoir accès à notre factory
            DAL dal = new DAL();
            List<string> roles = dal.MembreRoleFactory.GetRolesFromApiKey(apikey);

            //Recuperation du membre
            Membre? membre = dal.MembreFactory.GetMembreByApiKey(apikey);

            //comparaison de la liste de roles pouvant acceder a la ressource avec les roles de l'utilisateur
            if (roles == null || !_roles.Intersect(roles).Any())
            {
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Votre clef d'api ne vous permet pas d'acceder a cette ressource"
                };

                context.Result = new ObjectResult(errorResponse)
                {

                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return;
            }

            if (membre != null)
            {
                QuotaProcessor quotaProcessor = new QuotaProcessor(membre);

                if (!quotaProcessor.PeutFaireAppel())
                {
                    var errorResponse = new
                    {
                        StatusCode = StatusCodes.Status429TooManyRequests,
                        Message = "Votre quota de 20 requete été atteinte ! Réessayez plutard"
                    };
                    context.Result = new ObjectResult(errorResponse)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    return;
                }
            }
            else
            {
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Une erreur c'est produite"
                };
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }


        }
    }
}