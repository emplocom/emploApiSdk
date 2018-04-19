using System;
using System.Collections.Generic;
using System.Configuration;
using EmploApiSDK.Logger;

namespace EmploApiSDK.Logic.EmployeeImport
{
    public abstract class BaseImportConfiguration
    {
        public List<PropertyMapping> PropertyMappings { get; private set; } = new List<PropertyMapping>();

        public BaseImportConfiguration(ILogger logger)
        {
            var configSection = ConfigurationManager.GetSection(AttributeMappingSection.SectionName) as AttributeMappingSection;
            if (configSection == null)
            {
                logger.WriteLine(String.Format("Attributes mapping in {0} is empty", AttributeMappingSection.SectionName));
                Environment.Exit(-1);
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
