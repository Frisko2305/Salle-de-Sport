namespace Salle_Sport.Models
{
    public class Inscri_Seance
    {
        public int IdUser { get; set; }     //Clé étrangère vers Utilisateur
        public int IdSeance { get; set; }   //Clé étrangère vers Séance
        public DateTime DateInsc { get; set; }
        public bool Present { get; set;}
        public User? User { get; set;}
        public Seance? Seance { get; set; }
    }
}