using MyCouch;
using MyCouch.Requests;
using SkladisteDiplomskihRadovaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkladisteDiplomskihRadovaWeb.Controllers
{
    public class DokumentController : Controller
    {
        // GET: Dokument
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Izmeni(string id)
        {
            // pronaci dokument na osnovu id-a
            // pronaci studenta na osnovu broja indeksa 
            Dokument dokument = new Dokument();
            Student student = new Student();

            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {

                var doc = db.Entities.GetAsync<Dokument>(id).Result;


                if (!doc.IsEmpty)
                {
                    dokument = doc.Content;
                }

                var upitStudent = new FindRequest().Configure(q =>
                q.SelectorExpression("{\"brojIndeksa\": \"" + dokument.Student + "\" }"));

                var st = db.Queries.FindAsync<Student>(upitStudent).Result;

                if (st.DocCount > 0)
                {
                    student = st.Docs[0];
                }

            }

            ViewBag.student = student;
            ViewBag.dokument = dokument;

            return View();
        }

        // Izmeni dokument na osnovu podataka
        // Izmeni status dokumenta na Obradjeno

        //var izmena = db.Documents.PutAsync(id, rev, "{\"mentor\":\""+mentor+"\", \"status\":\"Obradjeno\"}").Result;
        //var izmena = db.Entities.PutAsync<Dokument>(dokument);
        //"1-65a95a372f7a6ef12eb30e9daba1d388", "{\"ime\":\"Izmenjeno\"}").Result;

        [HttpPost]
        public ActionResult Obradi(string id, string rev, string vreme, string putanja, string ime, 
            string prezime, string brojIndeksa, string email, string smer, string modul, string tema, string zadatak, string mentor)
        {
            
            Dokument dokument = new Dokument
            {
                _Id = id,
                Rev = rev,
                Naziv = tema,
                Mentor = mentor,
                Status = "Obradjeno",
                Putanja = putanja,
                Vreme = vreme,
                Zadatak = zadatak,
                StudentIme = ime,
                StudentPrezime = prezime,
                Student = brojIndeksa

            };
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var izmena = db.Entities.PutAsync<Dokument>(dokument).Result;
            }
            return RedirectToAction("Index", "Biblioteka");
        }

        // pronaci dokument na osnovu id-a
        // pronaci studenta na osnovu broja indeksa 
        public ActionResult Obrisi(string id)
        {
            Dokument dokument = new Dokument();
            Student student = new Student();

            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {

                var doc = db.Entities.GetAsync<Dokument>(id).Result;


                if (!doc.IsEmpty)
                {
                    dokument = doc.Content;
                }

                var upitStudent = new FindRequest().Configure(q =>
                q.SelectorExpression("{\"brojIndeksa\": \"" + dokument.Student + "\" }"));

                var st = db.Queries.FindAsync<Student>(upitStudent).Result;

                if (st.DocCount > 0)
                {
                    student = st.Docs[0];
                }

            }

            ViewBag.student = student;
            ViewBag.dokument = dokument;

            return View();
        }

        [HttpPost]
        public ActionResult ObrisiDokument(string id, string rev)
        {
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var obrisan = db.Documents.DeleteAsync(id, rev).Result;

                if (obrisan.IsSuccess)
                    TempData["obrisanDokument"] = "Dokument je uspešno obrisan!";
                else
                    TempData["obrisanDokument"] = "Dokument nije obrisan!";
            }
             
            return RedirectToAction("Index", "Biblioteka");
        }
    }
}