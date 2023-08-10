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
  public class InlineResponse20026Data {
    /// <summary>
    /// Gets or Sets MessageList
    /// </summary>
    [DataMember(Name="messageList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageList")]
    public List<InlineResponse20026DataMessageList> MessageList { get; set; }

    /// <summary>
    /// 最後のメッセージかどうか
    /// </summary>
    /// <value>最後のメッセージかどうか</value>
    [DataMember(Name="isLast", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isLast")]
    public bool? IsLast { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20026Data {\n");
      sb.Append("  MessageList: ").Append(MessageList).Append("\n");
      sb.Append("  IsLast: ").Append(IsLast).Append("\n");
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
