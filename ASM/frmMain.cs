using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private string userRole;
        public frmMain(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void frmMain_Load(object sender, EventArgs e)
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
            // Enable all buttons
            btnStaff.Enabled = true;
            btnProvider.Enabled = true;
            btnCategory.Enabled = true;
            btnCustomer.Enabled = true;
            btnShoes.Enabled = true;
            btnShoeDetails.Enabled = true;
            btnBill.Enabled = true;
            btnBillDetails.Enabled = true;
            btnStatistics.Enabled = true;
            btnBack.Enabled = true;
            btnClose.Enabled = true;
        }


        private void EnableStaffFeatures()
        {
            btnCustomer.Enabled = true;
            btnShoes.Enabled = true;
            btnShoeDetails.Enabled = true;
            btnBill.Enabled = true;
            btnBillDetails.Enabled = true;
            btnStatistics.Enabled = true;
            btnBack.Enabled = true;
            btnClose.Enabled = true;

            btnCategory.Enabled = false;
            btnProvider.Enabled = false;
            btnStaff.Enabled = false;
        }



        // Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        // Back
        private void btnBack_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
            this.Hide();
        }


        // Category
        private void btnCategory_Click(object sender, EventArgs e)
        {
            frmCategory frmCategory = new frmCategory("admin");    
            frmCategory.Show();
            this.Hide();
        }


        // Customer
        private void btnCustomer_Click(object sender, EventArgs e)
        {           
            frmCustomer frmCustomer = new frmCustomer(userRole);
            frmCustomer.Show();
            this.Hide();
        }


        // Provider
        private void btnProvider_Click(object sender, EventArgs e)
        {
            frmProvider frmProvider = new frmProvider("admin");
            frmProvider.Show();
            this.Hide();
        }


        // Staff
        private void btnStaff_Click(object sender, EventArgs e)
        {
            frmStaff frmStaff = new frmStaff("admin");
            frmStaff.Show();
            this.Hide();
        }


        // Shoes
        private void btnShoes_Click(object sender, EventArgs e)
        {
            frmShoes frmShoes = new frmShoes(userRole);
            frmShoes.Show();
            this.Hide();
        }


        // Shoe details
        private void btnShoeDetails_Click(object sender, EventArgs e)
        {
            frmShoeDetails frmShoeDetails = new frmShoeDetails(userRole);
            frmShoeDetails.Show();
            this.Hide();
        }


        // Bill
        private void btnBill_Click(object sender, EventArgs e)
        {
            frmBill frmBill = new frmBill(userRole);
            frmBill.Show();
            this.Hide();
        }


        // Bill details
        private void btnBillDetails_Click(object sender, EventArgs e)
        {
            frmBillDetails frmBillDetails = new frmBillDetails(userRole);
            frmBillDetails.Show();
            this.Hide();
        }


        // Statistics
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            frmStatistics frmStatistics = new frmStatistics(userRole);
            frmStatistics.Show();
            this.Hide();
        }      
    }
}
