using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CotizacionDolarBNA.Models;

public partial class ErreparContext : DbContext
{
    public ErreparContext()
    {
    }

    public ErreparContext(DbContextOptions<ErreparContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CotizacionesBna> CotizacionesBnas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=192.168.1.254;Port=5433;Database=errepar;UserId=errepar;Password=Errepar123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CotizacionesBna>(entity =>
        {
            entity.HasKey(e => e.CotizacionesId).HasName("cotizaciones_bna_pkey");

            entity.ToTable("cotizaciones_bna", "cotizacion_dolar_bna");

            entity.Property(e => e.CotizacionesId)
                .ValueGeneratedNever()
                .HasColumnName("cotizaciones_id");
            entity.Property(e => e.BilleteCompra)
                .HasColumnType("character varying")
                .HasColumnName("billete_compra");
            entity.Property(e => e.BilleteVenta)
                .HasColumnType("character varying")
                .HasColumnName("billete_venta");
            entity.Property(e => e.DivisaCompra)
                .HasColumnType("character varying")
                .HasColumnName("divisa_compra");
            entity.Property(e => e.DivisaVenta)
                .HasColumnType("character varying")
                .HasColumnName("divisa_venta");
            entity.Property(e => e.Fecha)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
