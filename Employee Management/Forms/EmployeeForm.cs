using Employee_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utils;

namespace Employee_Management.Forms
{
    public partial class EmployeeForm : Form
    {
        private DataAccess _dataAccess = new DataAccess();

        public EmployeeForm()
        {
            InitializeComponent();
            LoadEmployees();
            LoadDesignations();
            LoadDepartments();
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void LoadDesignations()
        {
            try
            {
                List<Designation> designations = _dataAccess.GetDesignations();
                cmbDesignation.DataSource = designations;
                cmbDesignation.DisplayMember = "DesignationName";
                cmbDesignation.ValueMember = "DesignationID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading designations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDepartments()
        {
            try
            {
                List<Department> departments = _dataAccess.GetDepartments();
                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                List<Employee> employees = _dataAccess.GetEmployees();

                dataGridView1.DataSource = employees.Select(emp => new
                {
                    emp.EmployeeID,
                    emp.FirstName,
                    emp.LastName,
                    emp.Salary,
                    emp.DateOfHire,
                    DesignationName = emp.Designation.DesignationName,
                    DepartmentName = emp.Department.DepartmentName
                }).ToList();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.Columns["EmployeeID"].HeaderText = "Employee ID";
                dataGridView1.Columns["FirstName"].HeaderText = "First Name";
                dataGridView1.Columns["LastName"].HeaderText = "Last Name";
                dataGridView1.Columns["Salary"].HeaderText = "Salary";
                dataGridView1.Columns["DateOfHire"].HeaderText = "Date of Hire";
                dataGridView1.Columns["DesignationName"].HeaderText = "Designation";
                dataGridView1.Columns["DepartmentName"].HeaderText = "Department";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtEmployeeID.Text = row.Cells["EmployeeID"].Value.ToString();
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                dtpDateOfHire.Value = Convert.ToDateTime(row.Cells["DateOfHire"].Value);
                txtSalary.Text = row.Cells["Salary"].Value.ToString();

                cmbDesignation.SelectedValue = GetDesignationID(row.Cells["DesignationName"].Value.ToString());
                cmbDepartment.SelectedValue = GetDepartmentID(row.Cells["DepartmentName"].Value.ToString());

                txtEmployeeID.Enabled = false;

            }
        }

        private bool ValidateInput()
        {
            if (Validator.IsFieldEmpty(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (Validator.IsFieldEmpty(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbDesignation.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Designation.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Department.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            if (!Validator.IsValidDate(dtpDateOfHire.Value))
            {
                MessageBox.Show("Date of Hire cannot be in the future.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Validator.IsValidSalary(txtSalary.Text))
            {
                MessageBox.Show("Invalid salary. Please enter a valid numeric value.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtEmployeeID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            cmbDesignation.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;
            txtSalary.Clear();
            dtpDateOfHire.Value = DateTime.Now;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    Employee emp = new Employee
                    {
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        DesignationID = (int)cmbDesignation.SelectedValue,
                        DepartmentID = (int)cmbDepartment.SelectedValue,
                        DateOfHire = dtpDateOfHire.Value,
                        Salary = decimal.Parse(txtSalary.Text)
                    };

                    _dataAccess.AddEmployee(emp);
                    LoadEmployees();
                    MessageBox.Show("Employee added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    Employee emp = new Employee
                    {
                        EmployeeID = int.Parse(txtEmployeeID.Text),
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        DesignationID = (int)cmbDesignation.SelectedValue,
                        DepartmentID = (int)cmbDepartment.SelectedValue,
                        DateOfHire = dtpDateOfHire.Value,
                        Salary = decimal.Parse(txtSalary.Text)
                    };

                    _dataAccess.UpdateEmployee(emp);
                    LoadEmployees();
                    MessageBox.Show("Employee updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!Validator.IsFieldEmpty(txtEmployeeID.Text))
            {
                try
                {
                    int employeeID = int.Parse(txtEmployeeID.Text);
                    _dataAccess.DeleteEmployee(employeeID);
                    LoadEmployees();
                    MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Employee ID is required for deletion.", "Input Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtEmployeeID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtSalary.Clear();
            dtpDateOfHire.Value = DateTime.Now;

            cmbDesignation.SelectedIndex = -1;
            cmbDepartment.SelectedIndex = -1;

            txtEmployeeID.Enabled = true;
        }

        private int GetDesignationID(string designationName)
        {
            foreach (Designation d in cmbDesignation.Items)
            {
                if (d.DesignationName == designationName)
                {
                    return d.DesignationID;
                }
            }
            return -1;
        }

        private int GetDepartmentID(string departmentName)
        {
            foreach (Department dep in cmbDepartment.Items)
            {
                if (dep.DepartmentName == departmentName)
                {
                    return dep.DepartmentID;
                }
            }
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                List<Employee> employees = _dataAccess.SearchEmployees(search_tb.Text);

                dataGridView1.DataSource = employees.Select(emp => new
                {
                    emp.EmployeeID,
                    emp.FirstName,
                    emp.LastName,
                    emp.Salary,
                    emp.DateOfHire,
                    DesignationName = emp.Designation.DesignationName,
                    DepartmentName = emp.Department.DepartmentName
                }).ToList();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.Columns["EmployeeID"].HeaderText = "Employee ID";
                dataGridView1.Columns["FirstName"].HeaderText = "First Name";
                dataGridView1.Columns["LastName"].HeaderText = "Last Name";
                dataGridView1.Columns["Salary"].HeaderText = "Salary";
                dataGridView1.Columns["DateOfHire"].HeaderText = "Date of Hire";
                dataGridView1.Columns["DesignationName"].HeaderText = "Designation";
                dataGridView1.Columns["DepartmentName"].HeaderText = "Department";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
