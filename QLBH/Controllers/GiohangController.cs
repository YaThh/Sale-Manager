using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH.Models;
using System.Net;
using System.Net.Mail;
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
        public ActionResult DelCartItem (string MaSP)
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
        
        public ActionResult Order (string Email, string Phone)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            string sMsg = "<html><body><table border='1'><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên hàng</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
            int i = 0;
            double tongtien = 0;
            foreach (CartItem item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenSP + "</td>";
                sMsg += "<td>" + item.SoLuong.ToString() + "</td>";
                sMsg += "<td>" + item.DonGia.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}", item.ThanhTien) + "</td>";
                sMsg += "<tr>";
                tongtien += item.ThanhTien;
            }
            sMsg += "<tr><th colspan = '5'>Tổng cộng: "
                 +String.Format("{0:#,### VND}", tongtien) + "</th></tr></table>";
            MailMessage mail = new MailMessage("2051012112thuan@ou.edu.vn", Email, "Thông tin đơn hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("2051012112thuan@ou.edu.vn", "matkhau");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "Home");
        }
    }
}