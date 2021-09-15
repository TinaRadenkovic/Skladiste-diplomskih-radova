using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkladisteDiplomskihRadovaWeb.Models
{
    public class StudentskaSluzba
    {
        public string BibliotekaId { get; set; }
        public string BibliotekaRev { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Sifra { get; set; }
    }
}