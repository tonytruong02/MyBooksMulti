using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBooks.Models;

namespace MyBooks.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        

        //Thêm vào lúc 23/11/2022


        public ActionResult AdminControl()
        {
            return View();
        }


        #region Quản lý đơn hàng
        public ActionResult QLDonHang()
        {
            var list = database.OrderDetails.OrderBy(x => x.IDOrder);
            return View(list);
        }
        
        #endregion

        #region Quản lý Voucher
        public ActionResult QLVoucher()
        {
            var list_voucher = database.VOUCHERs.OrderByDescending(x => x.MAVOUCHER);
            return View(list_voucher);
        }
        [HttpGet]
        public ActionResult EditVou(string id)
        {
            VOUCHER voucher = database.VOUCHERs.FirstOrDefault(x => x.MAVOUCHER == id);
            return View(voucher);
        }
        [HttpPost]
        public ActionResult EditVou(VOUCHER voucher)
        {
            if (ModelState.IsValid)
            {            
                var voucher_update = database.VOUCHERs.Find(voucher.MAVOUCHER);
                voucher_update.MAVOUCHER = voucher.MAVOUCHER;
                voucher_update.GIAM = voucher.GIAM;
                voucher_update.MoTa = voucher.MoTa;
                database.SaveChanges();
                return RedirectToAction("QLVoucher");
            }
            return View(voucher);
        }
        public ActionResult DeleteVoucher(string id)
        {
            var voucher = database.VOUCHERs.FirstOrDefault(x => x.MAVOUCHER == id);
            return View(voucher);
        }
        public PartialViewResult Voucher_Pro(string mavoucher)
        {
            var voucher_pro_list = database.voucher_pro.Where(x => x.IDVoucher == mavoucher).OrderByDescending(x => x.IDBook);
            return PartialView(voucher_pro_list);
        }
        public ActionResult DeleteVoucherNow(string mavoucher)
        {
            // OrderDetail, xóa voucher, đổi giá mới thành giá cũ
            var orderdetail_list = database.OrderDetails.OrderByDescending(x => x.IDProduct);
            foreach(var item in orderdetail_list)
            {
                if (item.IDVoucher == mavoucher)
                {
                    var orderdetail_list_update = database.OrderDetails.Find(item.ID);
                    orderdetail_list_update.PriceNew = orderdetail_list_update.PriceOld;
                    orderdetail_list_update.IDVoucher = null;
                }
            }

            // Xóa hẳn voucher trong voucher_pro

            var voucher_pro_list = database.voucher_pro.OrderByDescending(x => x.IDBook);
            foreach (var item in voucher_pro_list)
            {
                if (item.IDVoucher == mavoucher)
                {
                    database.voucher_pro.Remove(item);
                }
            }

            // Xóa voucher được áp dụng trong cartList

            var cartList_list = database.CartLists.OrderByDescending(x => x.IDCus);
            foreach(var item in cartList_list)
            {
                if (item.IDVoucher == mavoucher)
                {
                    var cartList_update = database.CartLists.Find(item.IDCartList);
                    cartList_update.IDVoucher = null;
                }
            }
            database.SaveChanges();
            return RedirectToAction("QLVoucher", "Admin");
        }
        [HttpGet]
        public ActionResult CreateVoucher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateVoucher(VOUCHER voucher)
        {
            if (ModelState.IsValid)
            {
                var voucher_add = database.VOUCHERs.FirstOrDefault(x => x.MAVOUCHER == voucher.MAVOUCHER);
                if (voucher_add == null) // Chưa có trong danh sách
                {
                    database.VOUCHERs.Add(voucher);
                    database.SaveChanges();
                    return RedirectToAction("QLVoucher", "Admin");
                }
                else
                {
                    ViewBag.Error = "Voucher đã tồn tại";
                    return View();
                }
            }
            return View();
        }
        public ActionResult ShowBookList() // Hiện danh sách thư viện
        {
            var book = database.Books.OrderByDescending(x => x.BookID);
            return View(book);
        }
        [HttpGet]
        public ActionResult LinkVoucher(string idbook) // Hiện thông tin về quyển sách sắp được thêm voucher
        {
            var book = database.Books.FirstOrDefault(x => x.BookID == idbook);
            return View(book);
        }
        [HttpPost]
        public ActionResult LinkVoucher(string idbook, FormCollection form)
        {
            try
            {
                voucher_pro add = new voucher_pro();
                add.IDBook = idbook;
                add.IDVoucher = form["voucherid"];
                database.voucher_pro.Add(add);
                database.SaveChanges();
                return RedirectToAction("QLVoucher");
            }
            catch
            {
                ViewBag.Error = "Lỗi...";
                return View();
            }

        }
        public PartialViewResult LinkedVoucher(string bookid) // Hiện những voucher đã áp dụng cho sách
        {
            var list = database.voucher_pro.Where(x => x.IDBook == bookid).OrderByDescending(x => x.IDVoucher);
            return PartialView(list);
        }
        public PartialViewResult NotLinkedVoucher(string bookid) // Hiện những voucher chưa áp dụng cho sách
        {
            var list = database.voucher_pro.Where(x => x.IDBook == bookid).OrderByDescending(x => x.IDVoucher); // Danh sách những voucher đã sử dụng
            var list2 = database.VOUCHERs.OrderByDescending(x => x.MAVOUCHER); // danh sách các voucher
            List<VOUCHER> list1 = new List<VOUCHER>();
            foreach (var item2 in list2)
            {
                bool check = true;
                foreach( var item1 in list)
                {

                    if(item2.MAVOUCHER == item1.IDVoucher)
                    {
                        check = false;
                        break;
                    }
                }
                if (check == true)
                {
                    list1.Add(item2);
                }
            }
            return PartialView(list1.OrderByDescending(x => x.GIAM));
        }
        public ActionResult Detail()
        {
            var book = database.Books.OrderByDescending(x => x.BookID); // trả về danh sách thư viện
            return View(book);
        }
        public PartialViewResult ListVoucherDetail(string idbook)
        {
            var list = database.voucher_pro.Where(x => x.IDBook == idbook).OrderByDescending(x => x.IDVoucher);
            return PartialView(list);
        }
        #endregion


        #region Quản lý Tài khoản
        public PartialViewResult CountOrder(int idcus)
        {
            IQueryable<OrderPro> orderPros = database.OrderProes.Where(x => x.IDCus == idcus);
            ViewBag.NumberOrder = orderPros.Count();
            return PartialView("CountOrder");
        }
        public ActionResult QLTaiKhoan()
        {
            var list_User = database.Customers.OrderByDescending(x => x.IDCus);
            return View(list_User);
        }
        #endregion

        #region Quản lý Sách
        public PartialViewResult EditBookCategory(string idbook) //Hiện danh sách những chủ đề
        {
            var list_voucher = database.Categories.OrderByDescending(x => x.MaCate);
            return PartialView(list_voucher);
        }
        public PartialViewResult BookVoucher(string idbook) //Danh sách vocher mà quyến sách đang có
        {
            var list_voucher = database.voucher_pro.Where(id => id.IDBook == idbook).OrderByDescending(x => x.IDVoucher);
            return PartialView(list_voucher);
        }
        public ActionResult CreateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                var book_check = database.Books.FirstOrDefault(x => x.BookID == book.BookID);
                if (book_check != null) //Đã tồn tại sách với id này
                {
                    ViewBag.ThongBaoCreate = "Đã tồn tại sách với id này";
                    return View();
                }
                else //Theem vao nè
                {
                    if (ModelState.IsValid)
                    {
                        book_check = book;
                        database.Books.Add(book_check);
                        database.SaveChanges();
                        return RedirectToAction("QLSanPham");
                    }
                    else
                    {
                        return View();
                    }

                }
            }
            return View(book);
        }
        [HttpGet]
        public ActionResult Edit(string id = null)
        {
            var book = database.Books.FirstOrDefault(x => x.BookID == id);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                Book book_update = database.Books.Find(book.BookID);
                Category book_cate = database.Categories.Find((string)form["BookCategory"]);
                book_update.BookID = book.BookID;
                book_update.BookName = book.BookName;
                book_update.NXB = book.NXB;
                book_update.NCC = book.NCC;
                book_update.Category = book.Category;
                book_update.Price = book.Price;
                book_update.DateAdd = book.DateAdd;
                book_update.SLuong = book.SLuong;
                book_update.Imagebook = book.Imagebook;
                database.SaveChanges();
                return RedirectToAction("QLSanPham", "Admin");
            }
            return View(book);
        }
        public ActionResult QLSanPham()
        {
            var list_Book = database.Books.OrderByDescending(x => x.BookName);
            return View(list_Book);
        }
        public ActionResult DeleteBook(string bookid)
        {
            Book book = database.Books.FirstOrDefault(x => x.BookID == bookid);
            Session["bookid"] = book.BookID;
            return View(book);
        }
        public PartialViewResult ChiTietHoaDon(string bookid)
        {
            var list_order = database.OrderDetails.Where(x => x.IDProduct == bookid).OrderByDescending(x => x.OrderPro.Customer.IDCus);
            return PartialView(list_order);
        }
        //Khi xóa sẽ có 4 bảng trong database để xóa lần lượt theo thứ tự
        //CartList -> OrderDeTail -> voucher_pro -> Book
        public ActionResult DeleteBookNow(string id)
        {
            Book book = database.Books.FirstOrDefault(x => x.BookID == id);
            //Xóa trong CartList
            var book_cartList = database.CartLists.OrderByDescending(x => x.IDCartList); //Lấy danh sách trong CartList ra
            foreach (var item in book_cartList)
            {
                if (item.BookID == book.BookID)
                {
                    database.CartLists.Remove(item);
                }
            }
            //Xóa trong OrderDetail (chi tiết hóa đơn)
            var book_orderDetail = database.OrderDetails.OrderByDescending(x => x.ID); //Lấy danh sáng OrderDetail ra
            foreach (var item in book_orderDetail)
            {
                if (item.IDProduct == book.BookID)
                {
                    database.OrderDetails.Remove(item);
                }
            }
            //Xóa trong voucher_pro (danh sách sản phẩm có voucher)
            var book_voucher_Pro = database.voucher_pro.OrderByDescending(x => x.ID); //Lấy danh sách OrderDetail ra
            foreach (var item in book_voucher_Pro)
            {
                if (item.IDBook == book.BookID)
                {
                    database.voucher_pro.Remove(item);
                }
            }
            //Xóa trong Books (xóa trong thư viện sách Book)
            var book_Book = database.Books.Find(book.BookID); //Lấy sách ra
            database.Books.Remove(book_Book);
            database.SaveChanges();
            return RedirectToAction("QLSanPham", "Admin");

        }


        #endregion
    }
}