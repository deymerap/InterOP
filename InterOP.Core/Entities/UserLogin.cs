using System;
using System.Collections.Generic;
using System.Text;

namespace InterOP.Core.Entities
{
    public class UserLogin
    {
        /// <summary>  
        /// Razon Social proveedor
        /// </summary>
        //[Required]
        //[Display(Name = "NitProveedor")]
        //[StringLength(9, ErrorMessage = "El campo {0} debe contener {2} caracteres de longitud.", MinimumLength = 9)]
        public string u { get; set; }
        /// <summary>  
        /// Contraseña proveedor
        /// </summary>
        //[Required]
        //[Display(Name = "ContraseñaProveedor")]
        //[StringLength(50, ErrorMessage = "El campo {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 8)]
        public string p { get; set; }
    }
}
