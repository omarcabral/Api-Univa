using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi1.Models
{
    public class model

    {


    }
    public class Persona
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellidos { get; set; }
        public string Rfc { get; set; }
        public string Curp { get; set; }
        [Required]
        public string Password { get; set; }
        public Boolean Logueado { get; set; }

    }
    public class Alumno : Persona
    {
        [Required]
        public string Matricula { get; set; }


    }
    public class Docente : Persona
    {
        public string Clabe { get; set; }
        [Required]
        public string Usuario { get; set; }
    }
    public class Materia
    {
        public int Id { get; set; }
        [Required]
        public String Nombre { get; set; }
        public int Creditos { get; set; }
        [Required]
        public string Clave { get; set; }
    }

    public class Carrera
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Clave { get; set; }
        public PlanEstudios IdPlan { get; set; }
    }

    public class PlanEstudios
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }

    }

    public class AlumnoCarrera
    {
        public int Id { get; set; }
        public Alumno IdAlumno { get; set; }
        public Carrera IdCarrera { get; set; }
    }

    
    public class Periodo {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
    }


    public class Grupo
    {
        public int Id { get; set; }
        public Docente IdDocente { get; set; }
        public Periodo IdPeriodo { get; set; }
        public Materia IdMateria { get; set; }
        [Required]
        public string Clave { get; set; }
        
    }
    public class Calificaciones
    {
        public int Id { get; set; }
        public Alumno IdAlumnos { get; set; }
        public int Calificacion { get; set; }
        public Grupo IdGrupo { get; set; }
    }
    public class ViewBoleta
    {
        public string Grupo { get; set; }
        public string Materia { get; set; }
        public int Calificacion { get; set; }
    }


}