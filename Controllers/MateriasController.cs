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
    public class MateriasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Materias
        public IQueryable<Materia> GetAll()
        {
            return db.DbMaterias;
        }

        // GET: api/Materias/5
        [ResponseType(typeof(Materia))]
        public IHttpActionResult GetMateria(int id)
        {
            Materia materia = db.DbMaterias.Find(id);
            if (materia == null)
            {
                return NotFound();
            }

            return Ok(materia);
        }

        // PUT: api/Materias/5
        [ResponseType(typeof(Materia))]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutMateria(int id, Materia materia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != materia.Id)
            {
                return BadRequest();
            }

            db.Entry(materia).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(materia);
        }

        // POST: api/Materias
        [ResponseType(typeof(Materia))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostMateria(Materia materia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (materia.Clave == null || materia.Nombre==null || materia.Clave.Equals(string.Empty) || materia.Nombre.Equals(string.Empty))
            {
                return BadRequest("La clave o el nombre de la materia no pueden ser nulos o vacios");
            }
            var auxiliar = this.db.DbMaterias.Where(m => m.Clave == materia.Clave && m.Nombre == materia.Nombre).FirstOrDefault();
            if (auxiliar!=null)
            {
                return BadRequest("La materia ya existe");
            }
            db.DbMaterias.Add(materia);
            db.SaveChanges();
            return Ok(materia);
        }

        // DELETE: api/Materias/5
        [ResponseType(typeof(Materia))]
        [HttpDelete]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteMateria(int id)
        {
            Materia materia = db.DbMaterias.Find(id);
            if (materia == null)
            {
                return NotFound();
            }

            db.DbMaterias.Remove(materia);
            db.SaveChanges();

            return Ok(materia);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MateriaExists(int id)
        {
            return db.DbMaterias.Count(e => e.Id == id) > 0;
        }
    }
}