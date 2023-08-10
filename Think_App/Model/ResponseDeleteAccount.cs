using System;
using Newtonsoft.Json;
namespace Think_App
{
    public class ResponseDeleteAccount
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
