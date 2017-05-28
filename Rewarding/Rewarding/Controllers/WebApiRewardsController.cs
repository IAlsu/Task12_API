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
using System.Collections;

namespace Rewarding.Controllers
{
    public class WebApiRewardsController : ApiController
    {
        private PersonContext db = new PersonContext();

        [EnableQuery]
        [Route("api/awards/{pieceOfName?}")]
        public IQueryable<Reward> GetByPieceOfName(string pieceOfName=null)
        //public IEnumerable<Reward> Get(string pieceOfName=null)
        {
            IQueryable<Reward> rewards = db.Rewards;

           if (pieceOfName!=null)
            {
                if (pieceOfName.Length == 1)
                {
                    rewards = rewards
                            .Where(x => x.Title.StartsWith(pieceOfName)).Select(y => y);
                }
                else
                {
                    rewards = rewards
                            .Where(x => pieceOfName != null ? x.Title.Contains(pieceOfName) : true).Select(y => y);
                }
            }
            IEnumerable<Reward> rew = rewards.ToList();
            return new EnumerableQuery<Reward>(rew);
        }

        [Route("api/award/{title}")]
        public IHttpActionResult GetByTitle(string title)
        {
            var reward = db.Rewards.Where(x => x.Title == title).FirstOrDefault();
            if (reward == null)
                return NotFound();
            return Ok(reward);
        }

        // GET: api/award/5
        [Route("api/award/{id:int}")]
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

        // POST: api/award
        [Route("api/award")]
        public HttpResponseMessage Post(Reward reward)
        {
            db.Rewards.Add(reward);
            db.SaveChanges();

            var response = Request.CreateResponse<Reward>(HttpStatusCode.Created, reward);
            var uri = Url.Link("DefaultApiReward", new { Id = reward.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // PUT: api/award/5
        [Route("api/award/{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReward(int id, Reward reward)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reward.Id)
            {
                return BadRequest();
            }

            db.Entry(reward).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.Rewards.Find(id)==null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/award/5
        [Route("api/award/{id:int}")]
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