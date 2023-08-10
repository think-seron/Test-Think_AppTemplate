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
  public class MessageDetail {
    /// <summary>
    /// メッセージID
    /// </summary>
    /// <value>メッセージID</value>
    [DataMember(Name="messageId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageId")]
    public int? MessageId { get; set; }

    /// <summary>
    /// メッセージ
    /// </summary>
    /// <value>メッセージ</value>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// メッセージ種別(1=メッセージ,2=画像)
    /// </summary>
    /// <value>メッセージ種別(1=メッセージ,2=画像)</value>
    [DataMember(Name="messageType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageType")]
    public int? MessageType { get; set; }

    /// <summary>
    /// 画像へのフルパス
    /// </summary>
    /// <value>画像へのフルパス</value>
    [DataMember(Name="image", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "image")]
    public string Image { get; set; }

    /// <summary>
    /// 既読かどうか
    /// </summary>
    /// <value>既読かどうか</value>
    [DataMember(Name="isRead", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isRead")]
    public bool? IsRead { get; set; }

    /// <summary>
    /// サロンのメッセージかどうか
    /// </summary>
    /// <value>サロンのメッセージかどうか</value>
    [DataMember(Name="isSalon", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isSalon")]
    public bool? IsSalon { get; set; }

    /// <summary>
    /// 投稿日時
    /// </summary>
    /// <value>投稿日時</value>
    [DataMember(Name="date", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "date")]
    public string Date { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MessageDetail {\n");
      sb.Append("  MessageId: ").Append(MessageId).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
      sb.Append("  MessageType: ").Append(MessageType).Append("\n");
      sb.Append("  Image: ").Append(Image).Append("\n");
      sb.Append("  IsRead: ").Append(IsRead).Append("\n");
      sb.Append("  IsSalon: ").Append(IsSalon).Append("\n");
      sb.Append("  Date: ").Append(Date).Append("\n");
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
