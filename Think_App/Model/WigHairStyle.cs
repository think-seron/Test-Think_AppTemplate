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
  public class WigHairStyle {
    /// <summary>
    /// ヘアスタイル名
    /// </summary>
    /// <value>ヘアスタイル名</value>
    [DataMember(Name="hairStyle", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hairStyle")]
    public string HairStyle { get; set; }

    /// <summary>
    /// ウィッグ一覧
    /// </summary>
    /// <value>ウィッグ一覧</value>
    [DataMember(Name="wigList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "wigList")]
    public List<InlineResponse20022DataWigList> WigList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class WigHairStyle {\n");
      sb.Append("  HairStyle: ").Append(HairStyle).Append("\n");
      sb.Append("  WigList: ").Append(WigList).Append("\n");
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
