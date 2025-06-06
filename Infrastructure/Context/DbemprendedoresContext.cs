using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EmprenderTucumanWebApi.Models;

public partial class DBemprendedoresContext : DbContext
{
    public DBemprendedoresContext()
    {
    }

    public DBemprendedoresContext(DbContextOptions<DBemprendedoresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calificacion> Calificacions { get; set; }

    public virtual DbSet<Caracteristica> Caracteristicas { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Imagen> Imagens { get; set; }

    public virtual DbSet<Publicacion> Publicacions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Califica__3214EC0728A1AEE2");

            entity.ToTable("Calificacion");

            entity.Property(e => e.ComentarioCalificacion).HasColumnType("text");
            entity.Property(e => e.FechaCalificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calificac__Publi__123EB7A3");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Calificacions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Calificac__Usuar__1332DBDC");
        });

        modelBuilder.Entity<Caracteristica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Caracter__3214EC07F1F0742F");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Valor)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Caracteristicas)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Caracteri__Publi__17036CC0");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07104916C9");

            entity.HasIndex(e => e.Nombre, "UQ__Categori__75E3EFCF6F3B5A29").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comentar__3214EC07AC2491CB");

            entity.ToTable("Comentario");

            entity.Property(e => e.Aprobado).HasDefaultValue(false);
            entity.Property(e => e.FechaComentario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Texto).HasColumnType("text");

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__Publi__0D7A0286");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__Usuar__0E6E26BF");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imagen__3214EC076B1A55CC");

            entity.ToTable("Imagen");

            entity.Property(e => e.EsPrincipal).HasDefaultValue(false);
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Publicacion).WithMany(p => p.Imagens)
                .HasForeignKey(d => d.PublicacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Imagen__Publicac__08B54D69");
        });

        modelBuilder.Entity<Publicacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publicac__3214EC07066987DB");

            entity.ToTable("Publicacion");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.CantidadDisponible).HasDefaultValue(1);
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.EstaEnOferta).HasDefaultValue(false);
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrecioOferta).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UltimaActualizacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrlImagenPrincipal)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Publicacions)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Publicaci__Categ__04E4BC85");

            entity.HasOne(d => d.Usuario).WithMany(p => p.PublicacionsNavigation)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Publicaci__Usuar__03F0984C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC079B4E6802");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCF31C5EC91").IsUnique();

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC0779F16C3E");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE01F23C780").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534C753940C").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Publicacions).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "Favorito",
                    r => r.HasOne<Publicacion>().WithMany()
                        .HasForeignKey("PublicacionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favoritos__Publi__1AD3FDA4"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Favoritos__Usuar__19DFD96B"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "PublicacionId").HasName("PK__Favorito__9A3016E098F5126E");
                        j.ToTable("Favoritos");
                    });

            entity.HasMany(d => d.Rols).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .HasConstraintName("FK__UsuarioRo__RolId__797309D9"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .HasConstraintName("FK__UsuarioRo__Usuar__787EE5A0"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "RolId").HasName("PK__UsuarioR__24AFD797824DCE1E");
                        j.ToTable("UsuarioRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
