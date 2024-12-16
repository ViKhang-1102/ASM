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
    public partial class frmBill : Form
    {
        public frmBill()
        {
            InitializeComponent();
        }


        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryBill = "SELECT IDBill, IDCustomer, IDStaff, DateBill FROM BILL";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryBill, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDBill"].ToString());
                        item.SubItems.Add(reader["IDCustomer"].ToString());
                        item.SubItems.Add(reader["IDStaff"].ToString());

                        // Show only date
                        DateTime dateBill = Convert.ToDateTime(reader["DateBill"]);
                        item.SubItems.Add(dateBill.ToString("yyyy-MM-dd")); // Date format                

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

        public frmBill(string role)
        {
            InitializeComponent();
            userRole = role;
        }


        // Load
        private void frmBill_Load(object sender, EventArgs e)
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
                txtIDBill.Text = SelectedItem.SubItems[0].Text;
                txtIDCustomer.Text = SelectedItem.SubItems[1].Text;
                txtIDStaff.Text = SelectedItem.SubItems[2].Text;

                // Convert "DateBill" back to the appropriate format
                dtpBill.Text = SelectedItem.SubItems[3].Text;
            }
        }


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain(userRole);
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
            txtIDBill.Clear();
            txtIDCustomer.Clear();
            txtIDStaff.Clear();
            dtpBill.Format = DateTimePickerFormat.Custom;
            dtpBill.CustomFormat = " ";
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
            string query = "INSERT INTO BILL (IDBill, IDCustomer, IDStaff, DateBill) VALUES (@IDBill, @IDCustomer, @IDStaff, @DateBill)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                command.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
                command.Parameters.AddWithValue("@IDStaff", txtIDStaff.Text);
                command.Parameters.AddWithValue("@DateBill", DateTime.Parse(dtpBill.Text)); // Convert date format

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDBill.Text);
                    newItem.SubItems.Add(txtIDCustomer.Text);
                    newItem.SubItems.Add(txtIDStaff.Text);
                    newItem.SubItems.Add(DateTime.Parse(dtpBill.Text).ToString("yyyy-MM-dd")); // Display date format

                    lvView.Items.Add(newItem);

                    MessageBox.Show("More success", "Notifications", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error" + ex.Message);
                }

                // Clear all TextBoxes
                txtIDBill.Clear();
                txtIDCustomer.Clear();
                txtIDStaff.Clear();
            }
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE BILL SET IDCustomer = @IDCustomer, IDStaff = @IDStaff, DateBill = @DateBill WHERE IDBill = @IDBill";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                command.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
                command.Parameters.AddWithValue("@IDStaff", txtIDStaff.Text);

                // Get only date (yyyy-MM-dd) from DateTimePicker
                command.Parameters.AddWithValue("@DateBill", dtpBill.Value.Date);

                try
                {
                    conn.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Update successful
                        MessageBox.Show("Update successful!", "Notifications", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);

                        // Update information in ListView
                        foreach (ListViewItem item in lvView.Items)
                        {
                            if (item.Text == txtIDBill.Text) // Find the row with matching IDBill
                            {
                                item.SubItems[1].Text = txtIDCustomer.Text;
                                item.SubItems[2].Text = txtIDStaff.Text;   
                                item.SubItems[3].Text = dtpBill.Value.ToString("yyyy-MM-dd"); 
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bill ID not found. Please check again.", "Notifications", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error);
                }

                // Clear the contents of the TextBoxes after completion
                txtIDBill.Clear();
                txtIDCustomer.Clear();
                txtIDStaff.Clear();
            }
        }


        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // Check if BILLDETAILS has any reference records in other tables
            string checkQuery = "SELECT COUNT(*) FROM BILLDETAILS WHERE IDBill = @IDBill"; 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@IDBill", txtIDBill.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        //If there is reference data, delete the records in the BILLDETAILS table
                        string deleteShoesQuery = "DELETE FROM BILLDETAILS WHERE IDBill = @IDBill";
                        SqlCommand deleteShoesCmd = new SqlCommand(deleteShoesQuery, conn);
                        deleteShoesCmd.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                        deleteShoesCmd.ExecuteNonQuery();
                    }

                    // After deleting the reference records in other tables, delete the record in the BILL table.
                    string deleteQuery = "DELETE FROM BILL WHERE IDBill = @IDBill";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@IDBill", txtIDBill.Text);
                    deleteCmd.ExecuteNonQuery();

                    MessageBox.Show("Record deleted successfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update data in ListView
                    FormLoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ResetIDsAfterDelete();
            }
        }


        // Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to search
            string query = "SELECT IDBill, IDCustomer, IDStaff, DateBill FROM BILL WHERE IDCustomer = @SearchKeyword";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign search value from TextBox
                string searchValue = txtSearch.Text.Trim();   // Do not use '%' to indicate exact search
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
                        ListViewItem item = new ListViewItem(reader["IDBill"].ToString());
                        item.SubItems.Add(reader["IDCustomer"].ToString());
                        item.SubItems.Add(reader["IDStaff"].ToString());

                        // Get only date (yyyy-MM-dd) from DateBill
                        DateTime dateBill = Convert.ToDateTime(reader["DateBill"]);
                        item.SubItems.Add(dateBill.ToString("yyyy-MM-dd"));

                        // Add row to ListView
                        lvView.Items.Add(item);
                    }

                    reader.Close();

                    // Notify if no results
                    if (lvView.Items.Count == 0)
                    {
                        MessageBox.Show("No results found.", "Notifications",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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
            // Check IDBill
            if (string.IsNullOrWhiteSpace(txtIDBill.Text) || !int.TryParse(txtIDBill.Text, out _))
            {
                MessageBox.Show("IDBill must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDBill.Focus();
                return false;
            }


            // Check uniqueness of IDBill
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM BILL WHERE IDBill = @IDBill";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDBill", txtIDBill.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDBill already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDBill.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDShoes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check IDCustomer
            if (string.IsNullOrWhiteSpace(txtIDCustomer.Text) || !int.TryParse(txtIDCustomer.Text, out _))
            {
                MessageBox.Show("IDCustomer must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDCustomer.Focus();
                return false;
            }


            // Check IDStaff
            if (string.IsNullOrWhiteSpace(txtIDStaff.Text) || !int.TryParse(txtIDStaff.Text, out _))
            {
                MessageBox.Show("IDStaff must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDStaff.Focus();
                return false;
            }


            // Check date
            if (dtpBill.Value == DateTimePicker.MinimumDateTime)
            {
                MessageBox.Show("Please select a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBill.Focus();
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
                ROW_NUMBER() OVER (ORDER BY IDBill) AS NewID,
                IDBill
            FROM BILL
        )
        UPDATE B
        SET B.IDBill = N.NewID
        FROM BILL B
        JOIN NumberedRows N
        ON B.IDBill = N.IDBill";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();
                    //MessageBox.Show("ID fields have been reset successfully.", "Notification",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    // Refresh the ListView
                    FormLoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error resetting IDs: " + ex.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
    }
}
