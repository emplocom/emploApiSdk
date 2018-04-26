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
            if (!(ConfigurationManager.GetSection(AttributeMappingSection.SectionName) is AttributeMappingSection configSection))
            {
                logger.WriteLine($"Attributes mapping in {AttributeMappingSection.SectionName} is empty", LogLevelEnum.Error);
                throw new EmploApiClientFatalException($"A fatal error has occurred while reading configuration.");
            }

            foreach (AttributeMappingElement e in configSection.Instances)
            {
                if (!string.IsNullOrEmpty(e.Value))
                {
                    PropertyMappings.Add(new PropertyMapping(e.Name, e.Value));
                }
            }
        }
    }
}
