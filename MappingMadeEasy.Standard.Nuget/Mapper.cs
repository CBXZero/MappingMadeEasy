using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using MappingMadeEasy.Standard.Nuget.CustomDataAttributes;
using MappingMadeEasy.Standard.Nuget.CustomExceptions;

namespace MappingMadeEasy.Standard.Nuget
{
    public class Mapper : IMapper
    {
        public TModelToMapToo Map<TModelWithValues, TModelToMapToo>(TModelWithValues objectWithValues, bool strict = true)
            where TModelWithValues : class
            where TModelToMapToo : new()
        {
            if (objectWithValues == null)
            {
                throw new ArgumentNullException();
            }

            var baseNamespaces = new[] {typeof(TModelWithValues).Namespace?.Split('.')[0], typeof(TModelToMapToo).Namespace?.Split('.')[0]};
            var propertiesWithValues = typeof(TModelWithValues).GetProperties()
                .Where(x => x.GetCustomAttribute<MapToName>(false) != null)
                .Select(x => (ModelPropertyName: x.GetCustomAttribute<MapToName>(false)?.Name, Value: x.GetValue(objectWithValues), PropertyType: x.PropertyType)).ToList();


            if (strict)
            {
                CheckThatPropertiesMatch<TModelWithValues, TModelToMapToo>(propertiesWithValues);
            }
            

            var objectToMapToo = new TModelToMapToo();
            foreach (var property in propertiesWithValues)
            {
                foreach (var propertyToMap in typeof(TModelToMapToo).GetProperties().Where(x => x.GetCustomAttribute<MapToName>(false)?.Name == property.ModelPropertyName))
                {
                    if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string)
                                                                                    && property.Value != null)
                    {
                        var mappedList = Activator.CreateInstance(propertyToMap.PropertyType);

                        foreach (var value in (IList)property.Value)
                        {
                            var listTypeToMapToo = propertyToMap.PropertyType.GetGenericArguments()[0];
                            if (value != null && IsClassMappableModel(value, baseNamespaces))
                            {
                                var mappedValue = MapModelInList(value, listTypeToMapToo, strict);

                                ((IList)mappedList).Add(mappedValue);
                            }
                            else
                            {
                                ((IList)mappedList).Add(value);
                            }
                        }

                        propertyToMap.SetValue(objectToMapToo, mappedList);
                    }
                    else if (property.Value != null && IsClassMappableModel(property.Value, baseNamespaces))
                    {
                        var mappedValue = MapChildModel(property, propertyToMap, strict);

                        propertyToMap.SetValue(objectToMapToo, mappedValue);
                    } 
                    else
                    {
                        propertyToMap.SetValue(objectToMapToo, property.Value);
                    }
                }
            }

            return objectToMapToo;
        }

        private static void CheckThatPropertiesMatch<T, T2>(IEnumerable<(string ModelPropertyName, object Value, Type PropertyType)> propertiesToMap) where T : class where T2 : new()
        {
            var mapToProperties = typeof(T2).GetProperties()
                .Where(x => x.GetCustomAttribute<MapToName>(false) != null)
                .Select(x => (ModelPropertyName: x.GetCustomAttribute<MapToName>(false)?.Name, PropertyType: x.PropertyType));
            if (mapToProperties.Count() != propertiesToMap.Count())
            {
                throw new MissingPropertyException("Number of mappable properties in models do not match");
            }

            foreach (var propertyToMap in propertiesToMap)
            {
                if (!mapToProperties.Select(x => x.ModelPropertyName).Contains(propertyToMap.ModelPropertyName))
                {
                    throw new PropertyMismatchException("Mappable properties from models do not match");
                }
            }
        }

        private static bool IsClassMappableModel(object value, string[] baseNamespaces)
        {
            var baseNamespaceOfModel = value.GetType().Namespace?.Split('.')[0];
            return value.GetType().IsClass 
                   && (Attribute.GetCustomAttribute(value.GetType(), typeof(IsModel)) != null
                       || baseNamespaces.Contains(baseNamespaceOfModel));
        }

        private object MapModelInList(object value, Type listTypeToMapToo, bool strict)
        {
            var method = typeof(Mapper).GetMethod("Map");
            var mappingMethodWithNewType =
                method.MakeGenericMethod(value.GetType(), listTypeToMapToo);
            var mappedValue = mappingMethodWithNewType.Invoke(this, new[] {value, strict});
            return mappedValue;
        }

        private object MapChildModel((string ModelPropertyName, object Value, Type PropertyType) property,
            PropertyInfo propertyToMap, bool strict)
        {
            var method = typeof(Mapper).GetMethod("Map");
            var mappingMethodWithNewType =
                method.MakeGenericMethod(property.PropertyType, propertyToMap.PropertyType);
            return mappingMethodWithNewType.Invoke(this, new[] {property.Value, strict });
        }
    }
}