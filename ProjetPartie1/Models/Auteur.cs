namespace ProjetPartie1.Models
{
    public class Auteur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        public Auteur() { }

        public Auteur(int id, string nom)
        {
            Id = id;
            Nom = nom;
        }
    }
}
