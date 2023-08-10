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
  public class InlineResponse2002DataMenuList {
    /// <summary>
    /// メニューID
    /// </summary>
    /// <value>メニューID</value>
    [DataMember(Name="menuId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "menuId")]
    public int? MenuId { get; set; }

    /// <summary>
    /// メニュー名
    /// </summary>
    /// <value>メニュー名</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets IconImage
    /// </summary>
    [DataMember(Name="iconImage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "iconImage")]
    public InlineResponse2001DataList IconImage { get; set; }

    /// <summary>
    /// 通知情報
    /// </summary>
    /// <value>通知情報</value>
    [DataMember(Name="notification", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "notification")]
    public bool? Notification { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse2002DataMenuList {\n");
      sb.Append("  MenuId: ").Append(MenuId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  IconImage: ").Append(IconImage).Append("\n");
      sb.Append("  Notification: ").Append(Notification).Append("\n");
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
