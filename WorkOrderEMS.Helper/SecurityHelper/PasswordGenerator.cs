using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helper
{
    /// <summary>
    /// This used Automatic generate passowrd
    /// </summary>
    /// <Createdby>Nagendra Upwanshi</Createdby>
    /// <CreatedDate>Aug-22-2014</CreatedDate>

    public static class PasswordGenerator
    {
        static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
        static string numerics = "1234567890";
        static string special = "@#$!*";
        //create another string which is a concatenation of all above
        static string allChars = alphaCaps + alphaLow + numerics + special;

        public static string GeneratePassword(int length)
        {
            Random r = new Random();
            String generatedPassword = "";
            for (int i = 0; i < length; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    //First character is an upper case alphabet
                    generatedPassword += alphaCaps.ToCharArray()[(int)Math.Floor(rand * alphaCaps.Length)];
                }
                else
                    if (i == 3)
                        generatedPassword += special.ToCharArray()[(int)Math.Floor(rand * special.Length)];
                    else
                    {
                        generatedPassword += allChars.ToCharArray()[(int)Math.Floor(rand * allChars.Length)];
                    }
            }
            return generatedPassword;
        }
    }
}