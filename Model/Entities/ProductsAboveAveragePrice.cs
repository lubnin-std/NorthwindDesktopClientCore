using System;
using System.Collections.Generic;

namespace NorthwindDesktopClientCore.Model.Entities
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
