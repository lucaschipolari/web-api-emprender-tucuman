using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Publicacion> Publicacions { get; set; } = new List<Publicacion>();
    public virtual ICollection<Emprendimiento> Emprendimientos { get; set; } = new List<Emprendimiento>();
}
