using Employee_Management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Employee_Management.Forms
{
    public partial class LoginForm : Form
    {
        private DataAccess _dataAccess = new DataAccess();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validator.IsFieldEmpty(username_tb.Text))
            {
                MessageBox.Show("First Name is required.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Validator.IsFieldEmpty(password_tb.Text))
            {
                MessageBox.Show("First Name is required.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                

               bool isSuccess =  _dataAccess.AuthenticateUser(username_tb.Text, password_tb.Text);

                if (isSuccess) { 
                
                    EmployeeForm employeeForm = new EmployeeForm();
                    employeeForm.Show();

                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Username or Password is Wrong!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
