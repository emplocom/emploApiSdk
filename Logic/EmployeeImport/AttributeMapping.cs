using System.Configuration;

namespace EmploApiSDK.Logic.EmployeeImport
{
    public class AttributeMapping : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AttributeMappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AttributeMappingElement)element).Name;
        }
    }
}
