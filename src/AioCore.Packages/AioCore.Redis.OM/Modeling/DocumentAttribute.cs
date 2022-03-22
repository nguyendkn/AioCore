namespace AioCore.Redis.OM.Modeling
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DocumentAttribute : Attribute
    {
        private static readonly Dictionary<string, IIdGenerationStrategy> IdGenerationStrategies = new()
        {
            {nameof(GuidGenerationStrategy), new GuidGenerationStrategy()},
        };


        public StorageType StorageType { get; set; } = StorageType.Hash;


        public string IdGenerationStrategyName { get; set; } = nameof(GuidGenerationStrategy);


        public string[] Prefixes { get; set; } = Array.Empty<string>();


        public string? IndexName { get; set; }


        public string? Language { get; set; }


        public string? LanguageField { get; set; }


        public string? Filter { get; set; }


        internal IIdGenerationStrategy IdGenerationStrategy => IdGenerationStrategies[IdGenerationStrategyName];


        public static void RegisterIdGenerationStrategy(string strategyName, IIdGenerationStrategy strategy)
        {
            IdGenerationStrategies.Add(strategyName, strategy);
        }
    }
}