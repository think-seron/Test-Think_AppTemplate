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
  public class Notice {
    /// <summary>
    /// お知らせID
    /// </summary>
    /// <value>お知らせID</value>
    [DataMember(Name="noticeId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "noticeId")]
    public int? NoticeId { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    /// <value>タイトル</value>
    [DataMember(Name="title", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    /// <summary>
    /// お知らせ概要
    /// </summary>
    /// <value>お知らせ概要</value>
    [DataMember(Name="summary", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "summary")]
    public string Summary { get; set; }

    /// <summary>
    /// お知らせ内容
    /// </summary>
    /// <value>お知らせ内容</value>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// 既読かどうか
    /// </summary>
    /// <value>既読かどうか</value>
    [DataMember(Name="isRead", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isRead")]
    public bool? IsRead { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Notice {\n");
      sb.Append("  NoticeId: ").Append(NoticeId).Append("\n");
      sb.Append("  Title: ").Append(Title).Append("\n");
      sb.Append("  Summary: ").Append(Summary).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  IsRead: ").Append(IsRead).Append("\n");
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
