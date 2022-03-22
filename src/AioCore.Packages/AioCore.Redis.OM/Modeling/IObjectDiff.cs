namespace AioCore.Redis.OM.Modeling
{
    internal interface IObjectDiff
    {
        string Script { get; }

        IEnumerable<string> SerializeScriptArgs();
    }
}