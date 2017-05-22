using Rewarding.Controllers;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
//using GameStore.Domain.Entities;
//using GameStore.Domain.Abstract;
//using GameStore.WebUI.Models;

namespace Rewarding.Models
{
    public class TempChangesController : SessionController
    {
        public RedirectToRouteResult ShowChanges(string returnUrl)
        {
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new TempChangesVM
            {
                TempChange = GetTempChange(),
                ReturnUrl = returnUrl
            });
        }

        private PersonContext db = new PersonContext();
        public RedirectToRouteResult ApplyChanges()
        {
            var data = GetTempChange().changesCollection;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in data)
                    {
                        switch (item.Method)
                        {
                            case "Create":
                                CreateTable(item.Entity);
                                break;
                            case "Edit":
                                EditTable(item.Entity);
                                break;
                            case "Delete":
                                DeleteTable(item.Entity);
                                break;
                        }
                    }
                    db.SaveChanges();
                    RemoveFromTempChanges("");
                    transaction.Commit();
                }
                catch (System.Exception e )
                {
                    transaction.Rollback();
                    ViewBag.ResultMessage = "Error occured, records rolledback.";
                }
                
            }
            return RedirectToAction("Index","Person");
        }

        private void CreateTable(IEntity table)
        {
            if (table.GetType().Name == "Reward")
            {
                Reward reward = (Reward)(table);
                db.Rewards.Add(reward);
            }
            else
            {
                Person person = (Person)(table);
                db.Persons.Add(person);
            }
        }

        private void DeleteTable(IEntity table)
        {
            if (table.GetType().Name.Contains("Reward"))
            {
                Reward reward = db.Rewards.Find(((Reward)(table)).Id);
                Image image = db.Pictures.SingleOrDefault(d => d.ImageId == reward.ImageId);
                db.Rewards.Remove(reward);
                if (image != null)
                {
                    db.Pictures.Remove(image);
                }
            }
            else
            {
                Person person = db.Persons.Find(((Person)(table)).Id);
                Image image = db.Pictures.SingleOrDefault(d => d.ImageId == person.PhotoId);
                db.Persons.Remove(person);
                if (image != null)
                {
                    db.Pictures.Remove(image);
                }
            }
        }
        private void EditTable(IEntity table)
        {
            if (table.GetType().Name == "Reward")
            {
                var newReward = (Reward)(table);
                Reward reward = db.Rewards.Find((newReward).Id);
                db.Entry(reward).CurrentValues.SetValues(newReward);
            }
            else
            {
                var newPerson = (Person)(table);
                Person person = db.Persons.Find((newPerson).Id);
                db.Entry(person).CurrentValues.SetValues(newPerson);
            }
        }

        public RedirectToRouteResult DoNotApplyChanges(string returnUrl)
        {
            RemoveFromTempChanges(returnUrl);
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}