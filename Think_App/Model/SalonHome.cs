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
  public class SalonHome {
    /// <summary>
    /// サロンID
    /// </summary>
    /// <value>サロンID</value>
    [DataMember(Name="salonId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonId")]
    public int? SalonId { get; set; }

    /// <summary>
    /// サロン名
    /// </summary>
    /// <value>サロン名</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// 住所
    /// </summary>
    /// <value>住所</value>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    /// <summary>
    /// 電話番号(半角ハイフン有り)
    /// </summary>
    /// <value>電話番号(半角ハイフン有り)</value>
    [DataMember(Name="tel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tel")]
    public string Tel { get; set; }

    /// <summary>
    /// 営業時間
    /// </summary>
    /// <value>営業時間</value>
    [DataMember(Name="businessHours", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "businessHours")]
    public string BusinessHours { get; set; }

    /// <summary>
    /// サロン説明
    /// </summary>
    /// <value>サロン説明</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets ThumbnailImage
    /// </summary>
    [DataMember(Name="thumbnailImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbnailImage")]
    public InlineResponse2001DataList ThumbnailImage { get; set; }

    /// <summary>
    /// お気に入りサロンかどうか
    /// </summary>
    /// <value>お気に入りサロンかどうか</value>
    [DataMember(Name="isFavorite", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isFavorite")]
    public bool? IsFavorite { get; set; }

    /// <summary>
    /// ポイント
    /// </summary>
    /// <value>ポイント</value>
    [DataMember(Name="points", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "points")]
    public List<InlineResponse2002DataHomeSalonInfoPoints> Points { get; set; }

    /// <summary>
    /// Gets or Sets MapInfo
    /// </summary>
    [DataMember(Name="mapInfo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mapInfo")]
    public InlineResponse2002DataHomeSalonInfoMapInfo MapInfo { get; set; }

    /// <summary>
    /// スタッフ情報
    /// </summary>
    /// <value>スタッフ情報</value>
    [DataMember(Name="staffList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "staffList")]
    public List<InlineResponse2002DataHomeSalonInfoStaffList> StaffList { get; set; }

    /// <summary>
    /// お気に入りスタッフの役職+名前
    /// </summary>
    /// <value>お気に入りスタッフの役職+名前</value>
    [DataMember(Name="favoriteStaffName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "favoriteStaffName")]
    public string FavoriteStaffName { get; set; }

    /// <summary>
    /// Gets or Sets FavoriteStaffImage
    /// </summary>
    [DataMember(Name="favoriteStaffImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "favoriteStaffImage")]
    public InlineResponse2001DataList FavoriteStaffImage { get; set; }

    /// <summary>
    /// Gets or Sets SalonImageList
    /// </summary>
    [DataMember(Name="salonImageList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonImageList")]
    public InlineResponse2001DataList SalonImageList { get; set; }

    /// <summary>
    /// お気に入りスタッフのID
    /// </summary>
    /// <value>お気に入りスタッフのID</value>
    [DataMember(Name="favoriteStaffId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "favoriteStaffId")]
    public int? FavoriteStaffId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SalonHome {\n");
      sb.Append("  SalonId: ").Append(SalonId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
      sb.Append("  Tel: ").Append(Tel).Append("\n");
      sb.Append("  BusinessHours: ").Append(BusinessHours).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
      sb.Append("  IsFavorite: ").Append(IsFavorite).Append("\n");
      sb.Append("  Points: ").Append(Points).Append("\n");
      sb.Append("  MapInfo: ").Append(MapInfo).Append("\n");
      sb.Append("  StaffList: ").Append(StaffList).Append("\n");
      sb.Append("  FavoriteStaffName: ").Append(FavoriteStaffName).Append("\n");
      sb.Append("  FavoriteStaffImage: ").Append(FavoriteStaffImage).Append("\n");
      sb.Append("  SalonImageList: ").Append(SalonImageList).Append("\n");
      sb.Append("  FavoriteStaffId: ").Append(FavoriteStaffId).Append("\n");
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
