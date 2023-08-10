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
  public class Reservation {
    /// <summary>
    /// 予約ID
    /// </summary>
    /// <value>予約ID</value>
    [DataMember(Name="reservationId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reservationId")]
    public string ReservationId { get; set; }

    /// <summary>
    /// 予約日時
    /// </summary>
    /// <value>予約日時</value>
    [DataMember(Name="dateStr", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dateStr")]
    public string DateStr { get; set; }

    /// <summary>
    /// サロン名
    /// </summary>
    /// <value>サロン名</value>
    [DataMember(Name="salonName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonName")]
    public string SalonName { get; set; }

    /// <summary>
    /// SipssのStoreID
    /// </summary>
    /// <value>SipssのStoreID</value>
    [DataMember(Name="sipssStoreId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sipssStoreId")]
    public string SipssStoreId { get; set; }

    /// <summary>
    /// SipssのCompanyID
    /// </summary>
    /// <value>SipssのCompanyID</value>
    [DataMember(Name="sipssCompanyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sipssCompanyId")]
    public string SipssCompanyId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Reservation {\n");
      sb.Append("  ReservationId: ").Append(ReservationId).Append("\n");
      sb.Append("  DateStr: ").Append(DateStr).Append("\n");
      sb.Append("  SalonName: ").Append(SalonName).Append("\n");
      sb.Append("  SipssStoreId: ").Append(SipssStoreId).Append("\n");
      sb.Append("  SipssCompanyId: ").Append(SipssCompanyId).Append("\n");
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
