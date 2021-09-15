using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkladisteDiplomskihRadovaWeb.Models
{
    public class Korisnik
    {
        public string _Id { get; set; }
        public string Rev { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Sifra { get; set; }
        public string Tip { get; set; }
    }
}