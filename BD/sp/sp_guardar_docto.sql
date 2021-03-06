ALTER PROCEDURE sp_guardar_docto
	@p_guid_docto          uniqueidentifier,
	@p_nombre_docto        nvarchar(250),
	@p_tipo_factura        nvarchar(5),
	@p_folio               nvarchar(150),
	@p_uuid_cufe           nvarchar(250),
	@p_documento           nvarchar(max),
	@p_notas_entrega       nvarchar(4000),
	@p_nit_ofe             nvarchar(50),
	@p_nit_adq             nvarchar(50),
	@p_nit_proveedor       nvarchar(50),
	@p_ticks_recepcion     bigint,
	@p_ind_repre_grafica   smallint,
	@p_ind_abjuntos        smallint,
	@p_extens_abjuntos     nvarchar(250),
	@p_ts_creacion         datetime,
	@p_ts_modificacion     datetime,
	@p_info_files_dir_S3   varchar(8000),
	@p_password			   nvarchar(255)
AS
BEGIN

	DECLARE @v_xml_encryp XML
	
	DECLARE @open nvarchar(200)
	SET @open = 'OPEN SYMMETRIC KEY PSTInterOP DECRYPTION BY PASSWORD = ' + quotename(@p_password,'''') + ';';
	EXEC sp_executesql @open
	SET @v_xml_encryp = dbo.EncryptString(@p_documento, 'PSTInterOP')

	INSERT INTO t002_facturas
           (f002_guid_docto,
			f002_nombre_docto,
			f002_tipo_factura,
			f002_folio,
			f002_uuid_cufe,
			f002_documento,
			f002_notas_entrega,
			f002_nit_ofe,
			f002_nit_adq,
			f002_nit_proveedor,
			f002_ticks_recepcion,
			f002_ind_repre_grafica,
			f002_ind_abjuntos,
			f002_extens_abjuntos,
			f002_ts_creacion,
			f002_ts_modificacion,
			f002_info_dir_s3)
     VALUES
           (@p_guid_docto,
			@p_nombre_docto,
			@p_tipo_factura,
			@p_folio,
			@p_uuid_cufe,
			@v_xml_encryp,
			@p_notas_entrega,
			@p_nit_ofe,
			@p_nit_adq,
			@p_nit_proveedor,
			@p_ticks_recepcion,
			@p_ind_repre_grafica,
			@p_ind_abjuntos,
			@p_extens_abjuntos,
			@p_ts_creacion,
			@p_ts_modificacion,
			@p_info_files_dir_S3)

	 CLOSE SYMMETRIC KEY PSTInterOP;
END
go
