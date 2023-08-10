using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class Home
  {
        /// <summary>
        /// Gets or Sets HomeSalonInfo
        /// </summary>
        [DataMember(Name = "homeSalonInfo", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "homeSalonInfo")]
        public InlineResponse2002DataHomeSalonInfo HomeSalonInfo { get; set; }

        /// <summary>
        /// 予約中情報
        /// </summary>
        /// <value>予約中情報</value>
        [DataMember(Name = "reservationList", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "reservationList")]
        public List<InlineResponse2002DataReservationList> ReservationList { get; set; }

        /// <summary>
        /// メニュー情報
        /// </summary>
        /// <value>メニュー情報</value>
        [DataMember(Name = "menuList", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "menuList")]
        public List<InlineResponse2002DataMenuList> MenuList { get; set; }

        /// <summary>
        /// スライド画像
        /// </summary>
        /// <value>スライド画像</value>
        [DataMember(Name = "slideList", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "slideList")]
        public List<InlineResponse2001DataList> SlideList { get; set; }

        /// <summary>
        /// 登録サロン数
        /// </summary>
        /// <value>登録サロン数</value>
        [DataMember(Name = "salonCount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "salonCount")]
        public int? SalonCount { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Home {\n");
            sb.Append("  HomeSalonInfo: ").Append(HomeSalonInfo).Append("\n");
            sb.Append("  ReservationList: ").Append(ReservationList).Append("\n");
            sb.Append("  MenuList: ").Append(MenuList).Append("\n");
            sb.Append("  SlideList: ").Append(SlideList).Append("\n");
            sb.Append("  SalonCount: ").Append(SalonCount).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
