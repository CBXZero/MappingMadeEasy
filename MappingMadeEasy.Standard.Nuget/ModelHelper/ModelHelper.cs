using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;

namespace MappingMadeEasy.Standard.Nuget.ModelHelper
{
    public class ModelHelper
    {
        public ModelCompareResult DoModelsMatch<T, T2>(T firstObjectToCompare, T2 secondObjectToCompare, bool strict = true)
            where T : class
            where T2 : class
        {
            var firstProperties = typeof(T)
                .GetProperties().Select(x => CustomAttributeExtensions.GetCustomAttribute<MapToName>((MemberInfo) x, false)).ToList();

            var secondProperties = typeof(T2)
                .GetProperties().Select(x => x.GetCustomAttribute<MapToName>(false)).ToList();

            if (firstProperties.Count == 0 || secondProperties.Count == 0)
            {
                return new ModelCompareResult(false, $"Model of type {(firstProperties.Count == 0 ? typeof(T).ToString(): typeof(T2).ToString())} has no mappable properties");
            }

            if (strict)
            {

                if (firstProperties.Count() != secondProperties.Count())
                {
                    return new ModelCompareResult(false, "Number of mappable properties on each models do not match");
                }

                foreach (var firstProperty in firstProperties)
                {
                    if (!secondProperties.Contains(firstProperty))
                    {
                        return new ModelCompareResult(false, "property names on models do not match"); 
                    }
                }

            }

            var baseNamespaces = new string[] { typeof(T).Namespace?.Split('.')[0], typeof(T2).Namespace?.Split('.')[0] };
            var firstPropertiesToCheck = typeof(T).GetProperties()
                .Select(x => (ModelPropertyName: x.GetCustomAttribute<MapToName>(false)?.Name, Value: x.GetValue(firstObjectToCompare), PropertyType: x.PropertyType));

            foreach (var firstPropertyToCheck in firstPropertiesToCheck)
            {
                foreach (var secondPropertyToCheck in typeof(T2).GetProperties().Where(x =>
                    x.GetCustomAttribute<MapToName>(false)?.Name == firstPropertyToCheck.ModelPropertyName))
                {
                    if (firstPropertyToCheck.Value != null && IsClassMappableModel(firstPropertyToCheck.Value, baseNamespaces))
                    {
                        var method = typeof(ModelHelper).GetMethod("DoModelsMatch");
                        var compareMethod =
                            method.MakeGenericMethod(firstPropertyToCheck.PropertyType, secondPropertyToCheck.PropertyType);
                        var compareResult = (ModelCompareResult)compareMethod.Invoke(this, new[] { firstPropertyToCheck.Value, secondPropertyToCheck.GetValue(secondObjectToCompare), false });

                        if (!compareResult.IsEquivalent)
                        {
                            return new ModelCompareResult(false, compareResult.ResultMessage);
                        }
                    }
                    else if (typeof(IList).IsAssignableFrom(firstPropertyToCheck.PropertyType)
                        && firstPropertyToCheck.Value != null)
                    {
                        foreach (var firstListValue in (IList)firstPropertyToCheck.Value)
                        {
                            if (firstListValue != null && IsClassMappableModel(firstListValue, baseNamespaces))
                            {
                                var matchFound = new ModelCompareResult(false, string.Empty);
                                foreach (var secondListValue in (IList)secondPropertyToCheck.GetValue(secondObjectToCompare))
                                {
                                    var method = typeof(ModelHelper).GetMethod("DoModelsMatch");
                                    var compareMethod =
                                        method.MakeGenericMethod(firstListValue.GetType(), secondListValue.GetType());
                                    matchFound = (ModelCompareResult)compareMethod.Invoke(this, new[] { firstListValue, secondListValue, false });

                                    if (matchFound.IsEquivalent)
                                    {
                                        break;
                                    }
                                }

                                if (!matchFound.IsEquivalent)
                                {
                                    return new ModelCompareResult(false, matchFound.ResultMessage);
                                }
                            }
                            else
                            {
                                if (!((IList)secondPropertyToCheck.GetValue(secondObjectToCompare)).Contains(firstListValue))
                                {
                                    return new ModelCompareResult(false, $"IList {secondPropertyToCheck.Name} does not contain value {firstListValue} in {firstPropertyToCheck.ModelPropertyName}");
                                }
                            }
                        }
                    }
                    else
                    {
                        var value = secondPropertyToCheck.GetValue(secondObjectToCompare);

                        if (!Equals(value, firstPropertyToCheck.Value))
                        {
                            return new ModelCompareResult(false, $"Values do not match. First Property: {firstPropertyToCheck.ModelPropertyName}. First Value: {firstPropertyToCheck.Value}. Second Property: {secondPropertyToCheck.Name}. Value 2: {value}"); 
                        }
                    }

                }

            }

            return new ModelCompareResult(true, ""); ;
        }

        private static bool IsClassMappableModel(object value, string[] baseNamespaces)
        {
            var baseNamespaceOfModel = value.GetType().Namespace?.Split('.')[0];
            return value.GetType().IsClass
                   && (Attribute.GetCustomAttribute(value.GetType(), typeof(IsModel)) != null
                       || baseNamespaces.Contains(baseNamespaceOfModel));
        }
    }
}