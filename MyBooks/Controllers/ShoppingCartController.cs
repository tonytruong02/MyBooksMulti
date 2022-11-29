using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBooks.Models;


namespace MyBooks.Controllers
{
    public class ShoppingCartController : Controller
    {
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        public ActionResult CheckOut(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            OrderPro _order = new OrderPro();
            _order.DateOrder = DateTime.Now;
            _order.AddressDeliverry = form["AddressDelivery"];
            _order.IDCus = int.Parse(form["CodeCustomer"]);
            database.OrderProes.Add(_order);
            foreach (var item in cart.Items)
            {
                OrderDetail _order_detail = new OrderDetail();
                _order_detail.IDOrder = _order.ID;
                _order_detail.IDProduct = item._book.BookID;
                _order_detail.PriceOld = (double)item._book.Price;
                _order_detail.PriceNew = (double)item._newPrice;
                _order_detail.IDVoucher = item._voucher;
                _order_detail.Quantity = item._quantity;
                database.OrderDetails.Add(_order_detail);
                foreach (var p in database.Books.Where(s => s.BookID == _order_detail.IDProduct))
                {
                    var update_quan_pro = p.SLuong - item._quantity;
                    p.SLuong = update_quan_pro;
                }
            }
            database.SaveChanges();
            cart.ClearCart();
            #region Remove from db
            CartList cus_cartList = database.CartLists.FirstOrDefault( s => s.IDCus == _order.IDCus);
            if (cus_cartList != null)
            {
                do
                {
                    database.CartLists.Remove(cus_cartList);
                    database.SaveChanges();
                    cus_cartList = database.CartLists.FirstOrDefault(s => s.IDCus == _order.IDCus);
                } while (cus_cartList != null);
            }
            #endregion
            return RedirectToAction("CheckOut_Success", "ShoppingCart");

            //catch
            //{
            //    return Content("Error checkout. Please check information of Customer ... Thanks");
            //}
        }
        public ActionResult CheckOut_Success()
        {
            return View();
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public ActionResult Index()
        {
            return View();
        }
        //Action tao moi gio hang
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        //Action them product vao gio hang
        public ActionResult AddToCart(string id, FormCollection form ,int idcus = -1)
        {
            var _pro = database.Books.SingleOrDefault(s => s.BookID == id);         //lay Pro theo ID
            var cus = database.Customers.FirstOrDefault(s => s.IDCus == idcus); // auto null
            if (form["VoucherCode"] == null)
            {
                if (_pro != null)
                {
                    GetCart().Add_Book_Cart(_pro, cus);
                }
                return RedirectToAction("ShowCart", "ShoppingCart");
            }
            else {
                string voucher = form["VoucherCode"];
                var _voucher = database.VOUCHERs.FirstOrDefault(s => s.MAVOUCHER == voucher);
                if (_pro != null)
                {
                    GetCart().Add_Book_Cart(_pro, _voucher, cus);
                }
                return RedirectToAction("ShowCart", "ShoppingCart");
            }
        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string id_Pro = (form["idPro"]);
            int _quantity = int.Parse(form["CartQuantity"]);
            cart.Update_Quantity(id_Pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
            
        }
        public ActionResult RemoveCart(string id, int idcus = -1)
        {
            Cart cart = Session["Cart"] as Cart;
            var cus = database.CartLists.FirstOrDefault(x => x.IDCus == idcus);
            cart.Remove_CartItem(id, cus);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int toltal_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                toltal_quantity_item = cart.Total_quantity();
            ViewBag.QuantityCart = toltal_quantity_item;
            return PartialView("BagCart");
        }

    }
}