namespace Salle_Sport.Models
{
    public class Session
    {
        public int Id { get; set;}
        public DateTime DateHDébut { get; set;}
        public int DuréeMinutes { get; set;}
        public int Cap_Max { get; set;}
        public int IdCoach { get; set;}
        public string NomCoach { get; set; } = string.Empty;

        public int IdAdé {get; set;}
        public string NomAdé {get; set;} = string.Empty;
    }
}