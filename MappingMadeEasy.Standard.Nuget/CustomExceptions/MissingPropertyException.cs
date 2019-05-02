using System;

namespace MappingMadeEasy.Standard.Nuget.CustomExceptions
{
    public class MissingPropertyException : Exception
    {
        public MissingPropertyException(string message) : base(message)
        {
            
        }
    }
}
