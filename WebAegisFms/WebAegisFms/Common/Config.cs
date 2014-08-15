using System.Configuration;

namespace Common
{
    //Nilesh@20100516 : Converted class to static to remove "Static holder types should not have constructors" error
    public static class Config
    {
        public static string Get(string parameterName) 
        {
            return ConfigurationManager.AppSettings[parameterName];
        }
    }
}