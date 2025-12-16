namespace Salle_Sport.Models
{
    public class Seance
    {
        public int Id { get; set;}
        public DateTime DateHDebut { get; set;}
        public int Duree { get; set;}
        public int Cap_Max { get; set;}
        public int IdCoach { get; set;}
        public int IdAde {get; set;}

        public string? NomCoach { get; set; } = string.Empty;
        public string? NomAde {get; set;} = string.Empty;
    }
}