namespace Salle_Sport.Models
{
    public class MbProfile
    {
        public int IdUser { get; set; }
        public string Statut { get; set; } = "EN_ATTENTE";
        public string? MotifBan { get; set; }
        public DateTime DateCreatDossier { get; set; }
        public DateTime? DateValidAdmin { get; set;}
        public int? ValidePar { get; set; }
        
        public User? User { get; set;}
    }
}