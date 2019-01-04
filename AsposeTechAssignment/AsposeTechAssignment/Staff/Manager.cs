using System.Collections.Generic;

namespace Aspose.Staff
{
    public class Manager : BaseManager
    {
        public Manager()
        {
            this.Subordinates = new List<Employee>();
        }

        public override StaffType StaffType => StaffType.Manager;
    }
}
