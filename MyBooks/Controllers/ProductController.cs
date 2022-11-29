using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBooks.Controllers;
using MyBooks.Models;

namespace MyBooks.Controllers
{
    public class ProductController : Controller
    {
        
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        // GET: Product
        public ActionResult TrangChuMore(string category = null)
        {
            if (category == null)
            {
                var productList = database.Books.OrderByDescending(x => x.BookName);
                return View(productList);
            } else
            {
                var productList = database.Books.Where(p => p.Category == category).OrderByDescending(x => x.BookName);
                return View(productList);
            }
        }
        public ActionResult TrangChu()
        {
            var productList = database.Books.Take(4).OrderByDescending(x => x.BookName);
            
                return View(productList);
        }
        public PartialViewResult LastestBook()
        {
            var list = database.Books.Take(4).OrderByDescending(x => x.DateAdd);
            return PartialView(list);
        }
        private ActionResult View(IOrderedQueryable<Book> productList, IQueryable<CartList> list)
        {
            throw new NotImplementedException();
        }

        public ActionResult SearchOption(double min = (double)double.MinValue, double max = double.MaxValue)
        {
            var list = database.Books.Where(p => (double)p.Price >= min && (double)p.Price <= max).ToList();
            return View(list);
        }
        public ActionResult Detail(string id)
        {
            var book = database.Books.FirstOrDefault(x => x.BookID == id);
            Session["BookID"] = book.BookID;
            return View(book);
        }
    }
}