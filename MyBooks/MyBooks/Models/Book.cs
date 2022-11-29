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

    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.CartLists = new HashSet<CartList>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.voucher_pro = new HashSet<voucher_pro>();
        }
        [Required(ErrorMessage = "Mã sách không được để trống")]
        public string BookID { get; set; }
        [Required(ErrorMessage = "Tên sách không được để trống")] 
        public string BookName { get; set; }
        [Required]
        public string NXB { get; set; }
        [Required(ErrorMessage = "Nhà cung cấp không được để trống")] 
        public string NCC { get; set; }
        [Required(ErrorMessage = "Chủ đề không được để trống")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Đơn vị không được để trống")]
        public string Unit { get; set; }
        [Required(ErrorMessage = "Giá tiền không được để trống")]
        [Range(1000, int.MaxValue, ErrorMessage ="Giá phải lớn hơn 1.000 VNĐ")]
        public Nullable<decimal> Price { get; set; }
        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(0,int.MaxValue, ErrorMessage = "Số lượng là số nguyên dương")]
        public Nullable<int> SLuong { get; set; }
        [Required(ErrorMessage = "Ngày không được để trống")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DateAdd { get; set; }
        [Required(ErrorMessage = "Ảnh không được để trống")]
        public string Imagebook { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartList> CartLists { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<voucher_pro> voucher_pro { get; set; }
        public virtual Category Category1 { get; set; }
    }
}