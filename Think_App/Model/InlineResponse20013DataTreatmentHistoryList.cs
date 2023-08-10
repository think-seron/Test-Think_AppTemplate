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
  public class InlineResponse20013DataTreatmentHistoryList {
    /// <summary>
    /// 施術履歴ID
    /// </summary>
    /// <value>施術履歴ID</value>
    [DataMember(Name="treatmentHistoryId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "treatmentHistoryId")]
    public string TreatmentHistoryId { get; set; }

    /// <summary>
    /// サロン名
    /// </summary>
    /// <value>サロン名</value>
    [DataMember(Name="salonName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonName")]
    public string SalonName { get; set; }

    /// <summary>
    /// 施術日
    /// </summary>
    /// <value>施術日</value>
    [DataMember(Name="dateStr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateStr")]
    public string DateStr { get; set; }

    /// <summary>
    /// Gets or Sets ThumbnailImage
    /// </summary>
    [DataMember(Name="thumbnailImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbnailImage")]
    public List<InlineResponse20013DataThumbnailImage> ThumbnailImage { get; set; }

    /// <summary>
    /// スタイリスト名
    /// </summary>
    /// <value>スタイリスト名</value>
    [DataMember(Name="stylist", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stylist")]
    public string Stylist { get; set; }

    /// <summary>
    /// 施術内容
    /// </summary>
    /// <value>施術内容</value>
    [DataMember(Name="treatmentDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "treatmentDescription")]
    public string TreatmentDescription { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20013DataTreatmentHistoryList {\n");
      sb.Append("  TreatmentHistoryId: ").Append(TreatmentHistoryId).Append("\n");
      sb.Append("  SalonName: ").Append(SalonName).Append("\n");
      sb.Append("  DateStr: ").Append(DateStr).Append("\n");
      sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
      sb.Append("  Stylist: ").Append(Stylist).Append("\n");
      sb.Append("  TreatmentDescription: ").Append(TreatmentDescription).Append("\n");
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
