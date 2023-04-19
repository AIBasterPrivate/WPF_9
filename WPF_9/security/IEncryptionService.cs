using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_9.security
{
    public interface IEncryptionService
    {     
        byte[] Encrypt(byte[] plainText);       
        byte[] Decrypt(byte[] cipherText);
        Encoding Encoding { get;}
        int GenHashCode(string str);
        int GenHashCode(byte[] bytes);
    }
}
