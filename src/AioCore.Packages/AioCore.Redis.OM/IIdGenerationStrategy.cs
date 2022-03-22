namespace AioCore.Redis.OM
{
    public interface IIdGenerationStrategy
    {
        string GenerateId();
    }
}