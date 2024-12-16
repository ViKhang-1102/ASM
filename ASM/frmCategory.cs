using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ASM
{
    public partial class frmCategory : Form
    {
        public frmCategory()
        {
            InitializeComponent();
        }

        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryCategory = "SELECT IDCategory, NameCategory FROM CATEGORY";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryCategory, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDCategory"].ToString());
                        item.SubItems.Add(reader["NameCategory"].ToString());


                        // Add item to ListView
                        lvView.Items.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }


        private string userRole;
        public frmCategory(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmCategory_Load(object sender, EventArgs e)
        {
          FormLoadData();

            // Check the user role and adjust button visibility
            if (userRole == "admin")
            {
                // Admin has full access
                EnableAdminFeatures();
            }
            else if (userRole == "staff")
            {
                // Staff has limited access
                EnableStaffFeatures();
            }
            else
            {
                MessageBox.Show("Invalid role.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        private void EnableAdminFeatures()
        {
            btnBack.Enabled = true;
            btnClose.Enabled = true;
            btnReset.Enabled = true;
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }


        private void EnableStaffFeatures()
        {
            btnBack.Enabled = false;
            btnClose.Enabled = false;
            btnReset.Enabled = false;
            btnSearch.Enabled = false;
            btnNew.Enabled = false;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
        }


        // Export to View
        private void lvView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvView.SelectedItems.Count > 0)
            {
                var SelectedItem = lvView.SelectedItems[0];
                txtIDCategory.Text = SelectedItem.SubItems[0].Text;
                txtNameCategory.Text = SelectedItem.SubItems[1].Text;
            }
        }


        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain("admin");
            frmMain.Show();
            this.Hide();
        }
 

        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDCategory.Clear();
            txtNameCategory.Clear();
            txtIDCategory.Focus();
        }

                
        // Add
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to add data
            string query = "INSERT INTO CATEGORY (IDCategory, NameCategory) VALUES (@IDCategory, @NameCategory)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);
                command.Parameters.AddWithValue("@NameCategory", txtNameCategory.Text);
                
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDCategory.Text);
                    newItem.SubItems.Add(txtNameCategory.Text);
                                                         
                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes
                txtIDCategory.Clear();
                txtNameCategory.Clear();              
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {          
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE CATEGORY SET NameCategory = @NameCategory WHERE IDCategory = @IDCategory";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text); 
                command.Parameters.AddWithValue("@NameCategory", txtNameCategory.Text); 

                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Update successful
                        MessageBox.Show("Update success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Update information in ListView
                        foreach (ListViewItem item in lvView.Items)
                        {
                            if (item.Text == txtIDCategory.Text) // Find the row with matching IDCategory
                            {
                                item.SubItems[1].Text = txtNameCategory.Text; 
                                break;
                            }
                        }
                    }
                    else
                    {
                        
                        MessageBox.Show("ID not found. Please check again.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                  
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear the contents of the TextBoxes after completion
                txtIDCategory.Clear();
                txtNameCategory.Clear();
            }
        }
               

        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                   "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // Check if IDCategory has any reference records in the SHOES table
            string checkQuery = "SELECT COUNT(*) FROM SHOES WHERE IDCategory = @IDCategory";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // If there is reference data, delete the records in the SHOES table
                        string deleteShoesQuery = "DELETE FROM SHOES WHERE IDCategory = @IDCategory";
                        SqlCommand deleteShoesCmd = new SqlCommand(deleteShoesQuery, conn);
                        deleteShoesCmd.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);
                        deleteShoesCmd.ExecuteNonQuery();
                    }

                    // After deleting the reference records in SHOES table, delete the record in the CATEGORY table.
                    string deleteQuery = "DELETE FROM CATEGORY WHERE IDCategory = @IDCategory";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("Record deleted successfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update data in ListView
                    FormLoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ResetIDsAfterDelete();
        }      


        // Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to search
            string query = "SELECT IDCategory, NameCategory FROM CATEGORY WHERE NameCategory LIKE @Search OR IDCategory LIKE @Search";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign search value from TextBox
                string searchValue = "%" + txtSearch.Text.Trim() + "%"; // Use the '%' character to search for similar
                command.Parameters.AddWithValue("@Search", searchValue);

                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Clear old list in ListView before displaying new results
                    lvView.Items.Clear();

                    // Iterate through the returned results
                    while (reader.Read())
                    {
                        // Create new row in ListView
                        ListViewItem item = new ListViewItem(reader["IDCategory"].ToString());
                        item.SubItems.Add(reader["NameCategory"].ToString());

                        // Add row to ListView
                        lvView.Items.Add(item);
                    }

                    reader.Close();

                    // Notify if no results
                    if (lvView.Items.Count == 0)
                    {
                        MessageBox.Show("No results found.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                  
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear TextBox content after searching
                txtSearch.Clear();
            }
        }

        private void ResetSearch()
        {
            txtSearch.Clear();

            FormLoadData();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetSearch();
        }


        // Check Validate Input
        private bool ValidateInput()
        {
            // Check IDCategory
            if (string.IsNullOrWhiteSpace(txtIDCategory.Text) || !int.TryParse(txtIDCategory.Text, out _))
            {
                MessageBox.Show("IDCategory must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDCategory.Focus();
                return false;
            }


            // Check uniqueness of IDCategory
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM CATEGORY WHERE IDCategory = @IDCategory";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDCategory already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDCategory.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDCategory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check NameCategory
            if (string.IsNullOrWhiteSpace(txtNameCategory.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txtNameCategory.Text, "^[a-zA-Z0-9\\s]+$"))
            {
                MessageBox.Show("NameCategory cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameCategory.Focus();
                return false;
            }

            return true;
        }


        // Reset ID
        private void ResetIDsAfterDelete()
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                                      "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string query = @"
        WITH NumberedRows AS (
            SELECT 
                ROW_NUMBER() OVER (ORDER BY IDCategory) AS NewID,
                IDCategory
            FROM CATEGORY
        )
        UPDATE B
        SET B.IDCategory = N.NewID
        FROM CATEGORY B
        JOIN NumberedRows N
        ON B.IDCategory = N.IDCategory";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    //MessageBox.Show("ID fields have been reset successfully.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the ListView
                    FormLoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting IDs: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
