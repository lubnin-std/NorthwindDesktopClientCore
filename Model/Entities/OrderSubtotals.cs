using System;
using System.Collections.Generic;

namespace NorthwindDesktopClientCore.Model.Entities
{
    public partial class OrderSubtotals
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
