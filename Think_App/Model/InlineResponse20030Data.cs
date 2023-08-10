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
  public class InlineResponse20030Data {
    /// <summary>
    /// 氏名
    /// </summary>
    /// <value>氏名</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// フリガナ
    /// </summary>
    /// <value>フリガナ</value>
    [DataMember(Name="kana", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "kana")]
    public string Kana { get; set; }

    /// <summary>
    /// 電話番号
    /// </summary>
    /// <value>電話番号</value>
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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20030Data {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Kana: ").Append(Kana).Append("\n");
      sb.Append("  Tel: ").Append(Tel).Append("\n");
      sb.Append("  Mail: ").Append(Mail).Append("\n");
      sb.Append("  Sex: ").Append(Sex).Append("\n");
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
