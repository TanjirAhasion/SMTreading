using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Common
{
    public static class GeneratedProductSerial
    {
        public static string Generate(string brandName, string modelNumber, string sequence)
        {
            var brand = Normalize(brandName);
            var model = Normalize(modelNumber);

            return $"{brand}-{model}_{sequence:D6}";
        }

        private static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value
                .Trim()
                .ToUpper()
                .Replace(" ", "")
                .Replace("/", "")
                .Replace("\\", "")
                .Replace("_", "-");
        }
    }
}
