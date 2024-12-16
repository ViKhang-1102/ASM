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
    public partial class frmBillDetails : Form
    {
        public frmBillDetails()
        {
            InitializeComponent();
        }

        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryShoes = "SELECT IDBillDetails, IDBill, IDShoes, Quantity, TotalPrices FROM BILLDETAILS";

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
                        ListViewItem item = new ListViewItem(reader["IDBillDetails"].ToString());
                        item.SubItems.Add(reader["IDBill"].ToString());
                        item.SubItems.Add(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());
                        item.SubItems.Add(reader["TotalPrices"].ToString());


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
        public frmBillDetails(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmBillDetails_Load(object sender, EventArgs e)
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
            btnBack.Enabled = true;
            btnClose.Enabled = true;
            btnReset.Enabled = true;
            btnSearch.Enabled = true;
            btnNew.Enabled = true;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }


        // Export to View
        private void lvView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvView.SelectedItems.Count > 0)
            {
                var SelectedItem = lvView.SelectedItems[0];
                txtIDBillDetails.Text = SelectedItem.SubItems[0].Text;
                txtIDBill.Text = SelectedItem.SubItems[1].Text;
                txtIDShoes.Text = SelectedItem.SubItems[2].Text;
                txtQuantity.Text = SelectedItem.SubItems[3].Text;
                txtTotalPrices.Text = SelectedItem.SubItems[4].Text;
               
            }
        }


        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDBillDetails.Clear();
            txtIDBill.Clear();
            txtIDShoes.Clear();
            txtQuantity.Clear();
            txtTotalPrices.Clear();
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
            string query = "INSERT INTO BILLDETAILS (IDBillDetails, IDBill, IDShoes, Quantity, TotalPrices) VALUES (@IDBillDetails, @IDBill, @IDShoes, @Quantity, @TotalPrices)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDBillDetails", txtIDBillDetails.Text);
                command.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                command.Parameters.AddWithValue("@TotalPrices", txtTotalPrices.Text);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDBillDetails.Text);
                    newItem.SubItems.Add(txtIDBill.Text);
                    newItem.SubItems.Add(txtIDShoes.Text);
                    newItem.SubItems.Add(txtQuantity.Text);
                    newItem.SubItems.Add(txtTotalPrices.Text);

                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes
                txtIDBillDetails.Clear();
                txtIDBill.Clear();
                txtIDShoes.Clear();
                txtQuantity.Clear();
                txtTotalPrices.Clear();
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE BILLDETAILS SET IDBill = @IDBill, IDShoes = @IDShoes, Quantity = @Quantity, TotalPrices = @TotalPrices WHERE IDBillDetails = @IDBillDetails";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDBillDetails", txtIDBillDetails.Text);
                command.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);
                command.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                command.Parameters.AddWithValue("@TotalPrices", txtTotalPrices.Text);

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
                            if (item.Text == txtIDBillDetails.Text) // Find the row with matching IDBillDetails
                            {
                                item.SubItems[1].Text = txtIDBill.Text;                                                                                     // 
                                item.SubItems[2].Text = txtIDShoes.Text;        
                                item.SubItems[3].Text = txtQuantity.Text;       
                                item.SubItems[4].Text = txtTotalPrices.Text;        
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
                txtIDBillDetails.Clear();
                txtIDBill.Clear();
                txtIDShoes.Clear();
                txtQuantity.Clear();
                txtTotalPrices.Clear();
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
                string query = "DELETE FROM BILLDETAILS WHERE IDBillDetails = @IDBillDetails";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);

                    // Add parameter
                    command.Parameters.Add("@IDBillDetails", SqlDbType.Int).Value = int.Parse(selectedDeleteID);

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
            txtIDBillDetails.Clear();
            txtIDBill.Clear();
            txtIDShoes.Clear();
            txtQuantity.Clear();
            txtTotalPrices.Clear();

            ResetIDsAfterDelete();
        }
       

        // Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to search
            string query = "SELECT IDBillDetails, IDBill, IDShoes, Quantity, TotalPrices FROM BILLDETAILS WHERE IDBill = @SearchKeyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign search value from TextBox
                string searchValue = txtSearch.Text.Trim();  // Use the '%' character to search for similar
                command.Parameters.AddWithValue("@SearchKeyword", searchValue);

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
                        ListViewItem item = new ListViewItem(reader["IDBillDetails"].ToString());
                        item.SubItems.Add(reader["IDBill"].ToString());
                        item.SubItems.Add(reader["IDShoes"].ToString());
                        item.SubItems.Add(reader["Quantity"].ToString());
                        item.SubItems.Add(reader["TotalPrices"].ToString());

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
            // Check IDBillDetails
            if (string.IsNullOrWhiteSpace(txtIDBillDetails.Text) || !int.TryParse(txtIDBillDetails.Text, out _))
            {
                MessageBox.Show("IDBillDetails must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDBillDetails.Focus();
                return false;
            }


            // Check uniqueness of IDBillDetails
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM BILLDETAILS WHERE IDBillDetails = @IDBillDetails";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDBillDetails", txtIDBillDetails.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDBillDetails already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDBillDetails.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDBillDetails: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check IDBill
            if (string.IsNullOrWhiteSpace(txtIDBill.Text) || !int.TryParse(txtIDBill.Text, out _))
            {
                MessageBox.Show("IDBill must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDBill.Focus();
                return false;
            }


            // Check IDShoes
            if (string.IsNullOrWhiteSpace(txtIDShoes.Text) || !int.TryParse(txtIDShoes.Text, out _))
            {
                MessageBox.Show("IDShoes must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDShoes.Focus();
                return false;
            }


            // Check Quantity
            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a positive number and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }


        // Quantity
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            // Check if there is a valid IDShoes and Quantity
            if (!string.IsNullOrEmpty(txtIDShoes.Text) && int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0)
            {
                // Get price from SHOES table
                string pricesQuery = "SELECT Prices FROM SHOES WHERE IDShoes = @IDShoes";
                string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                    "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(pricesQuery, conn);
                    command.Parameters.AddWithValue("@IDShoes", txtIDShoes.Text);

                    try
                    {
                        conn.Open();
                        object result = command.ExecuteScalar(); // Returns the price of the product

                        if (result != null)
                        {
                            decimal prices = Convert.ToDecimal(result); // Price of product from SHOES table
                            decimal totalPrices = prices * quantity; // Total prices

                            // Display TotalPrices into TextBox
                            txtTotalPrices.Text = totalPrices.ToString("0.00");
                        }
                        else
                        {
                            MessageBox.Show("No shoes found with this ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                // If the quantity is not entered correctly or there is no IDShoes
                txtTotalPrices.Clear();
            }
        }


        // Reset ID
        private void ResetIDsAfterDelete()
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                                      "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string query = @"
        WITH NumberedRows AS (
            SELECT 
                ROW_NUMBER() OVER (ORDER BY IDBillDetails) AS NewID,
                IDBillDetails
            FROM BILLDETAILS
        )
        UPDATE B
        SET B.IDBillDetails = N.NewID
        FROM BILLDETAILS B
        JOIN NumberedRows N
        ON B.IDBillDetails = N.IDBillDetails";

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
