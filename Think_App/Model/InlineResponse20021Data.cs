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
  public class InlineResponse20021Data {
    /// <summary>
    /// 予約ID
    /// </summary>
    /// <value>予約ID</value>
    [DataMember(Name="reservationId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "reservationId")]
    public string ReservationId { get; set; }

    /// <summary>
    /// シップスストアID
    /// </summary>
    /// <value>シップスストアID</value>
    [DataMember(Name="sipssStoreId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sipssStoreId")]
    public string SipssStoreId { get; set; }

    /// <summary>
    /// シップスカンパニーID
    /// </summary>
    /// <value>シップスカンパニーID</value>
    [DataMember(Name="sipssCompanyId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sipssCompanyId")]
    public string SipssCompanyId { get; set; }

    /// <summary>
    /// 予約時間
    /// </summary>
    /// <value>予約時間</value>
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
    /// 電話番号(半角ハイフン有り)
    /// </summary>
    /// <value>電話番号(半角ハイフン有り)</value>
    [DataMember(Name="tel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tel")]
    public string Tel { get; set; }

    /// <summary>
    /// スタッフネーム
    /// </summary>
    /// <value>スタッフネーム</value>
    [DataMember(Name="staffName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "staffName")]
    public string StaffName { get; set; }

    /// <summary>
    /// メニューネーム
    /// </summary>
    /// <value>メニューネーム</value>
    [DataMember(Name="menuName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "menuName")]
    public string MenuName { get; set; }

    /// <summary>
    /// クーポン利用時の名前
    /// </summary>
    /// <value>クーポン利用時の名前</value>
    [DataMember(Name="couponName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "couponName")]
    public string CouponName { get; set; }

    /// <summary>
    /// ユーザーメモ
    /// </summary>
    /// <value>ユーザーメモ</value>
    [DataMember(Name="memo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "memo")]
    public string Memo { get; set; }

    /// <summary>
    /// 予約元
    /// </summary>
    /// <value>予約元</value>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public string Source { get; set; }

    /// <summary>
    /// ソース
    /// </summary>
    /// <value>ソース</value>
    [DataMember(Name="canCancelFlag", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "canCancelFlag")]
    public bool? CanCancelFlag { get; set; }

    /// <summary>
    /// キャンセル可能な時間
    /// </summary>
    /// <value>キャンセル可能な時間</value>
    [DataMember(Name="canCancelDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "canCancelDate")]
    public string CanCancelDate { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20021Data {\n");
      sb.Append("  ReservationId: ").Append(ReservationId).Append("\n");
      sb.Append("  SipssStoreId: ").Append(SipssStoreId).Append("\n");
      sb.Append("  SipssCompanyId: ").Append(SipssCompanyId).Append("\n");
      sb.Append("  DateStr: ").Append(DateStr).Append("\n");
      sb.Append("  SalonName: ").Append(SalonName).Append("\n");
      sb.Append("  Tel: ").Append(Tel).Append("\n");
      sb.Append("  StaffName: ").Append(StaffName).Append("\n");
      sb.Append("  MenuName: ").Append(MenuName).Append("\n");
      sb.Append("  CouponName: ").Append(CouponName).Append("\n");
      sb.Append("  Memo: ").Append(Memo).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  CanCancelFlag: ").Append(CanCancelFlag).Append("\n");
      sb.Append("  CanCancelDate: ").Append(CanCancelDate).Append("\n");
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
