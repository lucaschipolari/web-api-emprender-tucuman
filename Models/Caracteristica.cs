using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Caracteristica
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Valor { get; set; } = null!;

    public virtual Publicacion Publicacion { get; set; } = null!;
}
