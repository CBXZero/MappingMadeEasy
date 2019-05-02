namespace MappingMadeEasy.Standard.Nuget.ModelHelper
{
    public class ModelCompareResult
    {
        public ModelCompareResult(bool isEquivalent, string resultMessage)
        {
            IsEquivalent = isEquivalent;
            ResultMessage = resultMessage;
        }
        public bool IsEquivalent { get; set; }
        public string ResultMessage { get; set; }
    }
}