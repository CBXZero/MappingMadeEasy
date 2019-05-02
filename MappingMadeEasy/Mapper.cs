using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using MappingMadeEasy.CustomDataAttributes;

namespace MappingMadeEasy
{
    public class Mapper : IMapper
    {
        public T2 Map<T, T2>(T objectWithValues)
            where T : class
            where T2 : new()
        {
            var propertiesToMap = typeof(T).GetProperties()
                .Select(x => (ModelPropertyName: x.GetCustomAttribute<MapToName>(false)?.Name, Value: x.GetValue(objectWithValues), PropertyType: x.PropertyType));

            var objectToMapToo = new T2();
            foreach (var property in propertiesToMap)
            {
                foreach (var propertyToMap in typeof(T2).GetProperties().Where(x => x.GetCustomAttribute<MapToName>(false)?.Name == property.ModelPropertyName))
                {
                    if (property.PropertyType.IsClass 
                        && Attribute.GetCustomAttribute(property.PropertyType, typeof(IsModel)) != null
                        && property.Value != null)
                    {
                        var method = typeof(Mapper).GetMethod("Map");
                        var mappingMethodWithNewType =
                            method.MakeGenericMethod(property.PropertyType, propertyToMap.PropertyType);
                        var mappedValue = mappingMethodWithNewType.Invoke(this, new[] {property.Value});
                        propertyToMap.SetValue(objectToMapToo, mappedValue);
                    }
                    else if (typeof(IList).IsAssignableFrom(property.PropertyType)
                                && property.Value != null)
                    {
                        var mappedList = Activator.CreateInstance(propertyToMap.PropertyType);

                        foreach (var value in (IList) property.Value)
                        {
                            var listTypeToMapToo = propertyToMap.PropertyType.GetGenericArguments()[0];
                            if (value.GetType().IsClass &&
                                Attribute.GetCustomAttribute(value.GetType(), typeof(IsModel)) != null)
                            {
                                var method = typeof(Mapper).GetMethod("Map");
                                var mappingMethodWithNewType =
                                    method.MakeGenericMethod(value.GetType(), listTypeToMapToo);
                                var mappedValue = mappingMethodWithNewType.Invoke(this, new[] { value });

                                ((IList) mappedList).Add(mappedValue);
                            }
                            else
                            {
                                ((IList)mappedList).Add(value);
                            }
                        }

                        propertyToMap.SetValue(objectToMapToo, mappedList);
                    }
                    else
                    {
                        propertyToMap.SetValue(objectToMapToo, property.Value);
                    }
                }
            }

            return objectToMapToo;
        }
    }
}