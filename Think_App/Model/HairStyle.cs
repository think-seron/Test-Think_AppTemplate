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
  public class HairStyle {
    /// <summary>
    /// ヘアスタイルID
    /// </summary>
    /// <value>ヘアスタイルID</value>
    [DataMember(Name="hairStyleId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hairStyleId")]
    public int? HairStyleId { get; set; }

    /// <summary>
    /// ヘアスタイル名
    /// </summary>
    /// <value>ヘアスタイル名</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets ThumbnailImage
    /// </summary>
    [DataMember(Name="thumbnailImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbnailImage")]
    public InlineResponse2001DataList ThumbnailImage { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HairStyle {\n");
      sb.Append("  HairStyleId: ").Append(HairStyleId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
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
