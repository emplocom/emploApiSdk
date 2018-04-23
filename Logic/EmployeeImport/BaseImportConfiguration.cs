using System;
using System.Collections.Generic;
using System.Configuration;
using EmploApiSDK.Logger;

namespace EmploApiSDK.Logic.EmployeeImport
{
    public abstract class BaseImportConfiguration
    {
        public List<PropertyMapping> PropertyMappings { get; private set; } = new List<PropertyMapping>();

        ///<exception cref = "EmploApiClientFatalException" > Thrown when a fatal error, requiring request abortion, has occurred </exception>
        protected BaseImportConfiguration(ILogger logger)
        {
            var configSection = ConfigurationManager.GetSection(AttributeMappingSection.SectionName) as AttributeMappingSection;
            if (configSection == null)
            {
                logger.WriteLine(String.Format("Attributes mapping in {0} is empty", AttributeMappingSection.SectionName));
                throw new EmploApiClientFatalException($"A fatal error has occurred while reading configuration. Check the logs for details.");
                //Environment.Exit(-1);
            }

            foreach (AttributeMappingElement e in configSection.Instances)
            {
                if (!String.IsNullOrEmpty(e.Value))
                {
                    PropertyMappings.Add(new PropertyMapping(e.Name, e.Value));
                }
            }
        }
    }
}
