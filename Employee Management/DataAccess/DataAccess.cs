using Employee_Management.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Employee_Management
{
    public class DataAccess
    {
        private readonly string _connectionString = "server=localhost;database=payroll_management_system;user=root;password=;SslMode=None;";

        public bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);  // Note: You should hash passwords in a real-world scenario

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    isAuthenticated = result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during user authentication: {ex.Message}");
            }

            return isAuthenticated;
        }


        public List<Designation> GetDesignations()
        {
            List<Designation> designations = new List<Designation>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM Designation", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        designations.Add(new Designation
                        {
                            DesignationID = (int)reader["DesignationID"],
                            DesignationName = reader["DesignationName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving designations: {ex.Message}");
            }

            return designations;
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM Department", con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            DepartmentID = (int)reader["DepartmentID"],
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving departments: {ex.Message}");
            }

            return departments;
        }

        // Method to retrieve all employees from the database
        public List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"
                        SELECT e.EmployeeID, e.FirstName, e.LastName, e.Salary, e.DateOfHire,
                               d.DesignationID, d.DesignationName, dep.DepartmentID, dep.DepartmentName
                        FROM Employees e
                        JOIN Designation d ON e.DesignationID = d.DesignationID
                        JOIN Department dep ON e.DepartmentID = dep.DepartmentID";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            EmployeeID = (int)reader["EmployeeID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Salary = (decimal)reader["Salary"],
                            DateOfHire = (DateTime)reader["DateOfHire"],
                            Designation = new Designation
                            {
                                DesignationName = reader["DesignationName"].ToString()
                            },
                            Department = new Department
                            {
                                DepartmentName = reader["DepartmentName"].ToString()
                            }
                        };

                        employees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employees: {ex.Message}");
            }

            return employees;
        }

        // Method to add a new employee to the database
        public void AddEmployee(Employee employee)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"
                        INSERT INTO Employees (FirstName, LastName, Salary, DateOfHire, DesignationID, DepartmentID)
                        VALUES (@FirstName, @LastName, @Salary, @DateOfHire, @DesignationID, @DepartmentID)";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@DateOfHire", employee.DateOfHire);
                    cmd.Parameters.AddWithValue("@DesignationID", employee.DesignationID);
                    cmd.Parameters.AddWithValue("@DepartmentID", employee.DepartmentID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new Exception($"Error while adding employee: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the employee: {ex.Message}", ex);
            }
        }

        // Method to update an existing employee in the database
        public void UpdateEmployee(Employee employee)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"
                        UPDATE Employees 
                        SET FirstName = @FirstName, LastName = @LastName, Salary = @Salary, DateOfHire = @DateOfHire,
                            DesignationID = @DesignationID, DepartmentID = @DepartmentID
                        WHERE EmployeeID = @EmployeeID";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeID);
                    cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", employee.LastName);
                    cmd.Parameters.AddWithValue("@Salary", employee.Salary);
                    cmd.Parameters.AddWithValue("@DateOfHire", employee.DateOfHire);
                    cmd.Parameters.AddWithValue("@DesignationID", employee.DesignationID);
                    cmd.Parameters.AddWithValue("@DepartmentID", employee.DepartmentID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new Exception($"Error while updating employee: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the employee: {ex.Message}", ex);
            }
        }

        // Method to delete an employee from the database
        public void DeleteEmployee(int employeeID)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID", con);
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException sqlEx)
            {
                throw new Exception($"Error while deleting employee: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the employee: {ex.Message}", ex);
            }
        }

        public List<Employee> SearchEmployees(String employeeId)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    con.Open();
                    string query = @"
                        SELECT e.EmployeeID, e.FirstName, e.LastName, e.Salary, e.DateOfHire,
                               d.DesignationID, d.DesignationName, dep.DepartmentID, dep.DepartmentName
                        FROM Employees e
                        JOIN Designation d ON e.DesignationID = d.DesignationID
                        JOIN Department dep ON e.DepartmentID = dep.DepartmentID WHERE e.EmployeeID = " + employeeId;

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Employee emp = new Employee
                        {
                            EmployeeID = (int)reader["EmployeeID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Salary = (decimal)reader["Salary"],
                            DateOfHire = (DateTime)reader["DateOfHire"],
                            Designation = new Designation
                            {
                                DesignationName = reader["DesignationName"].ToString()
                            },
                            Department = new Department
                            {
                                DepartmentName = reader["DepartmentName"].ToString()
                            }
                        };

                        employees.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employees: {ex.Message}");
            }

            return employees;
        }

       

    }
}
