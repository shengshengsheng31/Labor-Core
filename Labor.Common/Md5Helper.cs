using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Labor.Common
{
    public class Md5Helper
    {
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string password)
        {
            //判断非空
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return string.Empty;
            }

            var pwd = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(password);
                byte[] newBuffer = md5.ComputeHash(buffer);
                foreach (var item in newBuffer)
                {
                    pwd = string.Concat(pwd, item.ToString("X2"));
                }
            }
            return pwd;
        }
    }
}
