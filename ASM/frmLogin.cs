using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        // Load
        string connectionString = @"Data Source=LAPTOP-1UOTIHIH\KHANG1102;
        Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";


        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtUserName.Focus();
        }


        // Show password
        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }


        // Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (ValidateLogin(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Check if the user is admin or staff and pass the role to the main form
                string role = GetUserRole(username, password);

                // Proceed to the main application window or next screen
                frmMain mainForm = new frmMain(role);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetUserRole(string nameuser, string password)
        {
            if (nameuser == "admin" && password == "123")
            {
                return "admin"; // Admin role
            }
            else if (nameuser == "staff" && password == "456")
            {
                return "staff"; // Staff role
            }
            else
            {
                return "unknown"; // Unknown role
            }
        }

        // Check login
        private bool ValidateLogin(string nameuser, string password)
        {
            // Check if fields are empty
            if (string.IsNullOrEmpty(nameuser) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Input Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }
            string hashedPassword = HashSHA256(password);

            // SQL query to check if the username and password match a record in UserAccount table
            string query = "SELECT COUNT(1) FROM [USER] WHERE NameUser = @NameUser AND Password = @Password";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@NameUser", nameuser);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 1; // Return true if one record is found, otherwise false
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }


        // HashSHA256 
        static string HashSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Use UTF-16 (Unicode) encoding, matching SQL Server's behavior with N'123'
                byte[] bytes = sha256.ComputeHash(Encoding.Unicode.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                // Convert byte array to hex string
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString().ToUpper(); // Match the uppercase result in SQL Server
            }
        }
    }
}
