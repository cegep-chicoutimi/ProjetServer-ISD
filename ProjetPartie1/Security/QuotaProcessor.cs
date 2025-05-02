using Projet.DataAccessLayer;
using ProjetPartie1.Models;
using Projet.Models;

namespace Projet.Security
{
    public class QuotaProcessor
    {

        private DAL dal;
        private QuotaApi? quotaApi;

        public QuotaProcessor(Membre membre)
        {
            dal = new DAL();
            quotaApi = dal.QuotaApiFactory.GetByMemBreId(membre.Id);
        }

        private void VerificationJour()
        {
            if (quotaApi != null)
            {
                if (quotaApi.DateMAJ.AddHours(24) <= DateTime.Now)
                {
                    quotaApi.DateMAJ = DateTime.Now;
                    quotaApi.RequeteUtilisees = 0;
                }
            }
        }

        public bool PeutFaireAppel()
        {
            VerificationJour();

            if (quotaApi != null)
            {
                if (quotaApi.Maxrequetes > quotaApi.RequeteUtilisees)
                {
                    quotaApi.RequeteUtilisees++;

                    dal.QuotaApiFactory.Save(quotaApi);
                    return true;
                }
            }

            return false;
        }
    }
}
