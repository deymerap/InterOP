using InterOP.Core.Entities;
using InterOP.Core.Interfaces;
using InterOP.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace InterOP.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<EntFacturas>, IInvoiceRepository
    {
        public InterOPDevContext prvObjContext;
        public InvoiceRepository(InterOPDevContext pvContext) : base(pvContext) 
        {
            prvObjContext = pvContext;
        }

        public Task<IEnumerable<EntFacturas>> GetAllInvoices()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertDocument(EntFacturas entFacturas, string vPwdEncrypXml)
        {
            var vContextDB = prvObjContext.Database;
            int rowsAffected = 0;
            using (var vObTransaction = vContextDB.BeginTransaction())
            {
                try
                {
                    SqlParameter[] vParams = new SqlParameter[]
                    {
                            new SqlParameter() { ParameterName = "@p_guid_docto", SqlDbType = SqlDbType.UniqueIdentifier, Direction = ParameterDirection.Input, Value = entFacturas.GuidDocto },
                            new SqlParameter() { ParameterName = "@p_nombre_docto", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.NombreDocto.Length,  Direction = ParameterDirection.Input, Value = entFacturas.NombreDocto },
                            new SqlParameter() { ParameterName = "@p_tipo_factura", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.TipoFactura.Length,  Direction = ParameterDirection.Input, Value = entFacturas.TipoFactura },
                            new SqlParameter() { ParameterName = "@p_folio", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.Folio.Length,  Direction = ParameterDirection.Input, Value = entFacturas.Folio },
                            new SqlParameter() { ParameterName = "@p_uuid_cufe", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.UuidCufe.Length,  Direction = ParameterDirection.Input, Value = entFacturas.UuidCufe },
                            new SqlParameter() { ParameterName = "@p_documento", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.Documento.Length,  Direction = ParameterDirection.Input, Value = entFacturas.Documento },
                            new SqlParameter() { ParameterName = "@p_notas_entrega", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.NotasEntrega.Length,  Direction = ParameterDirection.Input, Value = entFacturas.NotasEntrega },
                            new SqlParameter() { ParameterName = "@p_nit_ofe", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.NitOfe.Length,  Direction = ParameterDirection.Input, Value = entFacturas.NitOfe },
                            new SqlParameter() { ParameterName = "@p_nit_adq", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.NitAdq.Length,  Direction = ParameterDirection.Input, Value = entFacturas.NitAdq },
                            new SqlParameter() { ParameterName = "@p_nit_proveedor", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.NitProveedor.Length,  Direction = ParameterDirection.Input, Value = entFacturas.NitProveedor },
                            new SqlParameter() { ParameterName = "@p_ticks_recepcion", SqlDbType = SqlDbType.BigInt,  Direction = ParameterDirection.Input, Value = entFacturas.TicksRecepcion },
                            new SqlParameter() { ParameterName = "@p_ind_repre_grafica", SqlDbType = SqlDbType.SmallInt, Direction = ParameterDirection.Input, Value = entFacturas.IndRepreGrafica },
                            new SqlParameter() { ParameterName = "@p_ind_abjuntos", SqlDbType = SqlDbType.SmallInt, Direction = ParameterDirection.Input, Value = entFacturas.IndAbjuntos },
                            new SqlParameter() { ParameterName = "@p_extens_abjuntos", SqlDbType = SqlDbType.NVarChar, Size = entFacturas.ExtensAbjuntos.Length,  Direction = ParameterDirection.Input, Value = entFacturas.ExtensAbjuntos },
                            new SqlParameter() { ParameterName = "@p_ts_creacion", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, IsNullable = true, Value = entFacturas.TsCreacion },
                            new SqlParameter() { ParameterName = "@p_ts_modificacion", SqlDbType = SqlDbType.DateTime, Direction = ParameterDirection.Input, IsNullable = true, Value =  entFacturas.TsModificacion ?? (object)DBNull.Value },
                            new SqlParameter() { ParameterName = "@p_info_files_dir_S3", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, IsNullable = true, Value =  entFacturas.InfoFilesDirS3 ?? (object)DBNull.Value },
                            new SqlParameter() { ParameterName = "@p_password", SqlDbType = SqlDbType.NVarChar, Size = vPwdEncrypXml.Length,  Direction = ParameterDirection.Input, Value = vPwdEncrypXml }
                    };
                    rowsAffected = await prvObjContext.Database.ExecuteSqlRawAsync("EXEC sp_guardar_docto @p_guid_docto,@p_nombre_docto,@p_tipo_factura,@p_folio,@p_uuid_cufe,@p_documento,@p_notas_entrega,@p_nit_ofe,@p_nit_adq,@p_nit_proveedor,@p_ticks_recepcion,@p_ind_repre_grafica,@p_ind_abjuntos,@p_extens_abjuntos,@p_ts_creacion,@p_ts_modificacion,@p_info_files_dir_S3,@p_password", vParams);
                    vObTransaction.Commit();
                }
                catch (Exception)
                {
                    vObTransaction.Rollback();
                }
            }


            return (rowsAffected > 0);
        }

        //void AddParameterForCommand(ref SqlParameter[] pvParameters, string pvParameterName, SqlDbType pvDbType, int pvSize = 0, ParameterDirection pvDirection = ParameterDirection.Input, object pvValue = null)
        //{
        //    List<object> list = new List<object>();
        //    if (pvParameters != null)
        //    {
        //        pvParameters = new SqlParameter;
        //    }

        //    var vParameter = new SqlParameter
        //    {
        //        ParameterName = pvParameterName,
        //        SqlDbType = pvDbType,
        //        Direction = pvDirection,
        //        Value = pvValue
        //    };
        //    if (pvSize != 0)
        //        vParameter.Size = pvSize;
        //    list.Add(vParameter);


        //}
    }
}
