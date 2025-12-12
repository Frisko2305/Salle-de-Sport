using System.Drawing;
using System.Windows.Forms;
using Salle_Sport.Models;
namespace Salle_Sport.Forms.Dashboards
{
    public class Mb_Dashb : Form
    {
        private User _currentUser;

        public Mb_Dashb(User user)
        {
            _currentUser = user;
            this.Text = $"Espace Membre - {_currentUser.Prenom} {_currentUser.Nom}";
            this.Size = new Size(800, 600);
        }
    }
}