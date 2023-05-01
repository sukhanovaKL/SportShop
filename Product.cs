//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportShop
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Windows.Media;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderProduct = new HashSet<OrderProduct>();
        }
    
        public int ProductID { get; set; }
        public string ProductArticleNumber { get; set; }
        public string ProductName { get; set; }
        public int UnitTypeID { get; set; }
        public decimal ProductCost { get; set; }
        public byte? ProductMaxDiscountAmount { get; set; }
        public int ProductManufacturerID { get; set; }
        public int ProductSupplierID { get; set; }
        public int ProductCategoryID { get; set; }
        public byte? ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPhoto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProduct { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductManufacturer ProductManufacturer { get; set; }
        public virtual ProductSupplier ProductSupplier { get; set; }
        public virtual UnitType UnitType { get; set; }

        [NotMapped]
        public SolidColorBrush ColorProductDiscountAmount => ProductMaxDiscountAmount > 15 ? new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#7fff00")) : new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));

        [NotMapped]
        public string ProductPhotoFromResources => "/Photos/" + ProductPhoto;

        [NotMapped]
        public double ProductCostWithAmount => double.Parse(ProductCost.ToString()) - (double.Parse(ProductCost.ToString()) * (double.Parse(ProductMaxDiscountAmount.ToString()) / 100.0));
    }
}
