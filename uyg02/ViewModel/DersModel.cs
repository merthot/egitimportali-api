using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uyg02.ViewModel
{
    public class DersModel
    {
        public string dersId { get; set; }
        public string dersKodu { get; set; }
        public string dersAdi { get; set; }
        public int dersKatId { get; set; }
        public string dersKatAdi { get; set; }
        public int dersSaati { get; set; }
    }
}