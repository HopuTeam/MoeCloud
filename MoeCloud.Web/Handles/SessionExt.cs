using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EnterpriseMaterial.Web
{
    public static class SessionExt
    {
        public static void SetModel(this ISession session, string key, object T)
        {
            var TtoString = JsonConvert.SerializeObject(T);
            session.SetString(key, TtoString);
        }

        public static T GetModel<T>(this ISession session, string key) where T : new()
        {
            var mod = session.GetString(key);
            if (mod != null)
            {
                var toModel = JsonConvert.DeserializeObject<T>(mod);
                return toModel;
            }
            return default;
        }
    }
}
