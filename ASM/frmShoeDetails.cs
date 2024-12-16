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
    public partial class frmShoeDetails : Form
    {
        public frmShoeDetails()
        {
            InitializeComponent();
        }

        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryShoeDetails = "SELECT IDShoeDetails, IDShoes, Size, Color, Material FROM SHOEDETAILS";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryShoeDetails, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDShoeDetails"].ToString());
                        item.SubItems.Add(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["Size"].ToString());
                        item.SubItems.Add(reader["Color"].ToString());
                        item.SubItems.Add(reader["Material"].ToString());

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
        public frmShoeDetails(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmShoeDetails_Load(object sender, EventArgs e)
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
                txtIDShoeDetails.Text = SelectedItem.SubItems[0].Text;
                txtIDShoes.Text = SelectedItem.SubItems[1].Text;
                txtSize.Text = SelectedItem.SubItems[2].Text;
                txtColor.Text = SelectedItem.SubItems[3].Text;
                txtMaterial.Text = SelectedItem.SubItems[4].Text;
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


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain(userRole);
            frmMain.Show();
            this.Hide();
        }


        // Colse
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        
        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDShoeDetails.Clear();
            txtIDShoes.Clear();
            txtSize.Clear();
            txtColor.Clear();
            txtMaterial.Clear();
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
            string query = "INSERT INTO SHOEDETAILS (IDShoeDetails, IDShoes, Size, Color, Material) VALUES (@IDShoeDetails, @IDShoes, @Size, @Color, @Material)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDShoeDetails", txtIDShoeDetails.Text);
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@Size", txtSize.Text);
                command.Parameters.AddWithValue("@Color", txtColor.Text);
                command.Parameters.AddWithValue("@Material", txtMaterial.Text);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDShoeDetails.Text);
                    newItem.SubItems.Add(txtIDShoes.Text);
                    newItem.SubItems.Add(txtSize.Text);
                    newItem.SubItems.Add(txtColor.Text);
                    newItem.SubItems.Add(txtMaterial.Text);

                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes 
                txtIDShoeDetails.Clear();
                txtIDShoes.Clear();
                txtSize.Clear();
                txtColor.Clear();
                txtMaterial.Clear();
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE SHOEDETAILS SET IDShoes = @IDShoes, Size = @Size, Color = @Color, Material = @Material WHERE IDShoeDetails = @IDShoeDetails";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDShoeDetails", txtIDShoeDetails.Text);
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@Size", txtSize.Text);
                command.Parameters.AddWithValue("@Color", txtColor.Text);
                command.Parameters.AddWithValue("@Material", txtMaterial.Text);

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
                            if (item.Text == txtIDShoeDetails.Text) // Tìm dòng có IDCustomer trùng khớp
                            {
                                item.SubItems[1].Text = txtIDShoes.Text;                                                                                   // 
                                item.SubItems[2].Text = txtSize.Text;      
                                item.SubItems[3].Text = txtColor.Text;   
                                item.SubItems[4].Text = txtMaterial.Text;      
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
                txtIDShoeDetails.Clear();
                txtIDShoes.Clear();
                txtSize.Clear();
                txtColor.Clear();
                txtMaterial.Clear();
            }
        }


        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvView.SelectedItems.Count > 0)
            {
                // Get selected item
                var selectedItem = lvView.SelectedItems[0];
                string selectedDeleteID = selectedItem.SubItems[0].Text;

                // Connection string
                string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                    "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

                // SQL statement to delete
                string query = "DELETE FROM SHOEDETAILS WHERE IDShoeDetails = @IDShoeDetails";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);

                    // Add parameter
                    command.Parameters.Add("@IDShoeDetails", SqlDbType.Int).Value = int.Parse(selectedDeleteID);

                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();

                        // Remove from ListView
                        lvView.Items.Remove(selectedItem);

                        MessageBox.Show("Delete successful!", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a provider to delete!", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Clear the contents of the TextBox after deletion
            txtIDShoeDetails.Clear();
            txtIDShoes.Clear();
            txtSize.Clear();
            txtColor.Clear();
            txtMaterial.Clear();

            ResetIDsAfterDelete();
        }


        // Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to search
            string query = "SELECT IDShoeDetails, IDShoes, Size, Color, Material FROM SHOEDETAILS WHERE IDShoes = @SearchKeyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign search value from TextBox
                string searchValue = txtSearch.Text.Trim(); // Use the '%' character to search for similar
                command.Parameters.AddWithValue("@SearchKeyword", searchValue);

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
                        ListViewItem item = new ListViewItem(reader["IDShoeDetails"].ToString());
                        item.SubItems.Add(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["Size"].ToString());
                        item.SubItems.Add(reader["Color"].ToString());
                        item.SubItems.Add(reader["Material"].ToString());

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
            // Check IDShoeDetails
            if (string.IsNullOrWhiteSpace(txtIDShoeDetails.Text) || !int.TryParse(txtIDShoeDetails.Text, out _))
            {
                MessageBox.Show("IDShoeDetails must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDShoeDetails.Focus();
                return false;
            }


            // Check uniqueness of IDShoeDetails
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM SHOEDETAILS WHERE IDShoeDetails = @IDShoeDetails";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDShoeDetails", txtIDShoeDetails.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDShoeDetails already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDShoeDetails.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDShoeDetails: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check IDShoes
            if (string.IsNullOrWhiteSpace(txtIDShoes.Text) || !int.TryParse(txtIDShoes.Text, out _))
            {
                MessageBox.Show("IDShoes must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDShoes.Focus();
                return false;
            }


            // Check Size
            if (string.IsNullOrWhiteSpace(txtSize.Text) ||
                !(txtSize.Text.Equals("37", StringComparison.OrdinalIgnoreCase) ||
                  txtSize.Text.Equals("38", StringComparison.OrdinalIgnoreCase) ||
                  txtSize.Text.Equals("39", StringComparison.OrdinalIgnoreCase) ||
                  txtSize.Text.Equals("40", StringComparison.OrdinalIgnoreCase) ||
                  txtSize.Text.Equals("41", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Please enter valid size for shoes (37-41).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSize.Focus();
                return false;
            }


            // Check Color
            if (string.IsNullOrWhiteSpace(txtColor.Text) ||
                !(txtColor.Text.Equals("White", StringComparison.OrdinalIgnoreCase) ||
                  txtColor.Text.Equals("Milky white", StringComparison.OrdinalIgnoreCase) ||
                  txtColor.Text.Equals("White with blue border", StringComparison.OrdinalIgnoreCase) ||
                  txtColor.Text.Equals("Black", StringComparison.OrdinalIgnoreCase) ||
                  txtColor.Text.Equals("Black and coffee color", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Please enter a valid color for shoes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtColor.Focus();
                return false;
            }


            // Check Material
            if (string.IsNullOrWhiteSpace(txtMaterial.Text) ||
                !(txtMaterial.Text.Equals("Cotton", StringComparison.OrdinalIgnoreCase) ||
                  txtMaterial.Text.Equals("Primeknit", StringComparison.OrdinalIgnoreCase) ||
                  txtMaterial.Text.Equals("Suede and leather", StringComparison.OrdinalIgnoreCase) ||
                  txtMaterial.Text.Equals("Suede and mesh", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Please enter a valid material for shoes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaterial.Focus();
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
                ROW_NUMBER() OVER (ORDER BY IDShoeDetails) AS NewID,
                IDShoeDetails
            FROM SHOEDETAILS
        )
        UPDATE B
        SET B.IDShoeDetails = N.NewID
        FROM SHOEDETAILS B
        JOIN NumberedRows N
        ON B.IDShoeDetails = N.IDShoeDetails";

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
