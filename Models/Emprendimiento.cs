using System;
using System.Collections.Generic;

namespace EmprenderTucumanWebApi.Models;

public partial class Emprendimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? FotoPerfil { get; set; }

    public string? FotoPortada { get; set; }

    public string? Direccion { get; set; }

    public string? Instagram { get; set; }

    public string? Facebook { get; set; }

    public string? WhatsApp { get; set; }

    public string? Descripcion { get; set; }

    public string? Historia { get; set; }

    public int CategoriaId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool Estado { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
