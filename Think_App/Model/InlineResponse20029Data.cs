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
  public class InlineResponse20029Data {
    /// <summary>
    /// ユーザー名
    /// </summary>
    /// <value>ユーザー名</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// ユーザー名のフリガナ
    /// </summary>
    /// <value>ユーザー名のフリガナ</value>
    [DataMember(Name="phonetic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "phonetic")]
    public string Phonetic { get; set; }

    /// <summary>
    /// 電話番号(半角ハイフン有り)
    /// </summary>
    /// <value>電話番号(半角ハイフン有り)</value>
    [DataMember(Name="tel", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tel")]
    public string Tel { get; set; }

    /// <summary>
    /// メールアドレス
    /// </summary>
    /// <value>メールアドレス</value>
    [DataMember(Name="mail", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mail")]
    public string Mail { get; set; }

    /// <summary>
    /// 1=女性 2=男性 
    /// </summary>
    /// <value>1=女性 2=男性 </value>
    [DataMember(Name="sex", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sex")]
    public int? Sex { get; set; }

    /// <summary>
    /// デバイスID
    /// </summary>
    /// <value>デバイスID</value>
    [DataMember(Name="deviceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deviceId")]
    public string DeviceId { get; set; }

    /// <summary>
    /// 引き継ぎID
    /// </summary>
    /// <value>引き継ぎID</value>
    [DataMember(Name="transferId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transferId")]
    public string TransferId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20029Data {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Phonetic: ").Append(Phonetic).Append("\n");
      sb.Append("  Tel: ").Append(Tel).Append("\n");
      sb.Append("  Mail: ").Append(Mail).Append("\n");
      sb.Append("  Sex: ").Append(Sex).Append("\n");
      sb.Append("  DeviceId: ").Append(DeviceId).Append("\n");
      sb.Append("  TransferId: ").Append(TransferId).Append("\n");
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
