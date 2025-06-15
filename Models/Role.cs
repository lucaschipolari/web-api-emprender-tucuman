using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Role
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int Nivel { get; set; } 
    public bool Activo { get; set; } = true;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}