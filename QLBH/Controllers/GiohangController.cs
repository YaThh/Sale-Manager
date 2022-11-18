using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH.Models;

namespace QLBH.Controllers
{
    public class GiohangController : Controller
    {
        qlbanhangEntities db = new qlbanhangEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }

        public ActionResult AddToCart(string MaSP)
        {
            var giohang = Session["giohang"] as List<CartItem>;
            var sanpham = db.SanPhams.FirstOrDefault(x => x.MaSP == MaSP);
            var item = giohang.FirstOrDefault(x => x.MaSP == MaSP);
            if (item != null)
            {
                item.SoLuong++;
            }
            else
            {
                CartItem newItem = new CartItem();
                newItem.MaSP = MaSP;
                newItem.TenSP = sanpham.TenSP;
                newItem.SoLuong = 1;
                newItem.DonGia = Convert.ToDouble(sanpham.Dongia);
                giohang.Add(newItem);
            }
            Session["giohang"] = giohang;
            return RedirectToAction("Index");
        }

        public ActionResult Update (string MaSP, int SoLuong)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(x => x.MaSP == MaSP);
            if (item != null)
            {
                item.SoLuong = SoLuong;
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public ActionResult DelCartItem (string MaSP, int SoLuong)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(x => x.MaSP == MaSP);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
    }
}