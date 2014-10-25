using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmsByPost.Models;

namespace SmsByPost.Controllers
{
    public class PhoneViewController : Controller
    {
        // GET: PhoneView
        public ActionResult Index(string msisdn)
        {
            return View(new PhoneViewModel() {SubscriberMsisdn = msisdn});
        }

        // GET: PhoneView/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PhoneView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhoneView/Create
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

        // GET: PhoneView/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PhoneView/Edit/5
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

        // GET: PhoneView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PhoneView/Delete/5
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
