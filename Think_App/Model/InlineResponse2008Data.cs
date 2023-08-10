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
  public class InlineResponse2008Data {
    /// <summary>
    /// 女性ヘアスタイル情報
    /// </summary>
    /// <value>女性ヘアスタイル情報</value>
    [DataMember(Name="womanHairStyleList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "womanHairStyleList")]
    public List<InlineResponse2002DataHomeSalonInfoWomanHairStyleList> WomanHairStyleList { get; set; }

    /// <summary>
    /// 男性ヘアスタイル情報
    /// </summary>
    /// <value>男性ヘアスタイル情報</value>
    [DataMember(Name="manHairStyleList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "manHairStyleList")]
    public List<InlineResponse2002DataHomeSalonInfoWomanHairStyleList> ManHairStyleList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse2008Data {\n");
      sb.Append("  WomanHairStyleList: ").Append(WomanHairStyleList).Append("\n");
      sb.Append("  ManHairStyleList: ").Append(ManHairStyleList).Append("\n");
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
