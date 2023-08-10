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
  public class DailySchedule {
    /// <summary>
    /// 曜日
    /// </summary>
    /// <value>曜日</value>
    [DataMember(Name="dayOfWeek", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dayOfWeek")]
    public string DayOfWeek { get; set; }

    /// <summary>
    /// 平日休日等判定用 1:平日　２：土曜　3:日曜 4：:祝日
    /// </summary>
    /// <value>平日休日等判定用 1:平日　２：土曜　3:日曜 4：:祝日</value>
    [DataMember(Name="dayType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dayType")]
    public int? DayType { get; set; }

    /// <summary>
    /// 時間単位のスケジュール一覧
    /// </summary>
    /// <value>時間単位のスケジュール一覧</value>
    [DataMember(Name="hourScheduleList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hourScheduleList")]
    public List<InlineResponse20019DataHourScheduleList> HourScheduleList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DailySchedule {\n");
      sb.Append("  DayOfWeek: ").Append(DayOfWeek).Append("\n");
      sb.Append("  DayType: ").Append(DayType).Append("\n");
      sb.Append("  HourScheduleList: ").Append(HourScheduleList).Append("\n");
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
