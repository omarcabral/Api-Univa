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
    [EnableCors(origins: "*",  headers:"*", methods:"*")]
    public class AlumnoesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Funciones f = new Funciones();
        // GET: api/Alumnoes
        public IQueryable<Alumno> GetAll()
        {
            return db.DbAlumnos;
        }

        // GET: api/Alumnoes/5
        [ResponseType(typeof(Alumno))]
        [HttpGet]
        public IHttpActionResult GetAlumno(int id)
        {
            Alumno alumno = db.DbAlumnos.Find(id);
            if (alumno == null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }


        [ResponseType(typeof(Alumno))]
        [HttpGet]
        public IHttpActionResult IngresarAlumno(string matricula, string contrasena)
        {
            if (matricula==null || matricula.Equals(string.Empty)|| contrasena==null|| contrasena.Equals(string.Empty))
            {
                return BadRequest("Los datos enviados no son válidos");
            }
            else
            {
                contrasena = f.getMD5(contrasena);
                Alumno al = db.DbAlumnos.Where(m => m.Matricula == matricula && m.Password == contrasena).FirstOrDefault();
                if (al != null)
                {
                    return Ok(al);
                }
                else return BadRequest("Alumno no encontrado");
            }

        }
        [HttpGet]
        public IQueryable<Periodo> mostrarPeriodosAlumno(int id)
        {
            var grupo = db.DbCalificaciones.Where(m => m.IdAlumnos.Id == id).Select(m=>m.IdGrupo).Include(m=>m.IdPeriodo).Distinct();
            if(grupo.ToList().Count > 0){
                IQueryable<Periodo> periodos = grupo.Select(m => m.IdPeriodo).Distinct();
                return periodos;
            }
            else
            {
                return null;
            }

        }
        [HttpGet]
        public IHttpActionResult getCalificacionPeriodo(int idPeriodo, int idAlumno)
        {
            var calificaciones = db.DbCalificaciones.Where(m => m.IdAlumnos.Id == idAlumno && m.IdGrupo.IdPeriodo.Id==idPeriodo).Include(m => m.IdGrupo.IdMateria).Include(m=>m.IdGrupo).ToList();
            if (calificaciones.Count > 0)
            {
                List<ViewBoleta> lista = new List<ViewBoleta>();
                foreach (var item in calificaciones)
                {
                    ViewBoleta objeto = new ViewBoleta { Calificacion = item.Calificacion, Grupo = item.IdGrupo.Clave, Materia = item.IdGrupo.IdMateria.Nombre };
                    lista.Add(objeto);
                }
                return Ok(lista);

            }
            else
            {
                return BadRequest("El alumno no cuenta con calificaciones para ese periodo");
            }
        }
        [HttpGet]
        public IHttpActionResult getHistorial(int id)
        {
            var calificaciones = db.DbCalificaciones.Where(m => m.IdAlumnos.Id == id).Include(m => m.IdGrupo).Include(m => m.IdGrupo.IdMateria).ToList();
            if (calificaciones.Count > 0)
            {
                List<ViewBoleta> lista = new List<ViewBoleta>();
                foreach (var item in calificaciones)
                {
                    ViewBoleta objeto = new ViewBoleta { Calificacion = item.Calificacion, Grupo = item.IdGrupo.Clave, Materia = item.IdGrupo.IdMateria.Nombre };
                    lista.Add(objeto);
                }
                return Ok(lista.OrderBy(m=>m.Grupo));
            }
            else
            {
                return BadRequest("El alumno no cuenta con materias cursadas");
            }
        }
        
        [HttpGet]
        public IQueryable mostrarCarrerasAlumno(int id)
        {
            return this.db.DbAlumnoCarrera.Where(m => m.IdAlumno.Id == id).Select(m => m.IdCarrera);
        }

        

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [ResponseType(typeof(Alumno))]
        [HttpPut]
        public IHttpActionResult Put(int id, Alumno alumno)
        {
            if (id != alumno.Id)
            {
                return BadRequest();
            }
            string passActual = db.DbAlumnos.Where(m=>m.Id==alumno.Id).Select(m=>m.Password).FirstOrDefault();
            if (!alumno.Password.Equals(string.Empty))
            {
                
                if (passActual != alumno.Password)
                {
                    alumno.Password = f.getMD5(alumno.Password);
                }
            }
            else
            {
                alumno.Password = passActual;
            }
            db.Entry(alumno).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(alumno);
            //return StatusCode(HttpStatusCode.NoContent);
        }
        
        // POST: api/Alumnoes
        [ResponseType(typeof(Alumno))]
        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult PostAlumno(Alumno alumno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (alumno.Matricula == null || alumno.Matricula.Equals(string.Empty) || alumno.Nombre == null || alumno.Nombre.Equals(string.Empty) || alumno.Apellidos == null || alumno.Apellidos.Equals(string.Empty) || alumno.Password==null || alumno.Password.Equals(string.Empty))
            {
                return BadRequest("los datos matricula, nombre, apellidos y password no pueden ser nulos o vacios");
            }
            alumno.Password = f.getMD5(alumno.Password);
            Alumno prueba = this.db.DbAlumnos.Where(m => m.Matricula == alumno.Matricula).FirstOrDefault();
            if (prueba != null)
            {
                return BadRequest("la matricula ya es usada por el alumno "+prueba.Nombre+" "+prueba.Apellidos);
            }
            db.DbAlumnos.Add(alumno);
            db.SaveChanges();
            return Ok(alumno);
        }

        // DELETE: api/Alumnoes/5
        [ResponseType(typeof(Alumno))]
        [HttpDelete]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult DeleteAlumno(int id)
        {
            Alumno alumno = db.DbAlumnos.Find(id);
            if (alumno == null)
            {
                return NotFound();
            }

            db.DbAlumnos.Remove(alumno);
            db.SaveChanges();

            return Ok(alumno);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlumnoExists(int id)
        {
            return db.DbAlumnos.Count(e => e.Id == id) > 0;
        }
    }
}