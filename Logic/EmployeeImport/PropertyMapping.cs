namespace EmploApiSDK.Logic.EmployeeImport
{
    public class PropertyMapping
    {
        public PropertyMapping(string emploPropertyName, string externalPropertyName)
        {
            EmploPropertyName = emploPropertyName;
            ExternalPropertyName = externalPropertyName;
        }

        public string EmploPropertyName { get; private set; }
        public string ExternalPropertyName { get; private set; }
    }
}
