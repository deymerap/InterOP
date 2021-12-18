using System;
using System.Collections.Generic;

namespace InterOP.Core.DTOs
{
    public partial class ProveedoresDto
    {
        public int Id { get; set; }
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public string Email { get; set; }
        public string Pwd { get; set; }
        public short IndCliente { get; set; }
        public short IndEstado { get; set; }
        public string UserUrlApiProv { get; set; }
        public string PwdUrlApiProv { get; set; }
        public string Url1ApiProv { get; set; }
        public string Url2ApiProv { get; set; }
        public string Url3ApiProv { get; set; }
        public string Url4ApiProv { get; set; }
        public string UrlSftpProv { get; set; }
        public string UsuSftpProv { get; set; }
        public string PwdSftpProv { get; set; }
        public DateTime? TsCreacion { get; set; }
        public DateTime? TsModificacion { get; set; }
        public DateTime? TsVigenciaPwd { get; set; }
    }
}
