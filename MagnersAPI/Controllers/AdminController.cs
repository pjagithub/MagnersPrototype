using Magners.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Magners.Controllers
{
    public class AdminController : Controller
    {
        MagnersContentEntities context = new MagnersContentEntities();

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            IQueryable<Image> images = context.Images;
            return View("List", images);
        }

        //
        // GET: /Admin/List
        public ActionResult List()
        {
            //context.Images.Add(new Image
            //{
            //    FileName = "Test.jpg",
            //    Tags = "TEST,TAGS"
            //});
            //context.SaveChanges();

            IQueryable<Image> images = context.Images;
            return View(images);
        }

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /Admin/Create
        // Called from List page when 'Create New' is selected
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        // Called from Create page when 'Create' is selected
        [HttpPost]
        public ActionResult Create(string tags, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    // save filename and tags to the database
                    Image image = new Image
                    {
                        FileName = file.FileName,
                        Tags = tags,
                        ImageType = "H"
                    };
                    context.Images.Add(image);
                    context.SaveChanges();

                    // upload the file
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        file.SaveAs(path);
                    }

                    return RedirectToAction("List");
                }
                else
                {
                    return View("Create");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: /Admin/Edit/5
        // Called from List page when 'Edit' is selected
        public ActionResult Edit(int id)
        {
            ViewBag.ImageID = id;

            Image image = context.Images
                .Where(i => i.Id == id)
                .FirstOrDefault();

            return View(image);
        }

        //
        // POST: /Admin/Edit/5
        // Called from Edit page when 'Save' is selected
        [HttpPost]
        public ActionResult Edit(Image image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    context.Images.Attach(image);
                    context.Entry(image).State = System.Data.EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    return View("Edit");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: /Admin/Delete/5
        // Called from List page when 'Delete' is selected
        public ActionResult Delete(int id)
        {
            Image image = context.Images
                .Where(i => i.Id == id)
                .FirstOrDefault();

            return View(image);
        }

        // POST: /Admin/Delete/5
        // Called from Delete page when 'Delete' is selected
        [HttpPost]
        public ActionResult Delete(Image image)
        {
            try
            {
                context.Images.Attach(image);
                context.Images.Remove(image);
                context.SaveChanges();

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                throw ex;
                return View();
            }
        }
    }
}
