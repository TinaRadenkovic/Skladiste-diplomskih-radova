﻿using MyCouch;
using SkladisteDiplomskihRadovaWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkladisteDiplomskihRadovaWeb.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Odjava()
        {
            TempData["student"] = "";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public ActionResult Predaja(HttpPostedFileBase fajl, string naziv, string student)
        {
            string path = Server.MapPath("~/Resources/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            fajl.SaveAs(path + Path.GetFileName(fajl.FileName));

            Student st = new Student();
            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var br = db.Entities.GetAsync<Student>(student).Result;
                st.BrojIndeksa = br.Content.BrojIndeksa;
            }

            Dokument dok = new Dokument
            {
                Putanja = fajl.FileName,
                Naziv = naziv,
                Student = st.BrojIndeksa,
                Vreme = DateTime.Now.ToString(),
                Status = "Novo"
            };

            using (var db = new MyCouchClient("http://admin:admin@localhost:5984", "skladiste_diplomskih_radova"))
            {
                var odg = db.Entities.PostAsync<Dokument>(dok).Result;
            }

            TempData["postavljenDokument"] = "Dokument je uspešno postavljen!";
            TempData["student"] = student;
            return RedirectToAction(nameof(StudentController.Index), "Student"); ;
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
