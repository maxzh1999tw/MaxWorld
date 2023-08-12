namespace MaxWorld.Web.Utilities
{
    public static class ObjectMapper
    {
        public static T MapTo<T>(object fromModel, Func<string, string?>? sourceFindingRules = null) where T : new()
        {
            var fromProperties = fromModel.GetType().GetProperties();
            var toProperties = typeof(T).GetProperties();

            var result = new T();

            foreach (var property in toProperties)
            {
                if (sourceFindingRules != null)
                {
                    var sourcePropertyName = sourceFindingRules(property.Name);
                    if (!string.IsNullOrEmpty(sourcePropertyName))
                    {
                        var sourceProperty = fromProperties.First(p => p.Name == sourcePropertyName);
                        property.SetValue(result, sourceProperty.GetValue(fromModel));
                    }
                }
                else
                {
                    var sourceProperty = fromProperties.FirstOrDefault(s => s.Name == property.Name);
                    if (sourceProperty != null)
                    {
                        property.SetValue(result, sourceProperty.GetValue(fromModel));
                    }

                }
            }

            return result;
        }
    }
}
