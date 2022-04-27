using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uyg02.ViewModel
{
    public class KayitModel
    {
        public string kayitId { get; set; }
        public string kayitDersId { get; set; }
        public string kayitUyeId { get; set; }
        public UyeModel uyeBilgi { get; set; }
        public DersModel dersBilgi { get; set; }
    }
}