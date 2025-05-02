using System.Text.RegularExpressions;

namespace ProjetPartie1.Models
{
    public class Membre
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Courriel { get; set; } = string.Empty ;
        public string Telephone { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; } = DateTime.MinValue;
        public string ApiKey { get; set; } = string.Empty;

        public Membre() { }

        public Membre(int id, string nom, string courriel, string telephone, DateTime dateCreation, string apiKey)
        {
            Id = id;
            Nom = nom;
            Courriel = courriel;
            Telephone = telephone;
            DateCreation = dateCreation;
            ApiKey = apiKey;
        }

        public static string GenerateApiKey(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom ne peut pas être vide.");

            // Prendre la première et la dernière lettre
            char premiereLettre = char.ToUpper(nom[0]);
            char derniereLettre = char.ToUpper(nom[nom.Length - 1]);

            // Générer 4 chiffres aléatoires
            Random rand = new Random();
            int chiffres = rand.Next(1000, 10000); // de 1000 à 9999 inclus

            // Assembler la clé
            string apiKey = $"{premiereLettre}{chiffres}{derniereLettre}";

            return apiKey;
        }

        public static bool EstEmailValide(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expression régulière de base pour vérifier le format d’un courriel
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, pattern);
        }
    }
}
