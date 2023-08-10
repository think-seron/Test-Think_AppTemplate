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
  public class InlineResponse20018Data {
    /// <summary>
    /// クーポン
    /// </summary>
    /// <value>クーポン</value>
    [DataMember(Name="couponList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "couponList")]
    public List<InlineResponse20016DataList> CouponList { get; set; }

    /// <summary>
    /// 通常メニュー
    /// </summary>
    /// <value>通常メニュー</value>
    [DataMember(Name="salonMenuList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonMenuList")]
    public List<InlineResponse20018DataSalonMenuList> SalonMenuList { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20018Data {\n");
      sb.Append("  CouponList: ").Append(CouponList).Append("\n");
      sb.Append("  SalonMenuList: ").Append(SalonMenuList).Append("\n");
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
