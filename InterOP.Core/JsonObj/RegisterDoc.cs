using InterOP.Core.JsonObj;
using Microsoft.AspNetCore.Http;

namespace InterOP.Core.JsonObj  
{
    public class RegisterDoc
    {
        public IFormFile DataZip { get; set; }
        public string InfoDocuments { get; set; }
    }
}