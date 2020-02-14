using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EmploApiSDK.Configuration
{
    public class AppSettingsConfigurationProvider
    {

        private static string ChunkSizeKey = "ChunkSize";
        private static string EmploUrlKey = "EmploUrl";
        private static string DryRunKey = "DryRun";
        private static string EmploLoginKey = "Login";
        private static string EmploPasswordKey = "Password";
        private static string ApiPathKey = "ApiPath";
        private static string ImportFromFilePath = "ImportFromFilePath";

        public static string GetConfigurationValue(string key)
        {
            return  ConfigurationManager.AppSettings[key] ?? null;
        }


        public static int GetChunkSize()
        {
            string sizeString = GetConfigurationValue(ChunkSizeKey);
            int size;
            if (Int32.TryParse(sizeString, out size))
                return size;
            return 5;
        }

        public static string GetEmploUrl()
        {
            return GetConfigurationValue(EmploUrlKey);
        }

        public static bool IsDryRunMode()
        {
            string value = GetConfigurationValue(DryRunKey);
            bool isDryRunMode;
            if (bool.TryParse(value, out isDryRunMode))
                return isDryRunMode;
            return false;
        }

        public static string GetApiPath()
        {
            return GetConfigurationValue(ApiPathKey) ?? "ApiV2";
        }

        public static string GetEmploLogin()
        {
            return GetConfigurationValue(EmploLoginKey);
        }
        public static string GetEmploPassword()
        {
            return GetConfigurationValue(EmploPasswordKey);
        }

        public static string GetImportFromFilePath()
        {
            return GetConfigurationValue(ImportFromFilePath);
        }




    }
}
