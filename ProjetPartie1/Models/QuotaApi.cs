namespace Projet.Models
{
    public class QuotaApi
    {
        public int Id { get; set; }
        public int MembreId { get; set; }
        public DateTime DateMAJ { get; set; }
        public int Maxrequetes { get; set; }
        public int RequeteUtilisees { get; set; }

        public QuotaApi() { }

        public QuotaApi(int id, int membreId, DateTime dateMAJ, int maxrequetes, int requeteUtilisees)
        {
            Id = id;
            MembreId = membreId;
            DateMAJ = dateMAJ;
            Maxrequetes = maxrequetes;
            RequeteUtilisees = requeteUtilisees;
        }
    }
}
