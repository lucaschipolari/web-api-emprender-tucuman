using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Imagen
{
    public int Id { get; set; }

    public int PublicacionId { get; set; }

    public string Url { get; set; } = null!;

    public bool? EsPrincipal { get; set; }

    public int? Orden { get; set; }

    public virtual Publicacion Publicacion { get; set; } = null!;
}
