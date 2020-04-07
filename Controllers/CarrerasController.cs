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
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class CarrerasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Carreras
        public IQueryable<Carrera> GetAll()
        {
            return db.DbCarreras.Include(m=>m.IdPlan);
        }

        [HttpGet]
        [ActionName("ByPlan")]
        public IQueryable<Carrera> ByPlan(int idPlan)
        {
            return this.db.DbCarreras.Where(m => m.IdPlan.Id == idPlan);
        }

        // GET: api/Carreras/5
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult GetCarrera(int id)
        {
            Carrera carrera = db.DbCarreras.Where(m=>m.Id==id).Include(m=>m.IdPlan).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }

            return Ok(carrera);
        }

        // PUT: api/Carreras/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCarrera(int id, Carrera carrera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carrera.Id)
            {
                return BadRequest();
            }

            db.Entry(carrera).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
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

        // POST: api/Carreras
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult PostCarrera(Carrera carrera)
        {
            
            carrera.IdPlan = db.DbPlan.Find(carrera.IdPlan.Id);
            if (carrera.IdPlan == null)
            {
                return BadRequest("El plan enviado no existe");
            }
            var prueba = this.db.DbCarreras.Where(m => m.IdPlan.Id == carrera.IdPlan.Id && m.Nombre == carrera.Nombre).FirstOrDefault();
            if (prueba != null)
            {
                return BadRequest("La carrera ya existe en ese plan de estudios");
            }
            db.DbCarreras.Add(carrera);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = carrera.Id }, carrera);
        }
        

        // DELETE: api/Carreras/5
        [ResponseType(typeof(Carrera))]
        public IHttpActionResult DeleteCarrera(int id)
        {
            Carrera carrera = db.DbCarreras.Find(id);
            if (carrera == null)
            {
                return NotFound();
            }

            db.DbCarreras.Remove(carrera);
            db.SaveChanges();

            return Ok(carrera);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarreraExists(int id)
        {
            return db.DbCarreras.Count(e => e.Id == id) > 0;
        }
    }
}