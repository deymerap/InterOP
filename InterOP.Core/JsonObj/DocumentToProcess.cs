using System.Collections.Generic;

namespace InterOP.Core.JsonObj
{
    public class DocumentToProcess
    {
        public string nombre { get; set; }
        public string uuid { get; set; }
        public bool zipReqPassword { get; set; }
        public IList<Extensione> extensiones { get; set; }
        public IList<Documento> documentos { get; set; }
    }

    public class Extensione 
    {
        public string nombreExt { get; set; }
        public string valorExt { get; set; }
    }
    public class Documento
    {
        public string nombre { get; set; }
        public string sha256 { get; set; }
        public string tipo { get; set; }
        public string notaDeEntrega { get; set; }
        public bool adjuntos { get; set; }
        public bool representacionGraficas { get; set; }
        public string identificacionDestinatario { get; set; }
        public IList<Extensione> extensiones { get; set; }
    }



}
