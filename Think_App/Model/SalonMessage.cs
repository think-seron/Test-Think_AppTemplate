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
  public class SalonMessage {
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
    [DataMember(Name="salonName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "salonName")]
    public string SalonName { get; set; }

    /// <summary>
    /// メッセージ履歴が存在するかどうか
    /// </summary>
    /// <value>メッセージ履歴が存在するかどうか</value>
    [DataMember(Name="messageExists", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "messageExists")]
    public bool? MessageExists { get; set; }

    /// <summary>
    /// 未読メッセージが存在するかどうか
    /// </summary>
    /// <value>未読メッセージが存在するかどうか</value>
    [DataMember(Name="unreadExists", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unreadExists")]
    public bool? UnreadExists { get; set; }

    /// <summary>
    /// 最新メッセージ(写真の場合「写真」)
    /// </summary>
    /// <value>最新メッセージ(写真の場合「写真」)</value>
    [DataMember(Name="newMessage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newMessage")]
    public string NewMessage { get; set; }

    /// <summary>
    /// 最新メッセージ投稿日時
    /// </summary>
    /// <value>最新メッセージ投稿日時</value>
    [DataMember(Name="newMessageCreated", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newMessageCreated")]
    public string NewMessageCreated { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SalonMessage {\n");
      sb.Append("  SalonId: ").Append(SalonId).Append("\n");
      sb.Append("  SalonName: ").Append(SalonName).Append("\n");
      sb.Append("  MessageExists: ").Append(MessageExists).Append("\n");
      sb.Append("  UnreadExists: ").Append(UnreadExists).Append("\n");
      sb.Append("  NewMessage: ").Append(NewMessage).Append("\n");
      sb.Append("  NewMessageCreated: ").Append(NewMessageCreated).Append("\n");
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
