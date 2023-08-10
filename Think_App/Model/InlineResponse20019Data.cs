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
  public class InlineResponse20019Data {
    /// <summary>
    /// 指名スタッフのスケジュール
    /// </summary>
    /// <value>指名スタッフのスケジュール</value>
    [DataMember(Name="staffDailyScheduleList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "staffDailyScheduleList")]
    public List<InlineResponse20019DataStaffDailyScheduleList> StaffDailyScheduleList { get; set; }

    /// <summary>
    /// サロンのスケジュール
    /// </summary>
    /// <value>サロンのスケジュール</value>
    [DataMember(Name="salonDailyScheduleList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonDailyScheduleList")]
    public List<InlineResponse20019DataStaffDailyScheduleList> SalonDailyScheduleList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20019Data {\n");
      sb.Append("  StaffDailyScheduleList: ").Append(StaffDailyScheduleList).Append("\n");
      sb.Append("  SalonDailyScheduleList: ").Append(SalonDailyScheduleList).Append("\n");
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
