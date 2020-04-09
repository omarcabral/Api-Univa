using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PlanEstudiosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PlanEstudios
        public IQueryable<PlanEstudios> GetAll()
        {
            return db.DbPlan;
        }

        // GET: api/PlanEstudios/5
        [ResponseType(typeof(PlanEstudios))]
        public IHttpActionResult GetPlanEstudios(int id)
        {
            PlanEstudios planEstudios = db.DbPlan.Find(id);
            if (planEstudios == null)
            {
                return NotFound();
            }

            return Ok(planEstudios);
        }

        // PUT: api/PlanEstudios/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutPlanEstudios(int id, PlanEstudios planEstudios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != planEstudios.Id)
            {
                return BadRequest();
            }

            db.Entry(planEstudios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanEstudiosExists(id))
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

        // POST: api/PlanEstudios
        [ResponseType(typeof(PlanEstudios))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostPlanEstudios(PlanEstudios planEstudios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (planEstudios.Nombre == null)
            {
                return BadRequest("El nombre no puede ser nulo");
            }
            if (this.db.DbPlan.Where(m => m.Nombre == planEstudios.Nombre).FirstOrDefault() != null)
            {
                return BadRequest("El nombre del plan ya existe");
            }

            db.DbPlan.Add(planEstudios);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = planEstudios.Id }, planEstudios);
        }

        // DELETE: api/PlanEstudios/5
        [ResponseType(typeof(PlanEstudios))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpDelete]
        public IHttpActionResult DeletePlanEstudios(int id)
        {
            PlanEstudios planEstudios = db.DbPlan.Find(id);
            if (planEstudios == null)
            {
                return NotFound();
            }

            db.DbPlan.Remove(planEstudios);
            db.SaveChanges();

            return Ok(planEstudios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlanEstudiosExists(int id)
        {
            return db.DbPlan.Count(e => e.Id == id) > 0;
        }
    }
}