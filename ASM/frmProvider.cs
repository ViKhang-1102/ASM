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
    public partial class frmProvider : Form
    {
        public frmProvider()
        {
            InitializeComponent();
        }

        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryProvider = "SELECT IDProvider, NameProvider, Phone, Address, Email FROM PROVIDER";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryProvider, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDProvider"].ToString());
                        item.SubItems.Add(reader["NameProvider"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        item.SubItems.Add(reader["Address"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());

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
        public frmProvider(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmProvider_Load(object sender, EventArgs e)
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
                txtIDProvider.Text = SelectedItem.SubItems[0].Text;
                txtNameProvider.Text = SelectedItem.SubItems[1].Text;               
                txtPhone.Text = SelectedItem.SubItems[2].Text;
                txtAddress.Text = SelectedItem.SubItems[3].Text;
                txtEmail.Text = SelectedItem.SubItems[4].Text;
            }
        }


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain("admin");
            frmMain.Show();
            this.Hide();
        }


        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDProvider.Clear();
            txtNameProvider.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
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
            string query = "INSERT INTO PROVIDER (IDProvider, NameProvider, Phone, Address, Email) VALUES (@IDProvider, @NameProvider, @Phone, @Address, @Email)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
                command.Parameters.AddWithValue("@NameProvider", txtNameProvider.Text);                
                command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@Email", txtEmail.Text);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDProvider.Text);
                    newItem.SubItems.Add(txtNameProvider.Text);                   
                    newItem.SubItems.Add(txtPhone.Text);
                    newItem.SubItems.Add(txtAddress.Text);
                    newItem.SubItems.Add(txtEmail.Text);
                          
                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes
                txtIDProvider.Clear();
                txtNameProvider.Clear();              
                txtPhone.Clear();
                txtAddress.Clear();
                txtEmail.Clear();
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {         
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE PROVIDER SET NameProvider = @NameProvider, Phone = @Phone, Address = @Address, Email = @Email WHERE IDProvider = @IDProvider";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
                command.Parameters.AddWithValue("@NameProvider", txtNameProvider.Text);
                command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@Email", txtEmail.Text);

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
                            if (item.Text == txtIDProvider.Text) // Find the row with matching IDProvider
                            {
                                item.SubItems[1].Text = txtNameProvider.Text;                                                                                   // 
                                item.SubItems[2].Text = txtPhone.Text;        
                                item.SubItems[3].Text = txtAddress.Text;      
                                item.SubItems[4].Text = txtEmail.Text;      
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
                txtIDProvider.Clear();
                txtNameProvider.Clear();
                txtAddress.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
            }
        }


        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {         
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // Check if IDProvider has any reference records in the SHOES table
            string checkQuery = "SELECT COUNT(*) FROM SHOES WHERE IDProvider = @IDProvider";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // If there is reference data, delete the records in the BILLDETAILS table
                        string deleteShoesQuery = "DELETE FROM SHOES WHERE IDProvider = @IDProvider";
                        SqlCommand deleteShoesCmd = new SqlCommand(deleteShoesQuery, conn);
                        deleteShoesCmd.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
                        deleteShoesCmd.ExecuteNonQuery();
                    }

                    // After deleting the reference records in other tables, delete the record in the BILL table.
                    string deleteQuery = "DELETE FROM PROVIDER WHERE IDProvider = @IDProvider";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);
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
            string query = "SELECT IDProvider, NameProvider, Phone, Address, Email FROM PROVIDER WHERE NameProvider LIKE @Search OR IDProvider LIKE @Search";

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
                        ListViewItem item = new ListViewItem(reader["IDProvider"].ToString());
                        item.SubItems.Add(reader["NameProvider"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        item.SubItems.Add(reader["Address"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());

                        //Add row to ListView 
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
            // Check IDProvider
            if (string.IsNullOrWhiteSpace(txtIDProvider.Text) || !int.TryParse(txtIDProvider.Text, out _))
            {
                MessageBox.Show("IDProvider must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDProvider.Focus();
                return false;
            }

            // Check uniqueness of IDCategory
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";
            string checkIDQuery = "SELECT COUNT(*) FROM PROVIDER WHERE IDProvider = @IDProvider";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDProvider", txtIDProvider.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDCategory already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDProvider.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDCategory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            // Check NameProvider
            if (string.IsNullOrWhiteSpace(txtNameProvider.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txtNameProvider.Text, "^[a-zA-Z0-9\\s]+$"))
            {
                MessageBox.Show("NameProvider cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameProvider.Focus();
                return false;
            }

            // Check Phone
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, "^\\d+$"))
            {
                MessageBox.Show("Phone number must be entered as a number only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Check Address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            // Check Email
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                !System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"))
            {
                MessageBox.Show("Email is invalid (ex:name@example.com) or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
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
                ROW_NUMBER() OVER (ORDER BY IDProvider) AS NewID,
                IDProvider
            FROM PROVIDER
        )
        UPDATE B
        SET B.IDProvider = N.NewID
        FROM PROVIDER B
        JOIN NumberedRows N
        ON B.IDProvider = N.IDProvider";

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
