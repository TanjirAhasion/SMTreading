using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Common
{
    public static class GeneratedProductSerial
    {
        public static string Generate(string modelNumber)
        {
            var shortGuid = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"{modelNumber}#GU#{shortGuid}";
        }
    }
}
