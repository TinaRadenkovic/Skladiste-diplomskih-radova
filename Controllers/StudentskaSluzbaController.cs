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
    public class StudentskaSluzbaController : Controller
    {
        public ActionResult Index()
        {
            // Pronalazi dokumenta sa statusom Obradjeno

            List<Dokument> listaObradjenihDokumenata = new List<Dokument>();

            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var upitDokumenta = new FindRequest().Configure(q =>
                q.SelectorExpression("{\"status\": \"Obradjeno\" }"));

                var doc = db.Queries.FindAsync<Dokument>(upitDokumenta).Result;

                if (doc.DocCount > 0)
                {
                    foreach (var dok in doc.Docs)
                    {
                        listaObradjenihDokumenata.Add(dok);
                    }

                    ViewBag.dokumenta = listaObradjenihDokumenata;
                }
            }

            return View();
        }

        public ActionResult Studenti()
        {
            List<Student> listaStudenata = new List<Student>();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var studenti = db.Views.QueryAsync<Student>(new QueryViewRequest("Studenti", "BrojIndeksa")).Result;

                if (!studenti.IsEmpty)
                {
                    foreach(var s in studenti.Rows)
                    {
                        var student = new Student();
                        student = s.Value;
                        listaStudenata.Add(student);
                    }
                }
            }

            ViewBag.studenti = listaStudenata;
            return View();
        }

        public ActionResult PretragaStudenti(string pretraga)
        {
            List<Student> listaStudenata = new List<Student>();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                // Naziv, student, vreme
                var kljuc = pretraga;
                QueryViewRequest upit;

                if (pretraga != "")
                    upit = new QueryViewRequest("Studenti", "BrojIndeksa").Configure(x => x.Key(kljuc));
                else
                    upit = new QueryViewRequest("Studenti", "BrojIndeksa");


                var studenti = db.Views.QueryAsync<Student>(upit).Result;

                if (!studenti.IsEmpty)
                {
                    foreach (var s in studenti.Rows)
                    {
                        var student = new Student();
                        student = s.Value;
                        listaStudenata.Add(student);
                    }
                }
            }
            TempData["studentiPretraga"] = listaStudenata;
            TempData["pretraga"] = pretraga;
            return RedirectToAction("Studenti", "StudentskaSluzba");
        }

        public ActionResult PretragaRadovi(string pretraga)
        {
            List<Dokument> listaStudenata = new List<Dokument>();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                // Naziv, student, vreme
                var kljuc = pretraga;
                QueryViewRequest upit;

                if (pretraga != "")
                    upit = new QueryViewRequest("Radovi", "NazivRada").Configure(x => x.Key(kljuc));
                else
                    upit = new QueryViewRequest("Radovi", "NazivRada");


                var radovi = db.Views.QueryAsync<Dokument>(upit).Result;

                if (!radovi.IsEmpty)
                {
                    foreach (var s in radovi.Rows)
                    {
                        var rad = new Dokument();
                        rad = s.Value;
                        listaStudenata.Add(rad);
                    }
                }
            }
            TempData["radoviPretraga"] = listaStudenata;
            TempData["pretraga"] = pretraga;
            return RedirectToAction("Index", "StudentskaSluzba");
        }

        public ActionResult Odjava()
        {
            TempData["sluzba"] = "";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
