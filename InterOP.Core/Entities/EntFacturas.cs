using System;
using System.Collections.Generic;

namespace InterOP.Core.Entities
{
    public partial class EntFacturas : BaseEntity
    {
        public Guid GuidDocto { get; set; }
        public string NombreDocto { get; set; }
        public string TipoFactura { get; set; }
        public string Folio { get; set; }
        public string UuidCufe { get; set; }
        public string Documento { get; set; }
        public string NotasEntrega { get; set; }
        public string NitOfe { get; set; }
        public string NitAdq { get; set; }
        public string NitProveedor { get; set; }
        public long? TicksRecepcion { get; set; }
        public short? IndRepreGrafica { get; set; }
        public short? IndAbjuntos { get; set; }
        public string ExtensAbjuntos { get; set; }
        public DateTime TsCreacion { get; set; }
        public DateTime? TsModificacion { get; set; }
        public string InfoFilesDirS3 { get; set; }
    }
}
