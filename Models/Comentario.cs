using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Comentario
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public int UsuarioId { get; set; }

    public string Texto { get; set; } = null!;

    public DateTime? FechaComentario { get; set; }

    public bool? Aprobado { get; set; }

    public virtual Publicacion Publicacion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
