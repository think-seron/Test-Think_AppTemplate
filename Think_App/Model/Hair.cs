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
  public class Hair {
    /// <summary>
    /// 髪型ID
    /// </summary>
    /// <value>髪型ID</value>
    [DataMember(Name="hairId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hairId")]
    public int? HairId { get; set; }

    /// <summary>
    /// スタッフ名
    /// </summary>
    /// <value>スタッフ名</value>
    [DataMember(Name="staffName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "staffName")]
    public string StaffName { get; set; }

    /// <summary>
    /// 髪型説明
    /// </summary>
    /// <value>髪型説明</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

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
      sb.Append("class Hair {\n");
      sb.Append("  HairId: ").Append(HairId).Append("\n");
      sb.Append("  StaffName: ").Append(StaffName).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
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
