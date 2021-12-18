create proc sp_proveedor_existe
@p_email	varchar(50),
@p_pwd		varchar(50)
as 
begin 
if exists(select 1 from t001_Proveedores  where  f001_email=@p_email and f001_pwd=@p_pwd)
	begin
		return -1
	end 
else 
	begin 
		return 0
	end
end