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
  public class InlineResponse20028Data {
    /// <summary>
    /// プライバシーポリシーのhtml
    /// </summary>
    /// <value>プライバシーポリシーのhtml</value>
    [DataMember(Name="privacyPolicy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "privacyPolicy")]
    public string PrivacyPolicy { get; set; }

    /// <summary>
    /// 利用規約のhtml
    /// </summary>
    /// <value>利用規約のhtml</value>
    [DataMember(Name="termsOfService", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "termsOfService")]
    public string TermsOfService { get; set; }

    /// <summary>
    /// ライセンスのhtml
    /// </summary>
    /// <value>利用規約のhtml</value>
    [DataMember(Name = "license", EmitDefaultValue = false)]
    [JsonProperty(PropertyName = "license")]
    public string License { get; set; }

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class InlineResponse20028Data {\n");
      sb.Append("  PrivacyPolicy: ").Append(PrivacyPolicy).Append("\n");
      sb.Append("  TermsOfService: ").Append(TermsOfService).Append("\n");
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
