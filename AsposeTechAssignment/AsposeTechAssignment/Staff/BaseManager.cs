using System.Collections.Generic;

namespace Aspose.Staff
{
    public abstract class BaseManager : Employee
    {
        public IList<Employee> Subordinates { get; set; }
    }
}
