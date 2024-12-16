using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace ASM
{
    public partial class frmStatistics : Form
    {

        private string connectionString = "Data Source=LAPTOP-1UOTIHIH\\KHANG1102;" +
            "Initial Catalog=SHOE_SALES_MANAGEMENT_SYSTEM;Integrated Security=True";

        public frmStatistics()
        {
            InitializeComponent();
        }

        private string userRole;
        public frmStatistics(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void frmStatistics_Load(object sender, EventArgs e)
        {

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
            btnStatistics.Enabled = true;
        }


        private void EnableStaffFeatures()
        {
            btnBack.Enabled = true;
            btnClose.Enabled = true;
            btnStatistics.Enabled = true;
        }


        // Combo box
        private void cboTime_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }



        // Statistics
        private void btnStatistics_Click(object sender, EventArgs e)
        {      
            DateTime StartDate = dtpStartDate.Value;
            DateTime EndDate = dtpEndDate.Value; 

            if (StartDate > EndDate)
            {
                MessageBox.Show("Start date must be earlier than end date.");
                return;
            }

            GetStatisticsByDate(StartDate, EndDate);        
        }


        // Get Statistics By Date
        private void GetStatisticsByDate(DateTime StartDate, DateTime EndDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    COUNT(DISTINCT BILL.IDBill) AS TotalBills, 

                    SUM(BILLDETAILS.TotalPrices) AS TotalRevenue, 

                    (SELECT TOP 1 SHOES.NameShoes 
                     FROM BILLDETAILS 
                     INNER JOIN SHOES ON BILLDETAILS.IDShoes = SHOES.IDShoes 
                     WHERE BILLDETAILS.IDBill IN (SELECT IDBill FROM BILL WHERE BILL.DateBill BETWEEN @StartDate AND @EndDate) 
                     GROUP BY SHOES.NameShoes 
                     ORDER BY SUM(BILLDETAILS.Quantity) DESC) AS BestSellingProduct, 

                    (SELECT TOP 1 SUM(BILLDETAILS.Quantity) 
                     FROM BILLDETAILS 
                     INNER JOIN SHOES ON BILLDETAILS.IDShoes = SHOES.IDShoes 
                     WHERE BILLDETAILS.IDBill IN (SELECT IDBill FROM BILL WHERE BILL.DateBill BETWEEN @StartDate AND @EndDate) 
                     GROUP BY SHOES.NameShoes 
                     ORDER BY SUM(BILLDETAILS.Quantity) DESC) AS BestSellingQuantity,

                    (SELECT TOP 1 SHOES.NameShoes 
                     FROM BILLDETAILS 
                     INNER JOIN SHOES ON BILLDETAILS.IDShoes = SHOES.IDShoes 
                     WHERE BILLDETAILS.IDBill IN (SELECT IDBill FROM BILL WHERE BILL.DateBill BETWEEN @StartDate AND @EndDate) 
                     GROUP BY SHOES.NameShoes 
                     ORDER BY SUM(BILLDETAILS.Quantity) ASC) AS LeastSellingProduct,

                    (SELECT TOP 1 SUM(BILLDETAILS.Quantity) 
                     FROM BILLDETAILS 
                     INNER JOIN SHOES ON BILLDETAILS.IDShoes = SHOES.IDShoes 
                     WHERE BILLDETAILS.IDBill IN (SELECT IDBill FROM BILL WHERE BILL.DateBill BETWEEN @StartDate AND @EndDate) 
                     GROUP BY SHOES.NameShoes 
                     ORDER BY SUM(BILLDETAILS.Quantity) ASC) AS LeastSellingQuantity

                FROM BILL 
                INNER JOIN BILLDETAILS ON BILL.IDBill = BILLDETAILS.IDBill 
                WHERE BILL.DateBill BETWEEN @StartDate AND @EndDate;
            ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Gán giá trị vào các Label
                            lblTotalBills.Text = reader["TotalBills"].ToString();
                            lblTotalRevenue.Text = reader["TotalRevenue"].ToString();
                            lblBestSellingProduct.Text = $"{reader["BestSellingProduct"]} (Quantity: {reader["BestSellingQuantity"]})";
                            lblLeastSellingProduct.Text = $"{reader["LeastSellingProduct"]} (Quantity: {reader["LeastSellingQuantity"]})";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
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
    }
}
