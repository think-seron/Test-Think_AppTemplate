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
  public class InlineResponse20024Data {
    /// <summary>
    /// 予約通知(来店1日前)
    /// </summary>
    /// <value>予約通知(来店1日前)</value>
    [DataMember(Name="reserveBeforeDay", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveBeforeDay")]
    public bool? ReserveBeforeDay { get; set; }

    /// <summary>
    /// 予約通知(来店N時間前)
    /// </summary>
    /// <value>予約通知(来店N時間前)</value>
    [DataMember(Name="reserveBeforeHour", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reserveBeforeHour")]
    public bool? ReserveBeforeHour { get; set; }

    /// <summary>
    /// お知らせの更新
    /// </summary>
    /// <value>お知らせの更新</value>
    [DataMember(Name="notice", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notice")]
    public bool? Notice { get; set; }

    /// <summary>
    /// クーポンの追加
    /// </summary>
    /// <value>クーポンの追加</value>
    [DataMember(Name="coupon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "coupon")]
    public bool? Coupon { get; set; }

    /// <summary>
    /// 新規メッセージ
    /// </summary>
    /// <value>新規メッセージ</value>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public bool? Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20024Data {\n");
      sb.Append("  ReserveBeforeDay: ").Append(ReserveBeforeDay).Append("\n");
      sb.Append("  ReserveBeforeHour: ").Append(ReserveBeforeHour).Append("\n");
      sb.Append("  Notice: ").Append(Notice).Append("\n");
      sb.Append("  Coupon: ").Append(Coupon).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
