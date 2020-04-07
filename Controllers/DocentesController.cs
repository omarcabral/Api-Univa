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
    public class DocentesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Funciones f = new Funciones();
        // GET: api/Docentes
        public IQueryable<Docente> GetAll()
        {
            return db.DbDocente;
        }

        // GET: api/Docentes/5
        [ResponseType(typeof(Docente))]
        public IHttpActionResult GetDocente(int id)
        {
            Docente docente = db.DbDocente.Find(id);
            if (docente == null)
            {
                return NotFound();
            }

            return Ok(docente);
        }

        [ResponseType(typeof(Docente))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult IngresarDocente(string usuario, string contrasena)
        {
            if (usuario == null || usuario.Equals(string.Empty) || contrasena == null || contrasena.Equals(string.Empty))
            {
                return BadRequest("Los datos enviados no son válidos");
            }
            else
            {
                contrasena = f.getMD5(contrasena);
                Docente al = db.DbDocente.Where(m => m.Usuario == usuario && m.Password == contrasena).FirstOrDefault();
                if (al != null)
                {
                    return Ok(al);
                }
                else return BadRequest("Alumno no encontrado");
            }

        }

        // PUT: api/Docentes/5
        [ResponseType(typeof(Docente))]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutDocente(int id, Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != docente.Id)
            {
                return BadRequest();
            }

            string passActual = db.DbDocente.Where(m => m.Id == docente.Id).Select(m => m.Password).FirstOrDefault();
            if (!docente.Password.Equals(string.Empty))
            {
                if (passActual != docente.Password)
                {
                    docente.Password = f.getMD5(docente.Password);
                }
            }
            else
            {
                docente.Password = passActual;
            }

            db.Entry(docente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocenteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(docente);
        }

        // POST: api/Docentes
        [ResponseType(typeof(Docente))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostDocente(Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            docente.Password = f.getMD5(docente.Password);
            db.DbDocente.Add(docente);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = docente.Id }, docente);
        }

        // DELETE: api/Docentes/5
        [ResponseType(typeof(Docente))]
        [HttpDelete]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteDocente(int id)
        {
            Docente docente = db.DbDocente.Find(id);
            if (docente == null)
            {
                return NotFound();
            }

            db.DbDocente.Remove(docente);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocenteExists(int id)
        {
            return db.DbDocente.Count(e => e.Id == id) > 0;
        }
    }
}