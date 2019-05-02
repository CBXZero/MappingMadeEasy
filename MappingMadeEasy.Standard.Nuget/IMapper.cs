namespace MappingMadeEasy.Standard.Nuget
{
    public interface IMapper
    {
        T2 Map<T, T2>(T objectWithValues, bool strict)
            where T : class
            where T2 : new();
    }
}