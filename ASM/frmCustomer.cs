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
    public partial class frmCustomer : Form
    {
        public frmCustomer()
        {
            InitializeComponent();
        }


        // Form Load Data
        private void FormLoadData()
        {
            // SQL statement to get data
            string queryCustomer = "SELECT IDCustomer, NameCustomer, Gender, Phone, Address, Email FROM CUSTOMER";

            // Delete old items in ListView
            lvView.Items.Clear();

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connectionString);
            {
                SqlCommand command = new SqlCommand(queryCustomer, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListViewItem item = new ListViewItem(reader["IDCustomer"].ToString());
                        item.SubItems.Add(reader["NameCustomer"].ToString());
                        item.SubItems.Add(reader["Gender"].ToString());
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

        public frmCustomer(string role)
        {
            InitializeComponent();
            userRole = role;
        }




        // Load
        private void frmCustomer_Load(object sender, EventArgs e)
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
                txtIDCustomer.Text = SelectedItem.SubItems[0].Text;
                txtNameCustomer.Text = SelectedItem.SubItems[1].Text;
                cboGender.Text = SelectedItem.SubItems[2].Text;
                txtPhone.Text = SelectedItem.SubItems[3].Text;
                txtAddress.Text = SelectedItem.SubItems[4].Text;
                txtEmail.Text = SelectedItem.SubItems[5].Text;
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
            frmMain frmMain = new frmMain(userRole);
            frmMain.Show();
            this.Hide();
        }


        // New
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtIDCustomer.Clear();
            txtNameCustomer.Clear();
            txtAddress.Clear();
            cboGender.SelectedIndex = -1;
            txtPhone.Clear();
            txtEmail.Clear();
            txtIDCustomer.Focus();
        }
        

        // Add
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to add data
            string query = "INSERT INTO CUSTOMER (IDCustomer, NameCustomer, Gender, Phone, Address, Email) VALUES (@IDCustomer, @NameCustomer, @Gender, @Phone, @Address, @Email)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Add parameter
                command.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
                command.Parameters.AddWithValue("@NameCustomer", txtNameCustomer.Text);
                command.Parameters.AddWithValue("@Gender", cboGender.SelectedItem.ToString());
                command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                command.Parameters.AddWithValue("@Address", txtAddress.Text);
                command.Parameters.AddWithValue("@Email", txtEmail.Text);

                try
                {
                    conn.Open();
                    command.ExecuteNonQuery();

                    // Add to ListView
                    ListViewItem newItem = new ListViewItem(txtIDCustomer.Text);
                    newItem.SubItems.Add(txtNameCustomer.Text);
                    newItem.SubItems.Add(cboGender.SelectedItem.ToString());
                    newItem.SubItems.Add(txtPhone.Text);
                    newItem.SubItems.Add(txtAddress.Text);
                    newItem.SubItems.Add(txtEmail.Text);

                    MessageBox.Show("More success", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show("Error" + ex.Message); }

                // Clear all TextBoxes
                txtIDCustomer.Clear();
                txtNameCustomer.Clear();
                cboGender.SelectedIndex = -1;
                txtPhone.Clear();   
                txtAddress.Clear();
                txtEmail.Clear();

            }      
        }


        // Update
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to update data
            string query = "UPDATE CUSTOMER SET NameCustomer = @NameCustomer, Gender = @Gender, Phone = @Phone, Address = @Address, Email = @Email WHERE IDCustomer = @IDCustomer";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);

                // Assign values ​​to parameters
                command.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
                command.Parameters.AddWithValue("@NameCustomer", txtNameCustomer.Text);
                command.Parameters.AddWithValue("@Gender", cboGender.SelectedItem.ToString());
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
                            if (item.Text == txtIDCustomer.Text) // Find the row with matching IDCustomer
                            {
                                item.SubItems[1].Text = txtNameCustomer.Text;    
                                item.SubItems[2].Text = cboGender.SelectedItem.ToString();        
                                item.SubItems[3].Text = txtPhone.Text;       
                                item.SubItems[4].Text = txtAddress.Text;      
                                item.SubItems[5].Text = txtEmail.Text;       
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
                txtIDCustomer.Clear();
                txtNameCustomer.Clear();
                cboGender.SelectedIndex = -1;
                txtAddress.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
            }
        }


        // Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // Check if IDCustomer has any reference records in the BILL table
            string checkQuery = "SELECT COUNT(*) FROM BILL WHERE IDCustomer = @IDCustomer";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // If there is reference data, delete the records in the BILL table
                        string deleteOrdersQuery = "DELETE FROM BILL WHERE IDCustomer = @IDCustomer";
                        SqlCommand deleteOrdersCmd = new SqlCommand(deleteOrdersQuery, conn);
                        deleteOrdersCmd.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
                        deleteOrdersCmd.ExecuteNonQuery();
                    }

                    // After deleting the reference records in BILL table, delete the record in the CUSTOMER table.
                    string deleteQuery = "DELETE FROM CUSTOMER WHERE IDCustomer = @IDCustomer";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                    deleteCmd.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);
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
            //// Connection string to the database
            //string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            //// SQL statement to search
            //string query = "SELECT IDCustomer, NameCustomer, Gender, Phone, Address, Email FROM CUSTOMER WHERE NameCustomer LIKE @Search OR IDCustomer LIKE @Search";

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand(query, conn);

            //    // Assign search value from TextBox
            //    string searchValue = "%" + txtSearch.Text.Trim() + "%"; // Use the '%' character to search for similar
            //    command.Parameters.AddWithValue("@Search", searchValue);

            //    try
            //    {
            //        conn.Open();
            //        SqlDataReader reader = command.ExecuteReader();

            //        // Clear the old list in ListView before displaying new results
            //        lvView.Items.Clear();

            //        // Iterate through the returned results
            //        while (reader.Read())
            //        {
            //            // Create new row in ListView
            //            ListViewItem item = new ListViewItem(reader["IDCustomer"].ToString());
            //            item.SubItems.Add(reader["NameCustomer"].ToString());
            //            item.SubItems.Add(reader["Gender"].ToString());
            //            item.SubItems.Add(reader["Phone"].ToString());
            //            item.SubItems.Add(reader["Address"].ToString());
            //            item.SubItems.Add(reader["Email"].ToString());


            //            // Add row to ListView
            //            lvView.Items.Add(item);
            //        }

            //        reader.Close();

            //        // Notify if no results
            //        if (lvView.Items.Count == 0)
            //        {
            //            MessageBox.Show("No results found.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //    // Clear TextBox content after searching
            //    txtSearch.Clear();
            //}
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Nếu không có ký tự nào trong ô tìm kiếm, xóa dữ liệu trên ListView và kết thúc hàm
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                lvView.Items.Clear();
                return;
            }

            // Connection string to the database
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            // SQL statement to search
            string query = "SELECT IDCustomer, NameCustomer, Gender, Phone, Address, Email FROM CUSTOMER WHERE NameCustomer LIKE @Search OR IDCustomer LIKE @Search";

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
                        ListViewItem item = new ListViewItem(reader["IDCustomer"].ToString());
                        item.SubItems.Add(reader["NameCustomer"].ToString());
                        item.SubItems.Add(reader["Gender"].ToString());
                        item.SubItems.Add(reader["Phone"].ToString());
                        item.SubItems.Add(reader["Address"].ToString());
                        item.SubItems.Add(reader["Email"].ToString());

                        // Add row to ListView
                        lvView.Items.Add(item);
                    }

                    reader.Close();

                    // Notify if no results
                    if (lvView.Items.Count == 0)
                    {
                        // Hiển thị thông báo "Không có kết quả nào" nếu cần thiết, nhưng không khuyến khích vì gây khó chịu khi gõ liên tục.
                        // MessageBox.Show("No results found.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            // Check IDSCustomer
            if (string.IsNullOrWhiteSpace(txtIDCustomer.Text) || !int.TryParse(txtIDCustomer.Text, out _))
            {
                MessageBox.Show("IDCustomer must be numeric only and cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIDCustomer.Focus();
                return false;
            }


            // Check uniqueness of IDCustomer
            string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
                "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

            string checkIDQuery = "SELECT COUNT(*) FROM BILL WHERE IDCustomer = @IDCustomer";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand checkCommand = new SqlCommand(checkIDQuery, conn);
                checkCommand.Parameters.AddWithValue("@IDCustomer", txtIDCustomer.Text);

                try
                {
                    conn.Open();
                    int count = (int)checkCommand.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("IDCustomer already exists. Please enter another ID.", "Notifications", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtIDCustomer.Focus();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking IDCustomer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }


            // Check NameCustomer
            if (string.IsNullOrWhiteSpace(txtNameCustomer.Text) || !System.Text.RegularExpressions.Regex.IsMatch(txtNameCustomer.Text, "^[a-zA-Z0-9\\s]+$"))
            {
                MessageBox.Show("NameCustomer cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNameCustomer.Focus();
                return false;
            }


            // Check Gender
            if (cboGender.SelectedItem == null ||
                string.IsNullOrWhiteSpace(cboGender.SelectedItem.ToString()) ||
                !(cboGender.SelectedItem.ToString().Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                  cboGender.SelectedItem.ToString().Equals("Female", StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Gender must be male or female.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                ROW_NUMBER() OVER (ORDER BY IDCustomer) AS NewID,
                IDCustomer
            FROM CUSTOMER
        )
        UPDATE B
        SET B.IDCustomer = N.NewID
        FROM CUSTOMER B
        JOIN NumberedRows N
        ON B.IDCustomer = N.IDCustomer";

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
