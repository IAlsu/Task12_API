using Rewarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

using System.Web.Http.OData;
using System.Data.Entity.Core.Objects;
using System.Xml.Linq;

namespace Rewarding.Controllers
{
    public class WebApiPersonController : ApiController
    {
        PersonContext db = new PersonContext();

        [EnableQuery]
        public IQueryable<Person> GetByPieceOfName(string pieceOfName)
        {
            IQueryable<Person> persons;
            if (pieceOfName.Length == 1)
            {
                persons = db.Persons
                        .Where(x => x.Name.StartsWith(pieceOfName)).Select(y => y);
            }
            else
            {
                persons = db.Persons
                        .Where(x => x.Name.StartsWith(pieceOfName) || x.Name.EndsWith(pieceOfName)).Select(y => y); //distinct
            }

            return new EnumerableQuery<Person>(persons);
        }


        public IHttpActionResult GetByName(string name)
        {
            var person = db.Persons.Where(x => x.Name == name).OrderBy(y => y.Birthdate).FirstOrDefault(); 
            if (person == null)
                return NotFound();
            return Ok(person);
        }


        public IHttpActionResult GetById(int id)
        {
            var person = db.Persons.Find(id);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        public HttpResponseMessage Post(Person person)
        {
            db.Persons.Add(person);
            db.SaveChanges();
            var response = Request.CreateResponse<Person>(HttpStatusCode.Created, person);
            var uri = Url.Link("DefaultApi", new { Id = person.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public IHttpActionResult Put(int id, Person person)
        {
            person.Id = id;
            if (person == null)
                throw new ArgumentNullException();
            var storedPerson = db.Persons.FirstOrDefault(p => p.Id == person.Id);
            if (storedPerson == null)
                return NotFound();
            storedPerson.Name = person.Name;
            storedPerson.Age = person.Age;
            storedPerson.Birthdate = person.Birthdate;
            return Ok(); 
        }
        public void Delete(int id)
        {
            Person person = db.Persons.SingleOrDefault(s => s.Id == id);
            if (person == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Image photo = db.Pictures.SingleOrDefault(d => d.ImageId == person.PhotoId);
            db.Persons.Remove(person);
            if (photo != null)
                db.Pictures.Remove(photo);
            db.SaveChanges();
        }

        public IHttpActionResult Post(int personId, int rewardId)
        {
            var person = db.Persons.Find(personId);
            var reward = db.Rewards.Find(rewardId);
            if (person==null || reward==null)
                return NotFound();

            if (person.Rewards.Contains(reward))
                return BadRequest();

            person.Rewards.Add(reward);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = personId }, reward);
            //var response = Request.CreateResponse<Person>(HttpStatusCode.Created, person);
            //var uri = Url.Link("DefaultApi", new { Id = person.Id });
            //response.Headers.Location = new Uri(uri);
            //return response;
        }

    }
}
