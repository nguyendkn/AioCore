namespace AioCore.Redis.OM
{
    public class GuidGenerationStrategy : IIdGenerationStrategy
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}