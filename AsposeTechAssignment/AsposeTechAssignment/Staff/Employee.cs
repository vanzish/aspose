using System;

namespace Aspose.Staff
{
    public class Employee
    {
        //Key to strongly identify Employees (even with same Names)
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime HireDate { get; set; }

        public decimal BaseSalary { get; set; }

        public BaseManager Supervisor { get; set; }

        public virtual StaffType StaffType => StaffType.Employee;
    }
}
