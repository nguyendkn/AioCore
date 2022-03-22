namespace AioCore.Redis.OM.Modeling
{
    internal class DelDiff : IObjectDiff
    {
        public string Script => nameof(Scripts.Unlink);


        public IEnumerable<string> SerializeScriptArgs()
        {
            return Array.Empty<string>();
        }
    }
}