CREATE TABLE t001_Proveedores(
	f001_id					int IDENTITY(1,1) NOT NULL,	
	f001_nit				nvarchar(50) NOT NULL,
	f001_razon_social		nvarchar(200) NOT NULL,
	f001_email				nvarchar(100) NOT NULL,
	f001_pwd				nvarchar(255) NOT NULL,
	f001_ts_vigencia_pwd	datetime NOT NULL,
	f001_ind_cliente		smallint NOT NULL,
	f001_ind_estado			smallint NOT NULL,
	f001_user_url_api_prov	nvarchar(100) NULL,
	f001_pwd_url_api_prov	nvarchar(255) NULL,
	f001_url1_api_prov		nvarchar(255) NULL,
	f001_url2_api_prov		nvarchar(255) NULL,
	f001_url3_api_prov		nvarchar(255) NULL,
	f001_url4_api_prov		nvarchar(255) NULL,
	f001_url_sftp_prov		nvarchar(255) NULL,
	f001_usu_sftp_prov		nvarchar(100) NULL,
	f001_pwd_sftp_prov		nvarchar(255) NULL,
	f001_ts_creacion		datetime NOT NULL,
	f001_ts_modificacion	datetime NULL,
	f001_pwd_archivo_zip	nvarchar(255) NULL,
CONSTRAINT PK_t001 PRIMARY KEY CLUSTERED 
(
	f001_id ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
