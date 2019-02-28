using System;
using System.Collections.Generic;

namespace PlanlogicAI.Data
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int PlatformId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string SubType { get; set; }
    }
}
