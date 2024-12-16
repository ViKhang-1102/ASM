using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM
{
    public partial class frmShoes : Form
    {
        public frmShoes()
        {
            InitializeComponent();
        }

        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryShoes = "SELECT IDShoes, NameShoes, IDProvider, IDCategory, Prices FROM SHOES";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryShoes, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["NameShoes"].ToString());
                        item.SubItems.Add(reader["IDProvider"].ToString());
                        item.SubItems.Add(reader["IDCategory"].ToString());
                        item.SubItems.Add(reader["Prices"].ToString());


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
        public frmShoes(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmShoes_Load(object sender, EventArgs e)
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


        // Export to View
        private void lvView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvView.SelectedItems.Count > 0)
            {
                var SelectedItem = lvView.SelectedItems[0];
                txtIDShoes.Text = SelectedItem.SubItems[0].Text;
                txtNameShoes.Text = SelectedItem.SubItems[1].Text;
                txtIDProvider.Text = SelectedItem.SubItems[2].Text;
                txtIDCategory.Text = SelectedItem.SubItems[3].Text;
                txtPrices.Text = SelectedItem.SubItems[4].Text;
              
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
            btnBack.Enabled = true;
            btnClose.Enabled = true;
            btnReset.Enabled = true;
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }


        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain(userRole);
            frmMain.Show();
            this.Hide();
        }

       
        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDShoes.Clear();
            txtNameShoes.Clear();
            txtIDProvider.Clear();
            txtIDCategory.Clear();
            txtPrices.Clear();
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
            string query = "INSERT INTO SHOES (IDShoes, NameShoes, IDProvider, IDCategory, Prices) VALUES (@IDShoes, @NameShoes, @IDProvider, @IDCategory, @Prices)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@NameShoes", txtNameShoes.Text);
                command.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
                command.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);
                command.Parameters.AddWithValue("@Prices", txtPrices.Text);
               
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDShoes.Text);
                    newItem.SubItems.Add(txtNameShoes.Text);
                    newItem.SubItems.Add(txtIDProvider.Text);
                    newItem.SubItems.Add(txtIDCategory.Text);
                    newItem.SubItems.Add(txtPrices.Text);                 

                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes
                txtIDShoes.Clear();
                txtNameShoes.Clear();
                txtIDProvider.Clear();
                txtIDCategory.Clear();
                txtPrices.Clear();              
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE SHOES SET NameShoes = @NameShoes, IDProvider = @IDProvider, IDCategory = @IDCategory, Prices = @Prices WHERE IDShoes = @IDShoes";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@NameShoes", txtNameShoes.Text);
                command.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
                command.Parameters.AddWithValue("@IDCategory", txtIDCategory.Text);
                command.Parameters.AddWithValue("@Prices", txtPrices.Text);
                
                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Update successful
                        MessageBox.Show("Update successful!", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Update information in ListView
                        foreach (ListViewItem item in lvView.Items)
                        {
                            if (item.Text == txtIDShoes.Text) // Find the row with matching IDShoes
                            {
                                item.SubItems[1].Text = txtNameShoes.Text;     
                                item.SubItems[2].Text = txtIDProvider.Text;        
                                item.SubItems[3].Text = txtIDCategory.Text;       
                                item.SubItems[4].Text = txtPrices.Text;   
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Customer ID not found. Please check again.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear the contents of the TextBoxes after completion
                txtIDShoes.Clear();
                txtNameShoes.Clear();
                txtIDProvider.Clear();
                txtIDCategory.Clear();
                txtPrices.Clear();
            }
        }


        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {               
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // Check if Shoes ID references SHOE DETAILS table and BILLDETAILS table
            string checkQuery1 = "SELECT COUNT(*) FROM SHOEDETAILS WHERE IDShoes = @IDShoes";
            string checkQuery2 = "SELECT COUNT(*) FROM BILLDETAILS WHERE IDShoes = @IDShoes";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd1 = new SqlCommand(checkQuery1, conn);
                checkCmd1.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);

                SqlCommand checkCmd2 = new SqlCommand(checkQuery2, conn);
                checkCmd2.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);

                try
                {
                    conn.Open();

                    int countSHOEDETAILS = (int)checkCmd1.ExecuteScalar();
                    int countBILLDETAILS = (int)checkCmd2.ExecuteScalar();

                    // If there are records in the BILLDETAILS table, delete first
                    if (countBILLDETAILS > 0)
                    {
                        string deleteBillDetailsQuery = "DELETE FROM BILLDETAILS WHERE IDShoes = @IDShoes";
                        SqlCommand deleteBillDetailsCmd = new SqlCommand(deleteBillDetailsQuery, conn);
                        deleteBillDetailsCmd.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                        deleteBillDetailsCmd.ExecuteNonQuery();
                    }

                    // If there are records in the SHOEDETAILS table, delete after
                    if (countSHOEDETAILS > 0)
                    {
                        string deleteShoeDetailsQuery = "DELETE FROM SHOEDETAILS WHERE IDShoes = @IDShoes";
                        SqlCommand deleteShoeDetailsCmd = new SqlCommand(deleteShoeDetailsQuery, conn);
                        deleteShoeDetailsCmd.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                        deleteShoeDetailsCmd.ExecuteNonQuery();
                    }

                    // Delete records in the SHOES table
                    string deleteShoesQuery = "DELETE FROM SHOES WHERE IDShoes = @IDShoes";
                    SqlCommand deleteShoesCmd = new SqlCommand(deleteShoesQuery, conn);
                    deleteShoesCmd.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                    deleteShoesCmd.ExecuteNonQuery();

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
            string query = "SELECT IDShoes, NameShoes, IDProvider, IDCategory, Prices FROM SHOES WHERE NameShoes LIKE @Search OR IDShoes LIKE @Search";

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

                    // Clear the old list in ListView before displaying new results
                    lvView.Items.Clear();

                    // Iterate through the returned results
                    while (reader.Read())
                    {
                        // Create new row in ListView
                        ListViewItem item = new ListViewItem(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["NameShoes"].ToString());
                        item.SubItems.Add(reader["IDProvider"].ToString());
                        item.SubItems.Add(reader["IDCategory"].ToString());
                        item.SubItems.Add(reader["Prices"].ToString());

                        // Add row to ListView
                        lvView.Items.Add(item);
                    }

                    reader.Close();

                    // TNotify if no results
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
            // Check IDShoes
            if (string.IsNullOrWhiteSpace(txtIDShoes.Text) || !int.TryParse(txtIDShoes.Text, out _))
            {
                MessageBox.Show("IDShoes must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDShoes.Focus();
                return false;
            }


            // Check uniqueness of IDShoes
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM SHOES WHERE IDShoes = @IDShoes";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDShoes already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDShoes.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDShoes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check NameShoes
            if (string.IsNullOrWhiteSpace(txtNameShoes.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txtNameShoes.Text, "^[a-zA-Z0-9\\s]+$"))
            {
                MessageBox.Show("NameShoes cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameShoes.Focus();
                return false;
            }


            // Check IDProvider
            if (string.IsNullOrWhiteSpace(txtIDProvider.Text) || !int.TryParse(txtIDProvider.Text, out _))
            {
                MessageBox.Show("IDProvider must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDProvider.Focus();
                return false;
            }


            // Check IDCategory
            if (string.IsNullOrWhiteSpace(txtIDCategory.Text) || !int.TryParse(txtIDCategory.Text, out _))
            {
                MessageBox.Show("IDCategory must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDCategory.Focus();
                return false;
            }


            // Check prices 1
            if (string.IsNullOrWhiteSpace(txtPrices.Text))
            {
                MessageBox.Show("Prices cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrices.Focus();
                return false;
            }


            // Check prices 2
            if (!int.TryParse(txtPrices.Text, out int prices) || prices < 0)
            {
                MessageBox.Show("Prices must be a non-negative integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrices.Focus();
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
                ROW_NUMBER() OVER (ORDER BY IDShoes) AS NewID,
                IDShoes
            FROM SHOES
        )
        UPDATE B
        SET B.IDShoes = N.NewID
        FROM SHOES B
        JOIN NumberedRows N
        ON B.IDShoes = N.IDShoes";

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
