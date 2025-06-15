using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Publicacion
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int CantidadDisponible { get; set; }

    public bool? EstaEnOferta { get; set; }

    public decimal? PrecioOferta { get; set; }

    public bool Eliminada { get; set; } = false;


    public string? UrlImagenPrincipal { get; set; }

    public bool? Activa { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual ICollection<Caracteristica> Caracteristicas { get; set; } = new List<Caracteristica>();

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Imagen> Imagens { get; set; } = new List<Imagen>();

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
