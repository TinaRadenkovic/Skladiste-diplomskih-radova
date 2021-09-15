using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkladisteDiplomskihRadovaWeb.Models
{
    public class Student
    {
        public string _Id { get; set; }
        public string _Rev { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojIndeksa { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Sifra { get; set; }
        public string Modul { get; set; }
        public string Smer { get; set; }
        public string Tip { get; set; }
        public Dokument Dokument { get; set; }
    }
}