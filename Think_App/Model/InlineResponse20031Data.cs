using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class InlineResponse20031Data {
    /// <summary>
    /// サーバーに登録されているDeviceToken
    /// </summary>
    /// <value>サーバーに登録されているDeviceToken</value>
    [DataMember(Name="token", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "token")]
    public string Token { get; set; }

    /// <summary>
    /// 0有効 or 1使えない
    /// </summary>
    /// <value>0有効 or 1使えない</value>
    [DataMember(Name="isInvalid", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isInvalid")]
    public int? IsInvalid { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20031Data {\n");
      sb.Append("  Token: ").Append(Token).Append("\n");
      sb.Append("  IsInvalid: ").Append(IsInvalid).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
