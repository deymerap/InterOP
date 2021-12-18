using System;
using System.Collections.Generic;

namespace InterOP.Core.Entities
{
    public partial class EntEventos : BaseEntity
    {
        public Guid GuidDocto { get; set; }
        public short IndAbjuntos { get; set; }
        public string NotasEntrega { get; set; }
        public DateTime TsCreacion { get; set; }
    }
}
