using MySql.Data.MySqlClient;

namespace Salle_Sport.Data
{
    public static class Database
    {
        private static string _currentUser = "app_login";
        private static string _currentPwd = "login_pass";
        
        public static MySqlConnection GetConnection()
        {
            string connectionString = $"server=localhost;user={_currentUser};password={_currentPwd};database=Salle_Sport;";
            return new MySqlConnection(connectionString);
        }

        public static void SwitchUserRole(string role)
        {
            switch(role)
            {
                case "AP" :
                    _currentUser = "app_ap";
                    _currentPwd = "ap_pass";
                    break;
                case "AS" :
                    _currentUser = "app_as";
                    _currentPwd = "as_pass";
                    break;
                case "Mb" :
                    _currentUser = "app_mb";
                    _currentPwd = "mb_pass";
                    break;
                case "Ev" :
                    _currentUser = "app_ev";
                    _currentPwd = "ev_pass";
                    break;
                default :
                    _currentUser = "app_login";
                    _currentPwd = "login_pass";
                    break;
            }
            Console.WriteLine($"[INFO] Bascule vers : {role} avec l'utilisateur {_currentUser}");
        }
    }
}
