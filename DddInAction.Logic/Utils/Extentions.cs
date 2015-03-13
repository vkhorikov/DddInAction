using System;


namespace DddInAction.Logic.Utils
{
    public static class Extentions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
