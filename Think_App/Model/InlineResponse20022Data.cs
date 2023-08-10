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
  public class InlineResponse20022Data {
    /// <summary>
    /// 女性ウィッグ一覧
    /// </summary>
    /// <value>女性ウィッグ一覧</value>
    [DataMember(Name="womanWigList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "womanWigList")]
    public List<InlineResponse20022DataWomanWigList> WomanWigList { get; set; }

    /// <summary>
    /// 男性ウィッグ一覧
    /// </summary>
    /// <value>男性ウィッグ一覧</value>
    [DataMember(Name="manWigList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "manWigList")]
    public List<InlineResponse20022DataWomanWigList> ManWigList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20022Data {\n");
      sb.Append("  WomanWigList: ").Append(WomanWigList).Append("\n");
      sb.Append("  ManWigList: ").Append(ManWigList).Append("\n");
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
