using Gflower.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Gflower.Common
{


    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SaveSessionData<T>(string key, T data)
        {
            JsonSerializer.Serialize(data);
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonSerializer.Serialize(data));
        }

        public T GetSessionData<T>(string key)
        {
            var data = _httpContextAccessor.HttpContext.Session.GetString(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }
    }
}