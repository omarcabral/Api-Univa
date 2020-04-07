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
    public class AlumnoCarrerasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AlumnoCarreras
        public IQueryable<AlumnoCarrera> GetDbAlumnoCarrera()
        {
            return db.DbAlumnoCarrera;
        }

        // GET: api/AlumnoCarreras/5
        [ResponseType(typeof(AlumnoCarrera))]
        public IHttpActionResult GetAlumnoCarrera(int id)
        {
            AlumnoCarrera alumnoCarrera = db.DbAlumnoCarrera.Find(id);
            if (alumnoCarrera == null)
            {
                return NotFound();
            }

            return Ok(alumnoCarrera);
        }

        // PUT: api/AlumnoCarreras/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlumnoCarrera(int id, AlumnoCarrera alumnoCarrera)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != alumnoCarrera.Id)
            {
                return BadRequest();
            }

            db.Entry(alumnoCarrera).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoCarreraExists(id))
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

        // POST: api/AlumnoCarreras
        [ResponseType(typeof(AlumnoCarrera))]
        public IHttpActionResult PostAlumnoCarrera(AlumnoCarrera alumnoCarrera)
        {
            
            alumnoCarrera.IdAlumno = this.db.DbAlumnos.Find(alumnoCarrera.IdAlumno.Id);
            alumnoCarrera.IdCarrera = this.db.DbCarreras.Find(alumnoCarrera.IdCarrera.Id);
            if (alumnoCarrera.IdAlumno==null|| alumnoCarrera.IdCarrera == null)
            {
                BadRequest("Verifique los datos ");
            }
            db.DbAlumnoCarrera.Add(alumnoCarrera);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = alumnoCarrera.Id }, alumnoCarrera);
        }

        // DELETE: api/AlumnoCarreras/5
        [ResponseType(typeof(AlumnoCarrera))]
        public IHttpActionResult DeleteAlumnoCarrera(int idAlumno, int idCarrera)
        {
            AlumnoCarrera alumnoCarrera = db.DbAlumnoCarrera.Where(m=>m.IdAlumno.Id==idAlumno && m.IdCarrera.Id==idCarrera).FirstOrDefault();
            if (alumnoCarrera == null)
            {
                return NotFound();
            }

            db.DbAlumnoCarrera.Remove(alumnoCarrera);
            db.SaveChanges();

            return Ok(alumnoCarrera);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlumnoCarreraExists(int id)
        {
            return db.DbAlumnoCarrera.Count(e => e.Id == id) > 0;
        }
    }
}