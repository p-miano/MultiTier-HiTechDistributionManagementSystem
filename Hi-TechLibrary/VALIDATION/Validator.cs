using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hi_TechLibrary.VALIDATION
{
    public static class Validator
    {
        public static bool IsValidID(string input)
        {
            return int.TryParse(input, out _); // The 'out _' discards the out parameter since we're only interested in whether the conversion succeeds
        }
        public static bool IsValidEmail(string input)
        {
            if (input.Length < 8)
            {
                return false;
            }
            else
            {
                int atPosition = input.IndexOf('@');
                int dotPosition = input.LastIndexOf('.');
                if (atPosition < 2 || dotPosition < atPosition + 2 || dotPosition + 2 >= input.Length)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public static bool IsValidName(string input)
        {
            if (input.Length < 2)
            {
                return false;
            }
            else
            {
                foreach (char c in input)
                {
                    if (!char.IsLetter(c) && c != '-' && c != ' ')
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public static bool isValidUsername(string input)
        {
            if (input.Length < 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
