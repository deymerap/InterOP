CREATE TABLE t002_facturas(
	f002_id					bigint IDENTITY(1,1) NOT NULL,
	f002_guid_docto			uniqueidentifier NOT NULL,
	f002_nombre_docto		nvarchar(250) NULL,
	f002_tipo_factura		nvarchar(5) NULL,
	f002_folio				nvarchar(150) NOT NULL,
	f002_uuid_cufe			nvarchar(250) NULL,
	f002_documento			xml NOT NULL,
	f002_notas_entrega		nvarchar(4000) NULL,
	f002_nit_ofe			nvarchar(50) NOT NULL,
	f002_nit_adq			nvarchar(50) NOT NULL,
	f002_nit_proveedor		nvarchar(50) NOT NULL,
	f002_ticks_recepcion	bigint NULL,
	f002_ind_repre_grafica	smallint NULL,
	f002_ind_abjuntos		smallint NULL,
	f002_extens_abjuntos	nvarchar(250) NULL,
	f002_ts_creacion		datetime NOT NULL,
	f002_ts_modificacion	datetime NULL,
	f002_info_dir_s3		nvarchar(8000) NULL,
CONSTRAINT PK_t002 PRIMARY KEY CLUSTERED 
(
	f002_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO