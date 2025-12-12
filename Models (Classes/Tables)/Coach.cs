namespace Salle_Sport.Models
{
    public class Coach
    {
        public int Id { get; set;}
        public string Nom {get; set;} = string.Empty;
        public string Prenom {get; set;} = string.Empty;
        public string Specialite {get; set;} = string.Empty;
    }
}