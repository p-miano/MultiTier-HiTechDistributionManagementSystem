using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hi_TechLibrary.UTILITIES
{
    public static class EnumUtilities
    {
        // Method to get the description of an enum value
        public static string GetEnumDescription(Enum enumValue)
        {
            string name = Enum.GetName(enumValue.GetType(), enumValue);
            if (name != null)
            {
                StringBuilder displayName = new StringBuilder(name.Length * 2);
                displayName.Append(name[0]);
                for (int i = 1; i < name.Length; i++)
                {
                    if (char.IsUpper(name[i]))
                    {
                        displayName.Append(' ');
                    }
                    displayName.Append(name[i]);
                }
                return displayName.ToString();
            }
            return null;
        }
    }
}
