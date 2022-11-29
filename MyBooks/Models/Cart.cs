using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBooks.Models;
namespace MyBooks.Models
{
    //Tao cac CartItem chua cac don san pham cua gio hang
    public class CartItem
    {
        public Book _book { get; set; }
        public int _quantity { get; set; }
        public string _voucher { get; set; }
        public decimal _newPrice { set; get; }
    }
    public class Cart
    {   
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        List<CartItem>items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }
        public void Add_Book_Cart(Book _pro, Customer cus, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._book.BookID == _pro.BookID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _book = _pro,
                    _quantity = _quan,
                    _newPrice = (decimal)(_pro.Price - _pro.Price * 0),
                });
            }
            else
            {
                item._quantity += _quan;
            }
            //xử lí giỏ hàng customer. 
            // Nếu cartList không tồn tại khách hàng và sản phẩm, thì thêm mới
            if (cus != null)
            {
                var kh = database.CartLists.FirstOrDefault(s => s.IDCus == cus.IDCus && s.BookID == _pro.BookID);
                CartList cartList = new CartList();
                if (kh == null)
                {
                    cartList.IDCus = cus.IDCus;
                    cartList.BookID = _pro.BookID;
                    database.CartLists.Add(cartList);
                    database.SaveChanges();
                }
            }
        }

        public void Add_Book_Cart(Book _pro, VOUCHER _voucher, Customer cus, int _quan = 1)
        {
            var item = Items.FirstOrDefault(s => s._book.BookID == _pro.BookID);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    _book = _pro,
                    _quantity = _quan,
                    _newPrice = (decimal)(_pro.Price - _pro.Price * _voucher.GIAM),
                    _voucher = _voucher.MAVOUCHER
                }); 
            }
            else
            {
                item._quantity += _quan;
            }
            //xử lí giỏ hàng customer. 
            // Nếu cartList không tồn tại khách hàng và sản phẩm, thì thêm mới
            if (cus != null)
            {
                var kh = database.CartLists.FirstOrDefault(s => s.IDCus == cus.IDCus && s.BookID == _pro.BookID);
                CartList cartList = new CartList();
                if (kh == null)
                {
                    cartList.IDCus = cus.IDCus;
                    cartList.BookID = _pro.BookID;
                    cartList.IDVoucher = _voucher.MAVOUCHER;
                    database.CartLists.Add(cartList);
                    database.SaveChanges();
                }
            }
        }
        public int Total_quantity()
        {
            return items.Sum(s => s._quantity);
        }
        public decimal Total_money()
        {
            var total = items.Sum(s => s._quantity * s._newPrice);
                
            return (decimal)total;
        }
        public void Update_Quantity(string id, int _new_quan)
        {
            var item = items.Find(s => s._book.BookID == id);
            if (item != null)
            {
                if (items.Find(s => s._book.SLuong > _new_quan) != null && _new_quan > 0)
                {
                    item._quantity = _new_quan;
                }
                    
                else
                {
                    item._quantity = 1;
                }
                    
            }
        }
        public void Remove_CartItem(string id, CartList customer)
        {
            items.RemoveAll(s => s._book.BookID == id);
            //Xóa trong cartList
            if (customer != null)
            {
                CartList obj = database.CartLists.FirstOrDefault(x => x.BookID == id && x.IDCus == customer.IDCus);
                database.CartLists.Remove(obj);
                database.SaveChanges();
            }    
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}
