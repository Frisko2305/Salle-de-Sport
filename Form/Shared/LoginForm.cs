using Salle_Sport.Data.Repositories;
using Salle_Sport.Forms.Dashboards;

namespace Salle_Sport.Forms
{
    public class LoginForm : Form
    {
        private TableLayoutPanel MainLayout;
        private TextBox txtEmail;
        private TextBox txtPwd;
        private Button btnLogin;
        private Label lblStatus;
        private LinkLabel lnkRegister;

        private AuthRepository _authRepository;

        public LoginForm()
        {
            _authRepository = new AuthRepository();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Connexion à la Salle de Sport";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            MainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(40, 30, 40, 30),
                AutoSize = true
            };

            MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            for(int i = 0 ; i < MainLayout.RowCount ; i++)
            {
                MainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            txtEmail = new TextBox()
            {
                Anchor = AnchorStyles.None,
                Dock = DockStyle.Fill,
                PlaceholderText = "Email",
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 10, 0, 10)
            };

            txtPwd = new TextBox()
            {
                Anchor = AnchorStyles.None,
                Dock = DockStyle.Fill,
                PasswordChar = '*',
                PlaceholderText = "Mot de passe",
                Font = new Font("Segoe UI", 11),
                Margin = new Padding(0, 10, 0, 10)
            };
            
            btnLogin = new Button()
            {
                Text = "Se connecter",
                Anchor = AnchorStyles.None,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Margin = new Padding(0, 20, 0, 10)
            };
            btnLogin.Click += BtnLogin_Click;

            lblStatus = new Label()
            {
                Anchor = AnchorStyles.None,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Red,
                Margin = new Padding(0, 5, 0, 10)
            };

            lnkRegister = new LinkLabel()
            {
                Text = "Pas de compte ? Inscrivez-vous",
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Font = new Font("Segoe UI", 9),
                Margin = new Padding(0, 10, 0, 0)
            };
            lnkRegister.Click += LnkRegister_Click;

            MainLayout.Controls.Add(txtEmail, 0, 0);
            MainLayout.Controls.Add(txtPwd, 0, 1);
            MainLayout.Controls.Add(btnLogin, 0, 2);
            MainLayout.Controls.Add(lblStatus, 0, 3);
            MainLayout.Controls.Add(lnkRegister, 0, 4);

            this.Controls.Add(MainLayout);
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
                
                this.Hide();

                Form Dashboard = user.Role switch
                {
                    "Mb" => new Mb_Dashb(user),
                    "AS" => new AS_Dashb(user),
                    "AP" => new AP_Dashb(user),
                    "Ev" => new Ev_Dashb(user),
                    _ => throw new Exception($"Rôle inconnue : {user.Role} ?")
                };
                Dashboard.FormClosed += (s, args) => this.Close();
                Dashboard.Show();
            }
            else
            {
                lblStatus.Text = "Échec de la connexion.";
            }
        }

        private void LnkRegister_Click(object? sender, EventArgs e)
        {
            RegisterForm Form = new RegisterForm();
            Form.Show();

            this.Hide();
            Form.FormClosed += (s,args) => this.Close();
        }
    }
}