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
    public class GrupoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Grupoes
        public IQueryable<Grupo> GetAll()
        {
            return db.DbGrupo;
        }

        // GET: api/Grupoes/5
        [ResponseType(typeof(Grupo))]
        public IHttpActionResult GetGrupo(int id)
        {
            Grupo grupo = db.DbGrupo.Find(id);
            if (grupo == null)
            {
                return NotFound();
            }

            return Ok(grupo);
        }

        public IQueryable<Grupo> getGruposByPeriodo(int id)
        {
            return db.DbGrupo.Where(m => m.IdPeriodo.Id == id).Include(m => m.IdDocente).Include(m => m.IdMateria).Include(m => m.IdPeriodo);
        }
        // PUT: api/Grupoes/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGrupo(int id, Grupo grupo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grupo.Id)
            {
                return BadRequest();
            }
            db.DbGrupo.Attach(grupo);
            db.Entry(grupo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoExists(id))
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

        // POST: api/Grupoes
        [ResponseType(typeof(Grupo))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostGrupo(Grupo grupo)
        {
            grupo.IdPeriodo = this.db.DbPeriodo.Find(grupo.IdPeriodo.Id);
            grupo.IdDocente = this.db.DbDocente.Find(grupo.IdDocente.Id);
            grupo.IdMateria = this.db.DbMaterias.Find(grupo.IdMateria.Id);
            if (grupo.IdDocente==null || grupo.IdMateria==null || grupo.IdPeriodo == null)
            {
                return BadRequest("Verifique los datos, los valores Alumno, Docente, Materia o Grupo no pueden ser nulos");
            }
            db.DbGrupo.Add(grupo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = grupo.Id }, grupo);
        }

        // DELETE: api/Grupoes/5
        [ResponseType(typeof(Grupo))]
        public IHttpActionResult DeleteGrupo(int id)
        {
            Grupo grupo = db.DbGrupo.Find(id);
            if (grupo == null)
            {
                return NotFound();
            }

            db.DbGrupo.Remove(grupo);
            db.SaveChanges();

            return Ok(grupo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GrupoExists(int id)
        {
            return db.DbGrupo.Count(e => e.Id == id) > 0;
        }
    }
}