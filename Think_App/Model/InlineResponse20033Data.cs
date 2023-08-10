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
  public class InlineResponse20033Data {
    /// <summary>
    /// ニュースの未読がまだあるかどうか
    /// </summary>
    /// <value>ニュースの未読がまだあるかどうか</value>
    [DataMember(Name="newsNotification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newsNotification")]
    public bool? NewsNotification { get; set; }

    /// <summary>
    /// クーポンの未読がまだあるかどうか
    /// </summary>
    /// <value>クーポンの未読がまだあるかどうか</value>
    [DataMember(Name="couponNotification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "couponNotification")]
    public bool? CouponNotification { get; set; }

    /// <summary>
    /// メッセージの未読がまだあるかどうか
    /// </summary>
    /// <value>メッセージの未読がまだあるかどうか</value>
    [DataMember(Name="messageNotification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageNotification")]
    public bool? MessageNotification { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20033Data {\n");
      sb.Append("  NewsNotification: ").Append(NewsNotification).Append("\n");
      sb.Append("  CouponNotification: ").Append(CouponNotification).Append("\n");
      sb.Append("  MessageNotification: ").Append(MessageNotification).Append("\n");
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
