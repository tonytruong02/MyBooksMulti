using MyBooks.Models;
using System.Linq;
using System.Web.Mvc;

namespace MyBooks.Controllers
{
    public class SignController : Controller
    {
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(Customer cus)
        {
            var check_admin = database.AdminUsers.FirstOrDefault(s => s.ID == cus.IDCus && s.PasswordUser == cus.PassCus);
            if (check_admin != null)
            {
                Session["AdminName"] = check_admin.NameUser;
                return RedirectToAction("AdminControl", "Admin");
            }
            var check = database.Customers.Where(s => s.IDCus == cus.IDCus && s.PassCus == cus.PassCus).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "Sai Info";
                return View("DangNhap");
            }
            else
            {
                Session["IDCus"] = check.IDCus;
                Session["NameCus"] = check.NameCus;
                Session["PassCus"] = check.PassCus;
                var list = database.CartLists.Where(s => s.IDCus == check.IDCus).OrderByDescending(x => x.IDCus);
                foreach (var n in list)
                {
                    AddToCart(n.BookID,  n.IDVoucher, (int)n.IDCus);
                }
                return RedirectToAction("TrangChu", "Product");
            }
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(Customer kh)
        {
            var acc = database.Customers.FirstOrDefault(k => k.IDCus == kh.IDCus);
            if (acc != null) //Đã có account chứa tài khoản đăng kí
            {
                ViewBag.ThongBaoDangKy = "Tài khoản đã tồn tại";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    database.Customers.Add(kh);
                    database.SaveChanges();
                    return RedirectToAction("DangNhap");
                }
                return View(kh);
            }
        }
        public ActionResult ShowInfo(int idCus = -1)
        {
            if (idCus != -1)
            {
                var info = database.Customers.FirstOrDefault(s => s.IDCus == idCus);
                return View(info);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "Sign");
        }
        #region From ShoppingController
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
        //Action them product vao gio hang AddToCart(n.BookID, (int)n.IDCus, n.IDVoucher);
        public ActionResult AddToCart(string id, string idvoucher, int idcus = -1)
        {
            var _pro = database.Books.SingleOrDefault(s => s.BookID == id);         //lay Pro theo ID
            var cus = database.Customers.FirstOrDefault(s => s.IDCus == idcus); // auto null
            var _voucher = database.VOUCHERs.FirstOrDefault(s => s.MAVOUCHER == idvoucher);
            if (_pro != null)
            {
                GetCart().Add_Book_Cart(_pro, _voucher, cus);
            }
            return RedirectToAction("ShowCart", "ShoppingCart");

        }
        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string id_Pro = form["idPro"];
            int _quantity = int.Parse(form["CartQuantity"]);
            cart.Update_Quantity(id_Pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        #endregion


    }
}