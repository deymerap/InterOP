CREATE TABLE t003_notas(
	f003_id					bigint IDENTITY(1,1) NOT NULL,
	f003_guid_docto			uniqueidentifier NOT NULL,
	f003_nombre_docto		nvarchar(250) NULL,
	f003_tipo_factura		nvarchar(5) NULL,
	f003_folio				nvarchar(150) NOT NULL,
	f003_uuid_cufe			nvarchar(250) NULL,
	f003_documento			xml NOT NULL,
	f003_notas_entrega		nvarchar(4000) NULL,
	f003_nit_ofe			nvarchar(50) NOT NULL,
	f003_nit_adq			nvarchar(50) NOT NULL,
	f003_nit_proveedor		nvarchar(50) NOT NULL,
	f003_ticks_recepcion	bigint NULL,
	f003_ind_repre_grafica	smallint NULL,
	f003_ind_abjuntos		smallint NULL,
	f003_extens_abjuntos	nvarchar(250) NULL,
	f003_ts_creacion		datetime NOT NULL,
	f003_ts_modificacion	datetime NULL,
	f003_info_dir_s3		nvarchar(8000) NULL,
CONSTRAINT PK_t003 PRIMARY KEY CLUSTERED 
(
	f003_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO