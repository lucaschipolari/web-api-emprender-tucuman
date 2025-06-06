using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Publicacion> PublicacionsNavigation { get; set; } = new List<Publicacion>();

    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();

    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();
}
