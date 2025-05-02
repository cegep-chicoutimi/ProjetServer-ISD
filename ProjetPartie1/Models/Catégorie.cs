namespace ProjetPartie1.Models
{
    public class Catégorie
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public Catégorie() { }

        public Catégorie(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }
    }
}
