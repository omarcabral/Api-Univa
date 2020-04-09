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
    public class CalificacionesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Calificaciones
        public IQueryable<Calificaciones> GetDbCalificaciones()
        {
            return db.DbCalificaciones;
        }

        // GET: api/Calificaciones/5
        [ResponseType(typeof(Calificaciones))]
        public IHttpActionResult GetCalificaciones(int id)
        {
            Calificaciones calificaciones = db.DbCalificaciones.Find(id);
            if (calificaciones == null)
            {
                return NotFound();
            }

            return Ok(calificaciones);
        }

        public IQueryable <Calificaciones> getCalificacionesByGrupo(int id)
        {
            return db.DbCalificaciones.Where(m=>m.IdGrupo.Id==id).Include(m=>m.IdAlumnos).Include(m=>m.IdGrupo);
        }

        // PUT: api/Calificaciones/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutCalificaciones(int id, Calificaciones calificaciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != calificaciones.Id)
            {
                return BadRequest();
            }
            db.DbCalificaciones.Attach(calificaciones);
            db.Entry(calificaciones).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalificacionesExists(id))
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

        // POST: api/Calificaciones
        [ResponseType(typeof(Calificaciones))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostCalificaciones(Calificaciones calificaciones)
        {
           
            calificaciones.IdAlumnos = db.DbAlumnos.Find(calificaciones.IdAlumnos.Id);
            if (calificaciones.IdAlumnos == null)
                return BadRequest("El alumno no fue encontrado");
            calificaciones.IdGrupo = db.DbGrupo.Find(calificaciones.IdGrupo.Id);
            if (calificaciones.IdGrupo == null)
                return BadRequest("El grupo no fue encontrado");
            db.DbCalificaciones.Add(calificaciones);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = calificaciones.Id }, calificaciones);
        }

        // DELETE: api/Calificaciones/5
        [ResponseType(typeof(Calificaciones))]
        [HttpDelete]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteCalificaciones(int id)
        {
            Calificaciones calificaciones = db.DbCalificaciones.Find(id);
            if (calificaciones == null)
            {
                return NotFound();
            }

            db.DbCalificaciones.Remove(calificaciones);
            db.SaveChanges();

            return Ok(calificaciones);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CalificacionesExists(int id)
        {
            return db.DbCalificaciones.Count(e => e.Id == id) > 0;
        }
    }
}