using System.Drawing;
using System.Windows.Forms;
using Salle_Sport.Data.Repositories;

namespace Salle_Sport.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPwd;
        private Button btnLogin;
        private Label lblStatus;

        private AuthRepository _authRepository;

        public LoginForm()
        {
            _authRepository = new AuthRepository();
            InitializeComponents();
        }

        private void InitializeComponent()
        {
            this.Text = "Connexion à la Salle de Sport";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            txtEmail = new TextBox()
            {
                Location = new Point(50, 50), Width = 300, PlaceholderText = "Email"
            };

            txtPwd = new TextBox()
            {
                Location = new Point(50, 100), Width = 300, PasswordChar = '*', PlaceholderText = "Mot de passe"
            };
            
            btnLogin = new Button()
            {
                Text = "Se connecter",
                Location = new Point(50, 150),
                Width = 100
            };
            btnLogin.Click += BtnLogin_Click;

            lblStatus = new Label()
            {
                Location = new Point(50, 200),
                Width = 300,
                ForeColor = Color.Red
            };

            this.Controls.Add(txtEmail);
            this.Controls.Add(txtPwd);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblStatus);
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string pwd = txtPwd.Text;

            Console.WriteLine($"Tentative de connexion avec Email: {email}");

            var user = _authRepository.Login(email, pwd);

            if(user != null)
            {
                lblStatus.Text = "Succès !";
                lblStatus.ForeColor = Color.Green;
                
                // OUVRIR DASHBORAD ICI
            }
            else
            {
                lblStatus.Text = "Échec de la connexion.";
            }
        }
    }
}