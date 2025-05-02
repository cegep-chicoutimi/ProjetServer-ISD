namespace ProjetPartie1.Models
{
    public class Livre
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int NbPages { get; set; }
        public int AuteurId { get; set; }
        public int CategorieId { get; set; }
        public Auteur? Auteur { get; set; }
        public Catégorie? Catégorie { get; set; }

        public Livre() { }

        public Livre(int id, string isbn, string titre, int nbPages, int auteurId, int categorieId)
        {
            Id = id;
            Titre = titre;
            ISBN = isbn;
            NbPages = nbPages;
            AuteurId = auteurId;
            CategorieId = categorieId;
        }
    }
}
