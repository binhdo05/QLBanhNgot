using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAn_QLBanhNgot
{
    public class Utility
    {
        public static bool IsDigit(string input)
        {
            return long.TryParse(input, out _);
        }

        public static bool IsPhoneNumber(string input)
        {
            string phonePattern = @"^0\d{9,10}$";
            return Regex.IsMatch(input, phonePattern);
        }
    }
}
