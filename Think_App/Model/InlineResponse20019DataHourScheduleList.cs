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
  public class InlineResponse20019DataHourScheduleList {
    /// <summary>
    /// 日時
    /// </summary>
    /// <value>日時</value>
    [DataMember(Name="date", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "date")]
    public string Date { get; set; }

    /// <summary>
    /// 予約可能かどうか
    /// </summary>
    /// <value>予約可能かどうか</value>
    [DataMember(Name="canReservation", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "canReservation")]
    public bool? CanReservation { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20019DataHourScheduleList {\n");
      sb.Append("  Date: ").Append(Date).Append("\n");
      sb.Append("  CanReservation: ").Append(CanReservation).Append("\n");
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
