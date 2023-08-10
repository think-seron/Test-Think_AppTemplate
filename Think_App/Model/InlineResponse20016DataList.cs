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
  public class InlineResponse20016DataList {
    /// <summary>
    /// クーポンID
    /// </summary>
    /// <value>クーポンID</value>
    [DataMember(Name="couponId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "couponId")]
    public int? CouponId { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    /// <value>タイトル</value>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// サロン名
    /// </summary>
    /// <value>サロン名</value>
    [DataMember(Name="salonName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonName")]
    public string SalonName { get; set; }

    /// <summary>
    /// 施術内容
    /// </summary>
    /// <value>施術内容</value>
    [DataMember(Name="treatment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "treatment")]
    public string Treatment { get; set; }

    /// <summary>
    /// 割引内容
    /// </summary>
    /// <value>割引内容</value>
    [DataMember(Name="discount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discount")]
    public string Discount { get; set; }

    /// <summary>
    /// 利用条件
    /// </summary>
    /// <value>利用条件</value>
    [DataMember(Name="condition", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "condition")]
    public string Condition { get; set; }

    /// <summary>
    /// 提示条件
    /// </summary>
    /// <value>提示条件</value>
    [DataMember(Name="timing", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timing")]
    public string Timing { get; set; }

    /// <summary>
    /// クーポン説明
    /// </summary>
    /// <value>クーポン説明</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// メニューとの紐付け
    /// </summary>
    /// <value>メニューとの紐付け</value>
    [DataMember(Name="isAssociated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isAssociated")]
    public bool? IsAssociated { get; set; }

    /// <summary>
    /// Gets or Sets ThumbnailImage
    /// </summary>
    [DataMember(Name="thumbnailImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbnailImage")]
    public InlineResponse2001DataList ThumbnailImage { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20016DataList {\n");
      sb.Append("  CouponId: ").Append(CouponId).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  SalonName: ").Append(SalonName).Append("\n");
      sb.Append("  Treatment: ").Append(Treatment).Append("\n");
      sb.Append("  Discount: ").Append(Discount).Append("\n");
      sb.Append("  Condition: ").Append(Condition).Append("\n");
      sb.Append("  Timing: ").Append(Timing).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  IsAssociated: ").Append(IsAssociated).Append("\n");
      sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
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
