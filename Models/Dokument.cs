using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkladisteDiplomskihRadovaWeb.Models
{
    public class Dokument
    {
        public string _Id { get; set; }
        public string Rev { get; set; }
        public string Naziv { get; set; }
        public string Putanja { get; set; }
        public string Student { get; set; }
        public string StudentIme { get; set; }
        public string StudentPrezime { get; set; }

        public string Status { get; set; }
        public string Vreme { get; set; }
        public string Mentor { get; set; }
        public string Zadatak { get; set; }
    }
}