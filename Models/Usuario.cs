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
    public string? ImagenPerfil { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public bool? Activo { get; set; }
    
    // ✅ Solo rol principal
    public int RolId { get; set; } = 1; // Default: Cliente
    public virtual Role Rol { get; set; } = null!;
    
    // Relaciones existentes
    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();
    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    public virtual ICollection<Publicacion> PublicacionsNavigation { get; set; } = new List<Publicacion>();
    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();

    public virtual ICollection<Emprendimiento> Emprendimientos { get; set; } = new List<Emprendimiento>();
    // ❌ REMOVER ESTA LÍNEA:
    // public virtual ICollection<Role> Rols { get; set; } = new List<Role>();

    // ✅ Métodos de utilidad
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    
    public bool PuedeVender => Rol?.Nivel >= 2;
    public bool PuedeGestionar => Rol?.Nivel >= 3;
    public bool PuedeAdministrar => Rol?.Nivel >= 4;
}