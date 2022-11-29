using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBooks.Models;

namespace MyBooks.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Voucher
        DB_MYBOOKSEntities database = new DB_MYBOOKSEntities();
        public PartialViewResult VoucherList(string masach, int id)
        {

            var check = database.CartLists.FirstOrDefault(i => i.BookID == masach);
            if (check == null)
            {
                var vList = database.voucher_pro.Where(x => x.IDBook == masach).OrderByDescending(x => x.VOUCHER.MoTa);
                return PartialView(vList);
            } else
            {
                var vList = database.voucher_pro.Where(x => x.IDVoucher == check.IDVoucher).OrderByDescending(x => x.VOUCHER.MoTa);
                return PartialView(vList);
            }

        }
    }
}