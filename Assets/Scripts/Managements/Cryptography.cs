using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace RK.Namespace.Security
{
    public static class Cryptography
    {
        #region Variables
        const string KEY_64 = "4To0RPDKC33r3ykMtv5miiLzJmNPPN7L";
        const string IV_64 = "D2C5D7CFDFBA4";
        #endregion

        /// <summary>
        /// EnCrypt Descripted Data string data using key 
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string EnCrypt(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8); else strKey = KEY_64;
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(strKey);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(strContent);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        /// <summary>
        /// Decrypt encrytred string data using the key 
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string DeCrypt(string strContent, string strKey)
        {
            if (string.IsNullOrEmpty(strContent)) return string.Empty;
            if (strKey.Length > 8) strKey = strKey.Substring(0, 8); else strKey = KEY_64;
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(strKey);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(strContent);
            }
            catch
            {
                return null;
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
    }
}