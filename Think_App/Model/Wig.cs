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
  public class Wig {
    /// <summary>
    /// 画像拡大率
    /// </summary>
    /// <value>画像拡大率</value>
    [DataMember(Name="scale", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scale")]
    public decimal? Scale { get; set; }

    /// <summary>
    /// 顔の中心点からのXの移動距離 画像のwhidthを1.0とする 
    /// </summary>
    /// <value>顔の中心点からのXの移動距離 画像のwhidthを1.0とする </value>
    [DataMember(Name="shiftX", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shiftX")]
    public decimal? ShiftX { get; set; }

    /// <summary>
    /// 顔の中心点からのYの移動距離 画像のheightを1.0とする 
    /// </summary>
    /// <value>顔の中心点からのYの移動距離 画像のheightを1.0とする </value>
    [DataMember(Name="shiftY", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "shiftY")]
    public decimal? ShiftY { get; set; }

    /// <summary>
    /// Gets or Sets Image
    /// </summary>
    [DataMember(Name="image", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "image")]
    public InlineResponse2001DataList Image { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Wig {\n");
      sb.Append("  Scale: ").Append(Scale).Append("\n");
      sb.Append("  ShiftX: ").Append(ShiftX).Append("\n");
      sb.Append("  ShiftY: ").Append(ShiftY).Append("\n");
      sb.Append("  Image: ").Append(Image).Append("\n");
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
