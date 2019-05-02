using System.Runtime.InteropServices.ComTypes;

namespace MappingMadeEasy
{
    public interface IMapper
    {
        T2 Map<T, T2>(T objectWithValues)
            where T : class
            where T2 : new();
    }
}