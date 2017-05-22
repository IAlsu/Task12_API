using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Rewarding.Models;
using System.Web;
using MvcSiteMapProvider;
using Rewarding.Filters;

namespace Rewarding.Controllers
{
    [LoggingFilter]
    [Authorize(Roles = "admin, candidateAdmin")]
    public class RewardsController : SessionController
    {
        private PersonContext db = new PersonContext();

        // GET: Rewards
        [Route("awards/{name?}")]
        public ActionResult Index(string name = null)
        {
            var rewards = db.Rewards.Include(i => i.Image)
                        .Where(x => name != null ? x.Title.Contains(name) : true).Select(y => y)
                        .ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_rewardsListPartial", rewards);
            }
            return View(rewards);
        }

        // GET: Rewards/Details/5
        [Route("award/{id:int}")]
        [Route("award/{name}")]
        public ActionResult Details(int? id, string name = null)
        {
            if (id == null && name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reward reward = new Reward();

            if (id != null)
                reward = db.Rewards.Find(id);
            else
                reward = db.Rewards.Where(x => x.Title == name).FirstOrDefault();

            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // GET: Rewards/Create
        [Route("create-award")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rewards/Create
        [Route("create-award")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description")] Reward reward,  HttpPostedFileBase uploaded)
        {
            if (ModelState.IsValid)
            {
                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    reward.Image = new Image();
                    reward.Image.ImageName = uploaded.FileName;
                    reward.Image.ContentType = uploaded.ContentType;
                    reward.Image.Content = new byte[uploaded.ContentLength];
                    uploaded.InputStream.Read(reward.Image.Content, 0, uploaded.ContentLength);
                }
                if (User.IsInRole("candidateAdmin"))
                {
                    GetTempChange().AddItem(reward, "Create");
                }
                else
                {
                    db.Rewards.Add(reward);
                    db.SaveChanges();
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_rewardSinglePartial", reward);
                }
                return RedirectToAction("Index");
            }

            return View(reward);
        }

        // GET: Rewards/Edit/5
        [Route("award/{id:int}/edit")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Rewards/Edit/5
        [Route("award/{id:int}/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description")] Reward reward,  HttpPostedFileBase uploaded)
        {
            if (ModelState.IsValid)
            {
                Reward newReward = db.Rewards.Find(reward.Id);
                newReward.Description = reward.Description;
                newReward.Title = reward.Title;

                if (uploaded != null && uploaded.ContentLength > 0)
                {
                    newReward.Image = new Image();
                    newReward.Image.ImageName = uploaded.FileName;
                    newReward.Image.ContentType = uploaded.ContentType;
                    newReward.Image.Content = new byte[uploaded.ContentLength];
                    uploaded.InputStream.Read(newReward.Image.Content, 0, uploaded.ContentLength);
                }
                if (User.IsInRole("candidateAdmin"))
                {
                    GetTempChange().AddItem(reward, "Edit");
                }
                else
                {
                    db.Entry(newReward).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(reward);
        }

        // GET: Rewards/Delete/5
        [Route("award/{id:int}/delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reward reward = db.Rewards.Find(id);
            if (reward.Persons.Any())
            {
                ViewBag.Message = "You can't delete the reward. It's used in another table";
                return View("DeleteFailure");
            }
            else
            {
                if (reward == null)
                {
                    return HttpNotFound();
                }
                return View(reward);
            }
        }

        // POST: Rewards/Delete/5
        //[Route("award/{id:int}/delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reward reward = db.Rewards.Find(id);
            Image image = db.Pictures.SingleOrDefault(d => d.ImageId == reward.ImageId);

            if (User.IsInRole("candidateAdmin"))
            {
                GetTempChange().AddItem(reward, "Delete");
            }
            else
            {
                db.Rewards.Remove(reward);
                if (image!=null)
                {
                    db.Pictures.Remove(image);
                }
                db.SaveChanges();
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { id });
            }
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
