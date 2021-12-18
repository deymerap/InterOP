using System;
using System.Collections.Generic;

namespace InterOP.Core.DTOs
{
    public partial class EventosDto 
    {
        public long Id { get; set; }
        public Guid GuidDocto { get; set; }
        public short IndAbjuntos { get; set; }
        public string NotasEntrega { get; set; }
        public DateTime? TsCreacion { get; set; }
    }
}
