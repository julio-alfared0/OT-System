using System;
using System.Collections.Generic;

namespace OT.Resources.ViewModels
{
    public class ProductItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }

    public class HomeViewModel
    {
        public IEnumerable<ProductItemViewModel> Products { get; set; } = new List<ProductItemViewModel>();
    }
}
