using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uyg02.ViewModel;
using uyg02.Models;

namespace uyg02.Controllers
{
    public class ServisController : ApiController
    {
        DB02Entities db = new DB02Entities();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]

        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel() {
            katId=x.katId,
            katAdi=x.katAdi,
            katDersSay=x.Ders.Count()
            
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{katId}")]

        public KategoriModel KategoriById(int katId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.katId == katId).Select(x => new KategoriModel() {
                katId = x.katId,
                katAdi = x.katAdi,
                katDersSay = x.Ders.Count()

            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kategoriekle")]

        public SonucModel KategoriEkle(KategoriModel model)
        {
            if(db.Kategori.Count(s=>s.katAdi==model.katAdi)>0)   
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }

            Kategori yeni = new Kategori();
            yeni.katAdi = model.katAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";

            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == model.katId).FirstOrDefault();
            if (kayit==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.katAdi = model.katAdi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{katId}")]
        public SonucModel KategoriSil(int katId)
        {
            Kategori kayit = db.Kategori.Where(s => s.katId == katId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Ders.Count(s => s.dersKatId == katId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Ders Kaydı Olan Kategori Silinemez!";
                return sonuc;
            }

            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
            #endregion

            #region Ders

        [HttpGet]
        [Route("api/dersliste")]
        public List<DersModel> DersListe()
        {
            List<DersModel> liste = db.Ders.Select(x => new DersModel() {
                dersId=x.dersId,
                dersKodu=x.dersKodu,
                dersAdi=x.dersAdi,
                dersKatId=x.dersKatId,
                dersKatAdi=x.Kategori.katAdi,
                dersSaati=x.dersSaati

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/derslistebykatid/{katId}")]
        public List<DersModel> DersListeByKatId(int katId)
        {
            List<DersModel> liste = db.Ders.Where(s=>s.dersKatId==katId).Select(x => new DersModel()
            {
                dersId = x.dersId,
                dersKodu = x.dersKodu,
                dersAdi = x.dersAdi,
                dersKatId = x.dersKatId,
                dersKatAdi = x.Kategori.katAdi,
                dersSaati = x.dersSaati

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/dersbyid/{dersId}")]
        public DersModel DersById(string dersId)
        {
            DersModel kayit = db.Ders.Where(s => s.dersId == dersId).Select(x=>new DersModel()
            {
                dersId = x.dersId,
                dersKodu = x.dersKodu,
                dersAdi = x.dersAdi,
                dersKatId = x.dersKatId,
                dersKatAdi = x.Kategori.katAdi,
                dersSaati = x.dersSaati
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/dersekle")]
        public SonucModel DersEkle(DersModel model)
        {
            if(db.Ders.Count(s=>s.dersKodu==model.dersKodu)>0)  
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ders Kayıtlıdır!";
                return sonuc;
            }

            Ders yeni = new Ders();
            yeni.dersId = Guid.NewGuid().ToString();
            yeni.dersKodu = model.dersKodu;
            yeni.dersAdi = model.dersAdi;
            yeni.dersKatId = model.dersKatId;
            yeni.dersSaati = model.dersSaati;
            db.Ders.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/dersduzenle")]
        public SonucModel DersDuzenle(DersModel model)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == model.dersId).SingleOrDefault();

            if(kayit==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ders Bulunamadı!";
                return sonuc;
            }

            kayit.dersKodu = model.dersKodu;
            kayit.dersAdi = model.dersAdi;
            kayit.dersKatId = model.dersKatId;
            kayit.dersSaati = model.dersSaati;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Ders Düzenlendi";
            
            return sonuc;
        }

        [HttpDelete]
        [Route("api/derssil/{dersId}")]
        public SonucModel DersSil(string dersId)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == dersId).SingleOrDefault();

            if(kayit==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if(db.Kayit.Count(s=>s.kayitDersId==dersId)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Derse Kayıtlı Öğrenci Olduğu İçin Ders Silinemez!";
                return sonuc;
            }

            db.Ders.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }
        #endregion

        #region Uye

        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyeModel> UyeListe()
        {
            List<UyeModel> liste = db.Uye.Select(x => new UyeModel()
            {
                uyeId=x.uyeId,
                uyeAdsoyad=x.uyeAdsoyad,
                uyeMail=x.uyeMail,
                uyeSifre=x.uyeSifre,
                uyeAdmin=x.uyeAdmin

            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]
        public UyeModel UyeById(string uyeId)
        {
            UyeModel kayit = db.Uye.Where(s=>s.uyeId==uyeId).Select(x => new UyeModel()
            {
                uyeId = x.uyeId,
                uyeAdsoyad = x.uyeAdsoyad,
                uyeMail = x.uyeMail,
                uyeSifre = x.uyeSifre,
                uyeAdmin = x.uyeAdmin

            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyeModel model)
        {
            if(db.Uye.Count(s=>s.uyeMail==model.uyeMail)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Üye Maili Kayıtlıdır!";
                return sonuc;
            }

            Uye yeni = new Uye();
            yeni.uyeId = Guid.NewGuid().ToString();
            yeni.uyeMail = model.uyeMail;
            yeni.uyeAdsoyad = model.uyeAdsoyad;
            yeni.uyeSifre = model.uyeSifre;
            yeni.uyeAdmin = model.uyeAdmin;
            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyeModel model)
        {
            Uye kayit = db.Uye.Where(s => s.uyeId == model.uyeId).SingleOrDefault();
            if(kayit==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            kayit.uyeMail = model.uyeMail;
            kayit.uyeAdsoyad = model.uyeAdsoyad;
            kayit.uyeSifre = model.uyeSifre;
            kayit.uyeAdmin = model.uyeAdmin;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public SonucModel UyeSil(string uyeId)
        {
            Uye kayit = db.Uye.Where(s => s.uyeId == uyeId).SingleOrDefault();
            if(kayit==null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if(db.Kayit.Count(s=>s.kayitUyeId==uyeId)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üye Üzerinde Ders Kaydı Olduğu İçin Üye Silinemez!";
                return sonuc;
            }

            db.Uye.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }

        #endregion

        #region Kayit

        [HttpGet]
        [Route("api/uyedersliste/{uyeId}")]
        public List<KayitModel> UyeDersListe(string uyeId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitUyeId == uyeId).Select(x => new KayitModel()
            {
                kayitId=x.kayitId,
                kayitDersId=x.kayitDersId,
                kayitUyeId=x.kayitUyeId,

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.uyeBilgi = UyeById(kayit.kayitUyeId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);
            }
            return liste;
        }

        [HttpGet]
        [Route("api/dersuyeliste/{dersId}")]
        public List<KayitModel> DersUyeListe(string dersId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitDersId == dersId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitDersId = x.kayitDersId,
                kayitUyeId = x.kayitUyeId,

            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.uyeBilgi = UyeById(kayit.kayitUyeId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);
            }
            return liste;
        }

        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel KayitEkle(KayitModel model)
        {
            if(db.Kayit.Count(s=>s.kayitDersId==model.kayitDersId && s.kayitUyeId==model.kayitUyeId)>0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "İlgili Üye Derse Önceden Kayıtlıdır!";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitUyeId = model.kayitUyeId;
            yeni.kayitDersId = model.kayitDersId;
            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Kaydı Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{kayitId}")]
        public SonucModel KayitSil(string KayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == KayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Kaydı Silindi";
            return sonuc;
        }
        #endregion
    }
}
