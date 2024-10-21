using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DesignationID { get; set; } 

        public int DepartmentID { get; set; }

        public DateTime DateOfHire { get; set; }
        public decimal Salary { get; set; }

        public Designation Designation { get; set; } 
        public Department Department { get; set; }
    }
}
