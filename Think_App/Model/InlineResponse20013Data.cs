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
  public class InlineResponse20013Data {
    /// <summary>
    /// 獲得ポイント
    /// </summary>
    /// <value>獲得ポイント</value>
    [DataMember(Name="pointStr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pointStr")]
    public string PointStr { get; set; }

    /// <summary>
    /// 施術履歴
    /// </summary>
    /// <value>施術履歴</value>
    [DataMember(Name="treatmentHistoryList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "treatmentHistoryList")]
    public List<InlineResponse20013DataTreatmentHistoryList> TreatmentHistoryList { get; set; }

    /// <summary>
    /// My美Log
    /// </summary>
    /// <value>My美Log</value>
    [DataMember(Name="myBeautyBlogList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "myBeautyBlogList")]
    public List<InlineResponse20013DataMyBeautyBlogList> MyBeautyBlogList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20013Data {\n");
      sb.Append("  PointStr: ").Append(PointStr).Append("\n");
      sb.Append("  TreatmentHistoryList: ").Append(TreatmentHistoryList).Append("\n");
      sb.Append("  MyBeautyBlogList: ").Append(MyBeautyBlogList).Append("\n");
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
