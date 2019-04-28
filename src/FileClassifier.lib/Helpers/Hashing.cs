using System;
using System.Security.Cryptography;

namespace FileClassifier.lib.Helpers
{
    public static class Hashing
    {
        public static string ToSHA1(this byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return Common.Constants.RESPONSE_ERROR;
            }

            using (var sha = new SHA1Managed())
            {
                return BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "");
            }
        }
    }
}