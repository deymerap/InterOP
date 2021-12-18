using System;
using System.Collections.Generic;
using System.Text;

namespace InterOP.Core.Responses
{


    public class ResponseDocumentProcess
    {
        public string timeStamp { get; set; }
        public IList<TrackingId> trackingIds { get; set; }
        public string mensajeGlobal { get; set; }
    }

    public class TrackingId
    {
        public string nombreDocumento { get; set; }
        public string uuid { get; set; }
        public int codigoError { get; set; }
        public string mensaje { get; set; }
    }

}
