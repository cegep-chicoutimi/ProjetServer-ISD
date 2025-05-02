using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Projet.DataAccessLayer;
using ProjetPartie1.Models;

namespace Projet.Security.Authentication
{
    public class ApiKeyAuthHandler : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "x-api-key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            //Verification de l'existance de la clé
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "Il faut fournir la clé d'API."
                };
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            //Recherche du memebre qui possed la clé
            Membre? membre = new DAL().MembreFactory.GetMembreByApiKey(potentialApiKey);

            if (membre == null)
            {
                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Votre clé d'API n'existe pas dans le système"
                };
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

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

            await next();
        }
    }
}
