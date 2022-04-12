//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.ProductOrders = new HashSet<ProductOrder>();
        }
    
        public int Id { get; set; }
        public Nullable<int> WorkerId { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> ClientId { get; set; }
        public Nullable<int> StatusOrderId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual StatusOrder StatusOrder { get; set; }
        public virtual Worker Worker { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
