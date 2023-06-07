using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TradeMonkey.Core.Extensions
{
    public static class StringExtensions
    {
        public static SecureString ConvertToSecureString(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            var secureStr = new SecureString();
            foreach (char ch in str)
            {
                secureStr.AppendChar(ch);
            }
            secureStr.MakeReadOnly();
            return secureStr;
        }
    }
}