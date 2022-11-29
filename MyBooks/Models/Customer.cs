﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBooks.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.CartLists = new HashSet<CartList>();
            this.OrderProes = new HashSet<OrderPro>();
        }
        [Required]
        [Range(1, 999999999, ErrorMessage = "Mã khách hàng dưới 10 chữ số")]
        public int IDCus { get; set; }
        [Required]
        public string NameCus { get; set; }
        [Required]
        public string PassCus { get; set; }
        
        [DataType(DataType.PhoneNumber, ErrorMessage ="Số điện thoại không hợp lệ")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại có 10 chữ số")]
        [Required]
        public string PhoneCus { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage ="Email không hợp lệ")]
        public string EmailCus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartList> CartLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPro> OrderProes { get; set; }
    }
}
