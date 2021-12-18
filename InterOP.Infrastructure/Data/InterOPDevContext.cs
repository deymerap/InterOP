using InterOP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InterOP.Infrastructure.Data
{
    public partial class InterOPDevContext : DbContext
    {
        public InterOPDevContext(DbContextOptions<InterOPDevContext> options) : base(options)
        {
        }

        public virtual DbSet<EntProveedores> EntProveedores { get; set; }
        public virtual DbSet<EntFacturas> EntFacturas { get; set; }
        public virtual DbSet<EntNotas> EntNotas { get; set; }
        public virtual DbSet<EntEventos> EntEventos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntProveedores>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_t001");

                entity.ToTable("t001_Proveedores");

                entity.Property(e => e.Id)
                    .HasColumnName("f001_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("f001_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IndCliente)
                    .HasColumnName("f001_ind_cliente");

                entity.Property(e => e.IndEstado)
                    .HasColumnName("f001_ind_estado");

                entity.Property(e => e.Nit)
                    .IsRequired()
                    .HasColumnName("f001_nit")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pwd)
                    .IsRequired()
                    .HasColumnName("f001_pwd")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PwdSftpProv)
                    .HasColumnName("f001_pwd_sftp_prov")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PwdUrlApiProv)
                    .HasColumnName("f001_pwd_url_api_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RazonSocial)
                    .IsRequired()
                    .HasColumnName("f001_razon_social")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TsCreacion)
                    .HasColumnName("f001_ts_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.TsModificacion)
                    .HasColumnName("f001_ts_modificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.TsVigenciaPwd)
                    .HasColumnName("f001_ts_vigencia_pwd")
                    .HasColumnType("datetime");

                entity.Property(e => e.Url1ApiProv)
                    .HasColumnName("f001_url1_api_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Url2ApiProv)
                    .HasColumnName("f001_url2_api_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Url3ApiProv)
                    .HasColumnName("f001_url3_api_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Url4ApiProv)
                    .HasColumnName("f001_url4_api_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UrlSftpProv)
                    .HasColumnName("f001_url_sftp_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserUrlApiProv)
                    .HasColumnName("f001_user_url_api_prov")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsuSftpProv)
                    .HasColumnName("f001_usu_sftp_prov")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PwdArchivoZip)
                    .HasColumnName("f001_pwd_archivo_zip")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntFacturas>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_t002");

                entity.ToTable("t002_facturas");

                entity.Property(e => e.Id)
                    .HasColumnName("f002_id");

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasColumnName("f002_documento")
                    .HasColumnType("xml");

                entity.Property(e => e.ExtensAbjuntos)
                    .HasColumnName("f002_extens_abjuntos")
                    .HasMaxLength(250);

                entity.Property(e => e.Folio)
                    .IsRequired()
                    .HasColumnName("f002_folio")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.GuidDocto)
                    .HasColumnName("f002_guid_docto");

                entity.Property(e => e.IndAbjuntos)
                    .HasColumnName("f002_ind_abjuntos");

                entity.Property(e => e.IndRepreGrafica)
                    .HasColumnName("f002_ind_repre_grafica");

                entity.Property(e => e.NitAdq)
                    .IsRequired()
                    .HasColumnName("f002_nit_adq")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NitOfe)
                    .IsRequired()
                    .HasColumnName("f002_nit_ofe")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NitProveedor)
                    .IsRequired()
                    .HasColumnName("f002_nit_proveedor")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDocto)
                    .HasColumnName("f002_nombre_docto")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NotasEntrega)
                    .HasColumnName("f002_notas_entrega")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.TicksRecepcion)
                    .HasColumnName("f002_ticks_recepcion");

                entity.Property(e => e.TipoFactura)
                    .HasColumnName("f002_tipo_factura")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TsCreacion)
                    .HasColumnName("f002_ts_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.TsModificacion)
                    .HasColumnName("f002_ts_modificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.UuidCufe)
                    .HasColumnName("f002_uuid_cufe")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.InfoFilesDirS3)
                    .HasColumnName("f002_info_dir_s3")
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntNotas>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_t003");

                entity.ToTable("t003_notas");

                entity.Property(e => e.Id)
                    .HasColumnName("f003_id");

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasColumnName("f003_documento")
                    .HasColumnType("xml");

                entity.Property(e => e.ExtensAbjuntos)
                    .HasColumnName("f003_extens_abjuntos")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Folio)
                    .IsRequired()
                    .HasColumnName("f003_folio")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.GuidDocto)
                    .HasColumnName("f003_guid_docto");

                entity.Property(e => e.IndAbjuntos)
                    .HasColumnName("f003_ind_abjuntos");

                entity.Property(e => e.IndRepreGrafica)
                    .HasColumnName("f003_ind_repre_grafica");

                entity.Property(e => e.NitAdq)
                    .IsRequired()
                    .HasColumnName("f003_nit_adq")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NitOfe)
                    .IsRequired()
                    .HasColumnName("f003_nit_ofe")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NitProveedor)
                    .IsRequired()
                    .HasColumnName("f003_nit_proveedor")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreDocto)
                    .HasColumnName("f003_nombre_docto")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NotasEntrega)
                    .HasColumnName("f003_notas_entrega")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.TicksRecepcion)
                    .HasColumnName("f003_ticks_recepcion");

                entity.Property(e => e.TipoFactura)
                    .HasColumnName("f003_tipo_factura")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TsCreacion)
                    .HasColumnName("f003_ts_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.TsModificacion)
                    .HasColumnName("f003_ts_modificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.UuidCufe)
                    .HasColumnName("f003_uuid_cufe")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.InfoFilesDirS3)
                    .HasColumnName("f003_info_dir_s3")
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntEventos>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_t004");

                entity.ToTable("t004_eventos");

                entity.Property(e => e.Id)
                    .HasColumnName("f004_id");

                entity.Property(e => e.GuidDocto)
                    .HasColumnName("f004_guid_docto");

                entity.Property(e => e.IndAbjuntos)
                    .HasColumnName("f004_ind_abjuntos");

                entity.Property(e => e.NotasEntrega)
                    .HasColumnName("f004_notas_entrega")
                    .HasMaxLength(4000)
                    .IsUnicode(false);

                entity.Property(e => e.TsCreacion)
                    .HasColumnName("f004_ts_creacion")
                    .HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
