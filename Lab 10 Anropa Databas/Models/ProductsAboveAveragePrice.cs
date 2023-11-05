using System;
using System.Collections.Generic;

namespace Lab_10_Anropa_Databas.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
    }
}
