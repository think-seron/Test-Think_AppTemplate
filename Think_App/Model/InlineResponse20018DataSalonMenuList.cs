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
  public class InlineResponse20018DataSalonMenuList {
    /// <summary>
    /// サロンメニューID
    /// </summary>
    /// <value>サロンメニューID</value>
    [DataMember(Name="salonMenuId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonMenuId")]
    public int? SalonMenuId { get; set; }

    /// <summary>
    /// メニュー概要
    /// </summary>
    /// <value>メニュー概要</value>
    [DataMember(Name="summary", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "summary")]
    public string Summary { get; set; }

    /// <summary>
    /// 施術時間
    /// </summary>
    /// <value>施術時間</value>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public string Duration { get; set; }

    /// <summary>
    /// 価格
    /// </summary>
    /// <value>価格</value>
    [DataMember(Name="price", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "price")]
    public string Price { get; set; }

    /// <summary>
    /// 施術内容
    /// </summary>
    /// <value>施術内容</value>
    [DataMember(Name = "treatment", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "treatment")]
    public string Treatment { get; set; }

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20018DataSalonMenuList {\n");
      sb.Append("  SalonMenuId: ").Append(SalonMenuId).Append("\n");
      sb.Append("  Summary: ").Append(Summary).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  Price: ").Append(Price).Append("\n");
      sb.Append("  Treatment: ").Append(Treatment).Append("\n");
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
