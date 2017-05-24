using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Rewarding.Models;
using System.Web.Http.OData;

namespace Rewarding.Controllers
{
    public class WebApiRewardsController : ApiController
    {
        private PersonContext db = new PersonContext();

        // GET: api/WebApiRewards
        [EnableQuery]
        public IQueryable<Reward> GetRewards()
        {
            return db.Rewards;
        }

        [EnableQuery]
        public IQueryable<Reward> GetByPieceOfName(string pieceOfName)
        {
            IQueryable<Reward> rewards;
            if (pieceOfName.Length == 1)
            {
                rewards = db.Rewards
                        .Where(x => x.Title.StartsWith(pieceOfName)).Select(y => y);
            }
            else
            {
                rewards = db.Rewards
                       .Where(x => pieceOfName != null ? x.Title.Contains(pieceOfName) : true).Select(y => y);
            }

            return new EnumerableQuery<Reward>(rewards);
        }


        public IHttpActionResult GetByTitle(string title)
        {
            var reward = db.Rewards.Where(x => x.Title == title).FirstOrDefault();
            if (reward == null)
                return NotFound();
            return Ok(reward);
        }
        

        // GET: api/WebApiRewards/5
        [ResponseType(typeof(Reward))]
        public IHttpActionResult GetRewardByID(int id)
        {
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return NotFound();
            }

            return Ok(reward);
        }

        // POST: api/WebApiRewards
        [ResponseType(typeof(Reward))]
        public IHttpActionResult PostReward(Reward reward)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rewards.Add(reward);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reward.Id }, reward);
        }

        //// PUT: api/WebApiRewards/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutReward(int id, Reward reward)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != reward.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(reward).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!RewardExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        public IHttpActionResult Put(int id, Reward reward)
        {
            reward.Id = id;
            if (reward == null)
                throw new ArgumentNullException();
            var storedReward = db.Rewards.FirstOrDefault(p => p.Id == reward.Id);
            if (storedReward == null)
                return NotFound();
            storedReward.Title = reward.Title;
            storedReward.Description = reward.Description;

            return Ok();
        }

        // DELETE: api/WebApiRewards/5
        [ResponseType(typeof(Reward))]
        public IHttpActionResult DeleteReward(int id)
        {
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return NotFound();
            }

            db.Rewards.Remove(reward);
            db.SaveChanges();

            return Ok(reward);
        }



    }
}