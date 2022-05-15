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
    public class BibliotekaController : Controller
    {
        public ActionResult Index()
        {

            List<Dokument> listaNovihDokumenata = new List<Dokument>();

            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var upitDokumenta = new FindRequest().Configure(q =>
                q.SelectorExpression("{\"status\": \"Novo\" }"));

                var doc = db.Queries.FindAsync<Dokument>(upitDokumenta).Result;

                if (doc.DocCount > 0)
                {
                    foreach (var dok in doc.Docs)
                    {
                        listaNovihDokumenata.Add(dok);
                    }
                    TempData["brojNovihDokumenata"] = doc.DocCount;
                    ViewBag.novaDokumenta = listaNovihDokumenata;
                }
                else
                    TempData["brojNovihDokumenata"] = 0;

            }

            return View();
        }

        public ActionResult Arhiva()
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
                    
                    ViewBag.obradjenaDokumenta = listaObradjenihDokumenata;
                }
            }

            return View();
        }

        public ActionResult Odjava()
        {
            TempData["biblioteka"] = "";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public ActionResult Pretraga(string pretraga)
        {
            List<Dokument> listaRadova = new List<Dokument>();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var kljuc = pretraga;
                QueryViewRequest upit;

                if (pretraga != "")
                    upit = new QueryViewRequest("Radovi", "Student").Configure(x => x.Key(kljuc));
                else
                    upit = new QueryViewRequest("Radovi", "Student");
               
                var radovi = db.Views.QueryAsync<Dokument>(upit).Result;

                if (!radovi.IsEmpty)
                {
                    foreach (var s in radovi.Rows)
                    {
                        var rad = new Dokument();
                        rad = s.Value;
                        listaRadova.Add(rad);
                    }
                }
            }
            TempData["radoviPretraga"] = listaRadova;
            TempData["pretraga"] = pretraga;
            return RedirectToAction("Index","Biblioteka");
        }

        public ActionResult PretragaRadovi(string pretraga)
        {
            List<Dokument> listaRadova = new List<Dokument>();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
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
                        listaRadova.Add(rad);
                    }
                }
            }
            TempData["radoviPretraga"] = listaRadova;
            TempData["pretraga"] = pretraga;
            return RedirectToAction("Arhiva", "Biblioteka");
        }


    }
}
