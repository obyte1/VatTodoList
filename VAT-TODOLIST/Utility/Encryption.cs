using System.Text;
using System;

namespace VAT_TODOLIST.Utility
{
    public class Encryption
    {
        public static string encryptId(string ToEncrypt)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(ToEncrypt));
        }
        public static string decryptId(string cypherString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }
    }
}
