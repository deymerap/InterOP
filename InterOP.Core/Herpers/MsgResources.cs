using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace InterOP.Core.Herpers
{
    public static class MsgResources
    {
        private static ResourceManager prvShrRsRecursos;
        public static string ReadResourceString(string vKey)
        {
            if (prvShrRsRecursos == null)
                prvShrRsRecursos = new ResourceManager("InterOP.Core.Resources.ErrorMessages", Assembly.GetExecutingAssembly());

            return prvShrRsRecursos.GetString(vKey);
        }
    }
}
