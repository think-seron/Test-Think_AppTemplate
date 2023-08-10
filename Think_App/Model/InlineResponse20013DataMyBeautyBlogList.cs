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
  public class InlineResponse20013DataMyBeautyBlogList {
    /// <summary>
    /// My美LogID
    /// </summary>
    /// <value>My美LogID</value>
    [DataMember(Name="myBeautyBlogId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "myBeautyBlogId")]
    public int? MyBeautyBlogId { get; set; }

    /// <summary>
    /// カテゴリ
    /// </summary>
    /// <value>カテゴリ</value>
    [DataMember(Name="category", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "category")]
    public int? Category { get; set; }

    /// <summary>
    /// Gets or Sets ThumbnailImage
    /// </summary>
    [DataMember(Name="thumbnailImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbnailImage")]
    public InlineResponse2001DataList ThumbnailImage { get; set; }

    /// <summary>
    /// 施術日
    /// </summary>
    /// <value>施術日</value>
    [DataMember(Name="date", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "date")]
    public string Date { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    /// <value>タイトル</value>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// ユーザ入力文字列
    /// </summary>
    /// <value>ユーザ入力文字列</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20013DataMyBeautyBlogList {\n");
      sb.Append("  MyBeautyBlogId: ").Append(MyBeautyBlogId).Append("\n");
      sb.Append("  Category: ").Append(Category).Append("\n");
      sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
      sb.Append("  Date: ").Append(Date).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
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
