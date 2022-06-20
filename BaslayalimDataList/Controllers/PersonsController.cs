using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaslayalimDataList.Controllers
{
    public class PersonsController : Controller
    {
        // GET: Persons
        public ActionResult Index()
        {
            return View();
        }

        SqlConnection _PublicConnectionStrings = new SqlConnection("server=srvbaslayalim;database=dbbaslayalim;uid=uidbaslayalim;password=passbaslayalim;");

        [HttpPost]
        public JsonResult DurumGetir()
        {
            var model = _PublicConnectionStrings.Query<parametreler>(@"select deger , aciklama from parametreler where tur='kullanicidurumu' and durum=1").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult KullaniciKayitTipi()
        {
            var model = _PublicConnectionStrings.Query<parametreler>(@"select deger , aciklama from parametreler where tur='kullanicikayittipi' and durum=1").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public class parametreler
        {
            public string deger { get; set; }
            public string aciklama { get; set; }
        }


        [HttpPost]
        public JsonResult _KullaniciListesi(KullaniciListesi _KullaniciListesi, FormCollection _FormCollection)
        {
            #region Filtrelemeler
            string _sql = string.Empty;

            if (_KullaniciListesi.ad != "" && _KullaniciListesi.ad != null && _KullaniciListesi.ad != "Tümü") _sql += " and " + "ad like '%" + _KullaniciListesi.ad + "%'";

            if (_KullaniciListesi.soyad != "" && _KullaniciListesi.soyad != null) _sql += " and " + "soyad like '%" + _KullaniciListesi.soyad + "%'";

            if (_KullaniciListesi.eposta != "" && _KullaniciListesi.eposta != null) _sql += " and " + "eposta like '%" + _KullaniciListesi.eposta + "%'";

            if (_KullaniciListesi.sifre != "" && _KullaniciListesi.sifre != null) _sql += " and " + "sifre like '%" + _KullaniciListesi.sifre + "%'";

            if (_KullaniciListesi.tarih.ToString().Substring(5, 4) != "0001" && _KullaniciListesi.tarih.ToString().Substring(5, 4) != ".000" && _KullaniciListesi.tarih != null)
                _sql += " and " + " convert(date, tarih, 104) = convert(date, '" + _KullaniciListesi.tarih + "', 104)";

            if (_KullaniciListesi.kayittipi != null && _KullaniciListesi.kayittipi != "Tümü") _sql += " and " + "kayittipi = '" + _KullaniciListesi.kayittipi + "'";

            if (_KullaniciListesi.durum.ToString() != "0" && _KullaniciListesi.durum.ToString() != null && _KullaniciListesi.durum.ToString() != "Tümü") _sql += " and " + "durum = '" + _KullaniciListesi.durum + "'";
            #endregion
            #region Siralama
            string _sort = "(select null)";
            if (Request.Form.GetValues("order[0][column]") != null)
            {
                var sortColumn = _FormCollection.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = _FormCollection.GetValues("order[0][dir]").FirstOrDefault();
                _sort = sortColumn + " " + sortColumnDir;
            }
            #endregion
            #region Sayfalama

            string Sayfalama = string.Empty;

            if (_FormCollection.GetValues("length").FirstOrDefault() != "-1")
                Sayfalama = "  offset " + _FormCollection.GetValues("start").FirstOrDefault() + " rows fetch next " + _FormCollection.GetValues("length").FirstOrDefault() + " rows only";

            #endregion
            #region Sorgular
            var list = _PublicConnectionStrings.Query<KullaniciListesi>(@"select * from kullanici where durum != 0 " + _sql + " order by " + _sort + Sayfalama).ToList();
            string Total = _PublicConnectionStrings.ExecuteScalar<string>("select count(*) from kullanici where durum != 0");
            #endregion
            #region JsonHazirlaniyor
            return Json(new
            {
                aaData = list,
                draw = _FormCollection.GetValues("draw")[0],
                recordsTotal = Total,
                recordsFiltered = Convert.ToInt32(Total)
            }, JsonRequestBehavior.AllowGet);
            #endregion 
        }
        public class KullaniciListesi
        {
            public int id { get; set; }
            public string ad { get; set; }
            public string soyad { get; set; }
            public string eposta { get; set; }
            public string sifre { get; set; }
            public string kayittipi { get; set; }
            public DateTime tarih { get; set; }
            public string kod { get; set; }
            public int durum { get; set; }
        }



        public class AutoAdArama
        {
            public string id { get; set; }
            public string text { get; set; }
        }
        [HttpPost]
        public JsonResult AdGetir(string ad)
        {
            var model = _PublicConnectionStrings.Query<AutoAdArama>(@"select ad as id ,ad as text from kullanici where (ad like '%" + ad + "%' OR soyad like '%" + ad + "%') and durum != 0").ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}