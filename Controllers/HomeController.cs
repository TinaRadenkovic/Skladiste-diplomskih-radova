using MyCouch;
using MyCouch.HttpRequestFactories;
using MyCouch.Requests;
using MyCouch.Responses;
using SkladisteDiplomskihRadovaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkladisteDiplomskihRadovaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexNovi()
        {
            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                // Upisivanje u bazu

                //var artistJson = "{\"ime\":\"Ama\", \"prezime\":\"Amic\"}";
                //var response = db.Documents.PostAsync(artistJson).Result;

                // Pribavljanje iz baze

                //var procitano = db.Documents.GetAsync("d36e42a1c9329584fb0df3e48e008434").Result;

                //var izmena = db.Documents.PutAsync("d36e42a1c9329584fb0df3e48e008434", "1-65a95a372f7a6ef12eb30e9daba1d388", "{\"ime\":\"Izmenjeno\"}").Result;

                var brisanje = db.Documents.DeleteAsync("d36e42a1c9329584fb0df3e48e00a40d", "2-ea5adf1dc18cb7b99d988dd217ac6130").Result;

                /*
                 
                 var artist = new Artist
{
    ArtistId = "5",
    Name = "Foo bar",
    Albums = new[]
    {
        new Album { Name = "Hello world!", Tracks = 9 }
    }
};

var response = db.Entities.Post(artist);

Console.Write(response.GenerateToStringDebugVersion());
                 */



                // var student = db.Views.QueryAsync(new QueryViewRequest("prijava", "new-view")).Result;

                //var query = new QueryViewRequest("prijava", "student").Configure(c => c.Equals(s));


                //var upitEmailISifra = new Query("prijava", "student");

                //var tStudent = db.Views.QueryAsync<Student>(query).Result;
                // var tStudent = db.Views.QueryAsync(new QueryViewRequest("prijava", "student").Configure(c => c.Em("someDocId")).Result;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Prijava(String email, String sifra)
        {
            Student s = new Student
            {
                Email = email,
                Sifra = sifra
            };
            Biblioteka b = new Biblioteka();
            StudentskaSluzba ss = new StudentskaSluzba();

            List<Student> studenti = new List<Student>();
            List<Biblioteka> bibliotekari = new List<Biblioteka>();
            List<StudentskaSluzba> sluzbenici = new List<StudentskaSluzba>();

            if (email == "")
            {
                TempData["neispravnoLogovanje"] = "Niste uneli email adresu!";
                return RedirectToAction(nameof(HomeController.Index), "Index");
            }

            if (sifra == "")
            {
                TempData["neispravnoLogovanje"] = "Niste uneli lozinku!";
                return RedirectToAction(nameof(HomeController.Index), "Index");
            }




            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {

                var request = new FindRequest().Configure(q =>
                q.SelectorExpression("{\"email\": \"" + email + "\", \"sifra\": \"" + sifra + "\" }"));

                var doc = db.Queries.FindAsync<Korisnik>(request).Result;


                if (doc.DocCount > 0)
                {
                    if (doc.Docs[0].Tip == "student")
                    {
                        TempData["student"] = doc.Docs[0]._Id;
                        return RedirectToAction(nameof(StudentController.Index), "Student");
                    }
                    else if (doc.Docs[0].Tip == "biblioteka")
                    {
                        TempData["biblioteka"] = doc.Docs[0]._Id;
                        return RedirectToAction(nameof(BibliotekaController.Index), "Biblioteka");
                    }
                    else if (doc.Docs[0].Tip == "sluzba")
                    {
                        TempData["sluzba"] = doc.Docs[0]._Id;
                        return RedirectToAction(nameof(StudentskaSluzbaController.Index), "StudentskaSluzba");
                    } 
                    else return RedirectToAction(nameof(HomeController.Index), "Index");

                }
                return RedirectToAction(nameof(HomeController.Index), "Index");

            }
            

        }
    }
    }