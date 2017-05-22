using System.Linq;
using System.Web.Mvc;
using Rewarding.Models;
using System.Net;
using System.Data.Entity;
using System.Web;
using System;
using Rewarding.Filters;
using System.Web.Caching;
using System.Web.UI;

namespace Rewarding.Controllers
{
    [ExceptionFilter]
    [LoggingFilter]
    [Authorize(Roles = "admin, candidateAdmin")]
    public class PersonController : SessionController
    {
        PersonContext db = new PersonContext();

        // GET: Person
        [OverrideAuthorization, Authorize(Roles = "user")]
        [AllowAnonymous]
        [OutputCache(Duration = 30, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult Index(string name = null)
        {
            var persons = db.Persons.Include(s => s.Photo).Include(i => i.Rewards)
                        .Where(x => name != null ? x.Name.Contains(name) : true).Select(y => y);

            ViewBag.Rewards = db.Rewards.ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_peopleListPartial", persons);
            }
            //to show how ExceptionFilter works
            //throw new Exception();
            return View(persons);   
        }

        //GET
        public ActionResult Create()
        {
            ViewBag.Rewards = db.Rewards.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Birthdate,Age,Rewards")] Person person, int[] selectedRewards, HttpPostedFileBase uploaded)
        {
            ViewBag.Rewards = db.Rewards.ToList();
            if (ModelState.IsValid)
            {
                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    person.Photo = new Image();
                    person.Photo.ImageName = uploaded.FileName;
                    person.Photo.ContentType = uploaded.ContentType;
                    person.Photo.Content = new byte[uploaded.ContentLength];
                    uploaded.InputStream.Read(person.Photo.Content, 0, uploaded.ContentLength);
                }
                if (User.IsInRole("candidateAdmin"))
                {
                    GetTempChange().AddItem(person, "Create");
                }
                else
                {
                    if (selectedRewards != null)
                    {
                        //получаем выбранные rewards
                        foreach (var c in db.Rewards.Where(co => selectedRewards.Contains(co.Id)))
                        {
                            person.Rewards.Add(c);
                        }
                    }
                    db.Persons.Add(person);
                    db.SaveChanges();
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_personSinglePartial", person);
                }
                return RedirectToAction("Index");
            }

            return View(person); //не должно вызываться
        }

        // GET: /Persons/Details/1 
        public ActionResult Details(int? id, string name = null)
        {
            if (id == null && name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = new Person();

            if (id != null)
                person = db.Persons.Find(id);
            else
                person = db.Persons.Where(x => x.Name == name).OrderBy(y => y.Birthdate).FirstOrDefault();
            

            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: /Persons/Edit/1 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rewards = db.Rewards.ToList();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Birthdate,Age")] Person person, int[] selectedRewards, HttpPostedFileBase uploaded)
        {
            if (ModelState.IsValid)
            {
                Person newPerson = db.Persons.Find(person.Id);
                newPerson.Name = person.Name;
                newPerson.Birthdate = person.Birthdate;
                newPerson.Age = person.Age;

                newPerson.Rewards.Clear();
                if (selectedRewards != null)
                {
                    //получаем выбранные rewards
                    foreach (var c in db.Rewards.Where(co => selectedRewards.Contains(co.Id)))
                    {
                        newPerson.Rewards.Add(c);
                    }
                }

                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    newPerson.Photo = new Image();
                    newPerson.Photo.ImageName = uploaded.FileName;
                    newPerson.Photo.ContentType = uploaded.ContentType;
                    newPerson.Photo.Content = new byte[uploaded.ContentLength];
                    uploaded.InputStream.Read(newPerson.Photo.Content, 0, uploaded.ContentLength);
                }
                if (User.IsInRole("candidateAdmin"))
                {
                    GetTempChange().AddItem(person, "Edit");
                }
                else
                {
                    db.Entry(newPerson).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.Rewards = db.Rewards.ToList();
            return View(person);
        }

        //GET: /Persons/Delete/1
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Person person = db.Persons.Include(s => s.Photo).SingleOrDefault(s => s.Id == id);
            Image photo = db.Pictures.SingleOrDefault(d => d.ImageId == person.PhotoId);
            if (User.IsInRole("candidateAdmin"))
            {
                GetTempChange().AddItem(person, "Delete");
            }
            else
            {
                db.Persons.Remove(person);
                if (photo !=null)
                    db.Pictures.Remove(photo);
                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { id });
            }
            return RedirectToAction("Index");
        }

        public FileResult DownloadListOfPeople()
        {
            var list = db.Persons.Include(i => i.Rewards).ToList();

            var listWithRewards = list.Select(i => i.Name + ": " + Environment.NewLine + "   - " + 
                ((i.Rewards.Count>0)?(string.Join(Environment.NewLine + "   - ", i.Rewards.Select(u => u.Title).ToArray()  )) : "No rewards")).ToArray();

            string people = string.Join(Environment.NewLine, listWithRewards);

            return File(new System.Text.UTF8Encoding().GetBytes(people), "text/pcdslain", "People.txt");
            
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}