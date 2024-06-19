using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZYZ_CSharp_Extension.Encrypt
{
    public class EncryptException:Exception
    {
        public EncryptException(string message) : base(message) { }
    }
}
