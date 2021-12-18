CREATE TABLE t004_eventos(
	f004_id					bigint IDENTITY(1,1) NOT NULL,
	f004_guid_docto			uniqueidentifier NOT NULL,
	f004_ind_abjuntos		smallint NOT NULL,
	f004_notas_entrega		nvarchar(4000) NULL,
	f004_ts_creacion		datetime NOT NULL,
 CONSTRAINT PK_t004 PRIMARY KEY CLUSTERED 
(
	f004_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
) 

GO

SET ANSI_PADDING OFF
GO
