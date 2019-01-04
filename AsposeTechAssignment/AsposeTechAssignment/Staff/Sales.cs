using System.Collections.Generic;

namespace Aspose.Staff
{
    public class Sales : BaseManager
    {
        public Sales()
        {
            Subordinates = new List<Employee>();
        }

        public override StaffType StaffType => StaffType.Sales;
    }
}
