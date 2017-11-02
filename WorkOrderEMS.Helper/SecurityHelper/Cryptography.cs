using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// This used to Encrypty the string or Decrypt vise- versa
    /// </summary>
    /// <Createdby>Nagendra Upwanshi</Createdby>
    /// <CreatedDate>Aug-22-2014</CreatedDate>
    public sealed class Cryptography
    {
        private static byte[] KEY = new byte[] { 0x12, 0xe3, 0x4a, 0xa1, 0x45, 0xd2, 0x56, 0x7c, 0x54, 0xac, 0x67, 0x9f, 0x45, 0x6e, 0xaa, 0x56 };
        private static byte[] IV = new byte[] { 0x12, 0xe3, 0x4a, 0xa1, 0x45, 0xd2, 0x56, 0x7c };

        /// <summary>
        /// GetEncryptedData
        /// </summary>
        /// <param name="input"></param>
        /// <param name="strEncryption"></param>
        /// <returns></returns>
        public static string GetEncryptedData(string input, bool stringEncryption)
        {
            try
            {
                if (!stringEncryption) return input;
                return EncryptBase64(input, KEY, IV);
            }
            catch (Exception )
            { throw; }
        }

        /// <summary>
        /// GetDecryptedData
        /// </summary>
        /// <param name="input"></param>
        /// <param name="strDecryption"></param>
        /// <returns></returns>
        public static string GetDecryptedData(string input, bool stringDecryption)
        {
            try
            {
                if (!stringDecryption) return input;
                if (!string.IsNullOrEmpty(input)) input = input.Replace(' ', '+');
                return DecryptBase64(input, KEY, IV);
            }
            catch (Exception)
            {
                throw;
                //return ex.Message; 
            }
        }

        /// <summary>
        /// EncryptBase64
        /// </summary>
        /// <param name="StringToEncrypt"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string EncryptBase64(string StringToEncrypt, byte[] Key, byte[] IV)
        {
            TripleDESCryptoServiceProvider tripledes;
            MemoryStream ms;
            CryptoStream cs;
            try
            {
                tripledes = new TripleDESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(StringToEncrypt);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, tripledes.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                String str = Convert.ToBase64String(ms.ToArray());
                cs.Clear();
                tripledes.Dispose();
                return str;

               
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                 tripledes = null;
                 ms = null;
                 cs = null;
            }
           
        }

        /// <summary>
        /// DecryptBase64
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string DecryptBase64(string ciphertext, byte[] Key, byte[] IV)
        {
            TripleDESCryptoServiceProvider tripledes;
            MemoryStream ms;
            CryptoStream cs;
            try
            {
                 tripledes = new TripleDESCryptoServiceProvider();
                 ms = new MemoryStream();
                 cs = new CryptoStream(ms, tripledes.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                byte[] cipherbytes = Convert.FromBase64String(ciphertext);
                cs.Write(cipherbytes, 0, cipherbytes.Length);
                cs.FlushFinalBlock();
                //construct the string
                byte[] DecryptedArray = ms.ToArray();
                Char[] characters = new Char[43693];
                Decoder dec = Encoding.UTF8.GetDecoder();
                int charlen = dec.GetChars(DecryptedArray, 0, DecryptedArray.Length, characters, 0);
                string DecrpytedString = new string(characters, 0, charlen);
                return DecrpytedString;
            }
            catch (Exception)
            {   
                throw;
            }
        }

        /// <summary>
        /// This is used to Generate a r Chararter Code for Contact verifications
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>24 Jan 2014</CreateDate>
        /// <returns></returns>
        public static string GenerateAnUniqueCodeForVerificationOfSixDigit()
        {
            try
            {

                //string randomValue = "";
                //Random ObjRandome = new Random();
                //randomValue = Convert.ToString(ObjRandome.Next(11111, 99999));
                //return randomValue;
                string CharaterString = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                Random ObjRandome = new Random();
                string Value = "";
                for (int i = 0; i < 6; i++)
                {
                    char ch = CharaterString[ObjRandome.Next(CharaterString.Length)];
                    Value += ch.ToString();
                } return Value;
            }
            catch (Exception)
            { throw ; }
        }

        /// <summary>
        /// This is used to Generate a 4 Chararter Code for Contact verifications
        /// </summary>
        /// <Createdby>Nagendra Upwanshi</Createdby>
        /// <CreateDate>24 Jan 2014</CreateDate>
        /// <returns></returns>
        public static string GenerateAnUniqueCodeForVerificationOfFourDigit()
        {
            try
            {
                string CharaterString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
                Random ObjRandome = new Random();
                string Value = "";
                for (int i = 0; i < 4; i++)
                {
                    char ch = CharaterString[ObjRandome.Next(CharaterString.Length)];
                    Value += ch.ToString();
                } return Value;
            }
            catch (Exception )
            { throw ; }
        }

        //public static string GetEncryptedString(string plaintText)
        //{
        //    var bytestr = EncryptStringToBytes(plaintText);
        //    var returnstr = Convert.ToBase64String(bytestr.ToArray());
        //    //returnstr = returnstr.Replace("/", "786");
        //    return returnstr;
        //}

        public static byte[] EncryptStringToBytes(string plaintext)
        {
            byte[] key = Encoding.UTF8.GetBytes("9757135457251847");
            byte[] iv = Encoding.UTF8.GetBytes("9757135457251847");


            // Check arguments.
            if (plaintext == null || plaintext.Length <= 0)
            {
                throw new ArgumentNullException("","plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("","key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("","key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plaintext);
                        } encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}
