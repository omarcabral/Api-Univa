using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApi1.Models
{
    public class ApplicationDbContext: DbContext

    {
        public ApplicationDbContext()
            : base("DefaultConection")
        {
        }

        public DbSet<Alumno> DbAlumnos { get; set; }
        public DbSet<Materia> DbMaterias { get; set; }
        public DbSet<Carrera> DbCarreras { get; set; }
        public DbSet<Docente> DbDocente { get; set; }
        public DbSet <AlumnoCarrera> DbAlumnoCarrera { get; set; }
        public DbSet<Grupo> DbGrupo { get; set; }
        public DbSet<PlanEstudios> DbPlan { get; set; }
        public DbSet<Periodo> DbPeriodo { get; set; }
        public DbSet <Calificaciones> DbCalificaciones { get; set; }

    }
}