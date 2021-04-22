using System.Collections.Generic;

namespace BakeryMS.API.Common.DTOs
{
    public class DashboardDto
    {
        public IList<decimal> Expense { get; set; }
        public IList<decimal> Income { get; set; }
        public IList<decimal> Net { get; set; }

        public int OrderReceived { get; set; }
        public int MaxOrders { get; set; }

        public int OrdersHandled { get; set; }
        public int MaxOrdersToHandle { get; set; }

        public int Reorders { get; set; }
        public int MaxItems { get; set; }

        public int workers { get; set; }
        public int workersMax { get; set; }


    }
}