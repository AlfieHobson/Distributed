using System;
using CoreExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DistSysACW
{
    public static class RSA
    {
        private static RSACryptoServiceProvider rsa;
        private static CspParameters cspParams;

        static RSA()
        {
            // Create container
            cspParams = new CspParameters();
            cspParams.KeyContainerName = ("KeyContainer");
            RSACryptoServiceProvider.UseMachineKeyStore = true;

            // Create RSAProvider using a container.
            rsa = new RSACryptoServiceProvider(cspParams);
            rsa.PersistKeyInCsp = false;
        }

        static public string getPublicKey () 
        {
            string key = RSACryptoExtensions.ToXmlStringCore22(rsa, false);
            return key; 
        }

        // Encrypt data using the public key.
        static public byte[] RSAEncrypt(byte[] DataToEncrypt)
        {
            try
            {
                byte[] encryptedData; using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    RSA.ImportParameters(rsa.ExportParameters(false)); 
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e) { Console.WriteLine(e.Message); return null; }
        }

        // Decrypt data using the private key.
        static public byte[] RSADecrypt(byte[] DataToDecrypt) 
        {
            try 
            { 
                byte[] decryptedData; 
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider()) 
                { 
                    RSA.ImportParameters(rsa.ExportParameters(true)); 
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

        // Encrypt data using the private key.
        static public byte[] RSASign (byte[] DataToEncrypt)
        {
            try
            {
                byte[] encryptedData; using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    RSA.ImportParameters(rsa.ExportParameters(true));
                    encryptedData = RSA.Encrypt(DataToEncrypt, false);
                }
                return encryptedData;
            }
            catch (CryptographicException e) { Console.WriteLine(e.Message); return null; }
        }
    }
}
