using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.IoC;
using Aspose.Services.Interfaces;
using Aspose.Staff;
using Autofac;
using NUnit.Framework;

namespace Aspose.Tests
{
    [TestFixture]
    public class SalaryTests
    {
        private IContainer container;
        private ISalaryService salaryService;
        private IList<Employee> employees;

        [Test]
        public void CalulateSalaryTest()
        {
            var employee = employees.First(x=>x.Id == 1);
            var salary = salaryService.GetSalary(employee);
            Assert.AreEqual(1m, salary);
        }

        [OneTimeSetUp]
        public void Setup()
        {
            var builder = ContainerConfig.Register();
            container = builder.Build();
            salaryService = container.Resolve<ISalaryService>();
            employees = Init();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            container.Dispose();
        }

        private static List<Employee> Init()
        {
            var baseSalary = 1000m;
            var john = new Manager
            {
                Id = 1,
                BaseSalary = baseSalary,
                Name = "John",
                HireDate = new DateTimeOffset(new DateTime(1999, 1, 1)),
            };

            var michael = new Manager
            {
                Id = 2,
                BaseSalary = baseSalary,
                Name = "Michael",
                HireDate = new DateTimeOffset(new DateTime(2000, 3, 15)),
                Supervisor = john
            };

            var anna = new Sales
            {
                Id = 3,
                BaseSalary = baseSalary,
                HireDate = new DateTimeOffset(new DateTime(2001, 4, 11)),
                Name = "Anna",
                Supervisor = john
            };

            var dave = new Sales
            {
                Id = 4,
                BaseSalary = baseSalary,
                HireDate = new DateTimeOffset(new DateTime(2005, 8, 24)),
                Name = "Dave",
                Supervisor = anna
            };


            var susan = new Employee
            {
                Id = 5,
                BaseSalary = baseSalary,
                HireDate = new DateTimeOffset(new DateTime(2006, 1, 7)),
                Name = "Susan",
                Supervisor = michael
            };

            var larry = new Employee
            {
                Id = 6,
                BaseSalary = baseSalary,
                HireDate = new DateTimeOffset(new DateTime(2011, 5, 4)),
                Name = "Larry",
                Supervisor = anna
            };

            john.Subordinates.Add(anna);
            john.Subordinates.Add(michael);
            michael.Subordinates.Add(susan);
            anna.Subordinates.Add(larry);
            anna.Subordinates.Add(dave);

            return new List<Employee> { john, michael, anna, dave, larry, susan };
        }
    }
}
