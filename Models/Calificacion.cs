using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Calificacion
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public int UsuarioId { get; set; }

    public int Puntuacion { get; set; }

    public DateTime? FechaCalificacion { get; set; }

    public string? ComentarioCalificacion { get; set; }

    public virtual Publicacion Publicacion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
