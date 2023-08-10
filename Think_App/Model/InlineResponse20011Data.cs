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
  public class InlineResponse20011Data {
    /// <summary>
    /// ページインデックス
    /// </summary>
    /// <value>ページインデックス</value>
    [DataMember(Name="index", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "index")]
    public int? Index { get; set; }

    /// <summary>
    /// 最終ページかどうか
    /// </summary>
    /// <value>最終ページかどうか</value>
    [DataMember(Name="isEnd", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isEnd")]
    public bool? IsEnd { get; set; }

    /// <summary>
    /// Gets or Sets List
    /// </summary>
    [DataMember(Name="list", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "list")]
    public List<InlineResponse20011DataList> List { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20011Data {\n");
      sb.Append("  Index: ").Append(Index).Append("\n");
      sb.Append("  IsEnd: ").Append(IsEnd).Append("\n");
      sb.Append("  List: ").Append(List).Append("\n");
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
