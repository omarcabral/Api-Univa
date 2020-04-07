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
    public class PeriodoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Periodoes
        public IQueryable<Periodo> GetAll()
        {
            return db.DbPeriodo;
        }

       

        // GET: api/Periodoes/5
        [ResponseType(typeof(Periodo))]
        public IHttpActionResult GetPeriodo(int id)
        {
            Periodo periodo = db.DbPeriodo.Find(id);
            if (periodo == null)
            {
                return NotFound();
            }

            return Ok(periodo);
        }

        // PUT: api/Periodoes/5
        [ResponseType(typeof(Periodo))]
        [HttpPut]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PutPeriodo(int id, Periodo periodo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != periodo.Id)
            {
                return BadRequest();
            }

            db.Entry(periodo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeriodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(periodo);
        }

        // POST: api/Periodoes
        [ResponseType(typeof(Periodo))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostPeriodo(Periodo periodo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Periodo p = db.DbPeriodo.Where(m => m.Nombre == periodo.Nombre).FirstOrDefault();
            if (p!=null)
            {
                return BadRequest("El periodo ya existe");
            }
            db.DbPeriodo.Add(periodo);
            db.SaveChanges();

            return Ok(periodo);
        }

        // DELETE: api/Periodoes/5
        [ResponseType(typeof(Periodo))]
        [HttpDelete]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeletePeriodo(int id)
        {
            Periodo periodo = db.DbPeriodo.Find(id);
            if (periodo == null)
            {
                return NotFound();
            }
            
            db.DbPeriodo.Remove(periodo);
            try
            {
                db.SaveChanges();
            }
            catch(Exception e){
                return BadRequest("No se puede eliminar");
            }

            return Ok(periodo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PeriodoExists(int id)
        {
            return db.DbPeriodo.Count(e => e.Id == id) > 0;
        }
    }
}