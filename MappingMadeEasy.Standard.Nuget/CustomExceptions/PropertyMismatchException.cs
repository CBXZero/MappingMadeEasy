using System;

namespace MappingMadeEasy.Standard.Nuget.CustomExceptions
{
    public class PropertyMismatchException : Exception
    {
        public PropertyMismatchException(string message) : base(message)
        {

        }
    }
}