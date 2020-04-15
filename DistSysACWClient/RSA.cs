﻿using System;
using CoreExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACWClient
{
    public static class RSA
    {
        static RSA(){}

        // Encrypt data using the public key.
        /*static public byte[] RSAEncrypt(byte[] DataToEncrypt, string key)
        {
            try
            {
                byte[] encryptedData; using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    // Convert string to keyParameter
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e) { Console.WriteLine(e.Message); return null; }
        }*/

        // Decrypt data using the private key.
        static public byte[] RSADecrypt(byte[] DataToDecrypt,string key) 
        {
            try 
            { 
                byte[] decryptedData; 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) 
                {
                    RSACryptoExtensions.FromXmlStringCore22(RSA, key);

                    decryptedData = RSA.Decrypt(DataToDecrypt, false);
                } 
                return decryptedData;
            } 
            catch (CryptographicException e) 
            { 
                Console.WriteLine(e.ToString()); 
                return null; 
            } 
        }
    }
}
