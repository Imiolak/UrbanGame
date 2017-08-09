namespace UrbanGame.Core.Services
{
    public interface IApplicationVariableService
    {
        string GetValueByKey(string key);

        void SetValue(string key, string value);
    }
}
