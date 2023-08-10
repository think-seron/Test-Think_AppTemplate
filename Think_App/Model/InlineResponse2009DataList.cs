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
    public class InlineResponse2009DataList
    {
        /// <summary>
        /// スタッフID
        /// </summary>
        /// <value>スタッフID</value>
        [DataMember(Name = "staffId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "staffId")]
        public int? StaffId { get; set; }

        /// <summary>
        /// スタッフ名
        /// </summary>
        /// <value>スタッフ名</value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// スタッフ名(フリガナ)
        /// </summary>
        /// <value>スタッフ名(フリガナ)</value>
        [DataMember(Name = "kana", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "kana")]
        public string Kana { get; set; }

        /// <summary>
        /// スタッフ経歴
        /// </summary>
        /// <value>スタッフ経歴</value>
        [DataMember(Name = "career", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "career")]
        public string Career { get; set; }

        /// <summary>
        /// スタッフ概要
        /// </summary>
        /// <value>スタッフ概要</value>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// お気に入りのスタッフかどうか
        /// </summary>
        /// <value>お気に入りのスタッフかどうか</value>
        [DataMember(Name = "isFavorite", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "isFavorite")]
        public bool? IsFavorite { get; set; }

        /// <summary>
        /// Gets or Sets ThumbnailImage
        /// </summary>
        [DataMember(Name = "thumbnailImage", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "thumbnailImage")]
        public InlineResponse2001DataList ThumbnailImage { get; set; }
        /// <summary>
        /// 予約可能なスタッフかどうか
        /// </summary>
        /// <value>予約可能なスタッフかどうか</value>
        [DataMember(Name = "canReservate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "canReservate")]
        public bool? CanReservate { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class InlineResponse2009DataList {\n");
            sb.Append("  StaffId: ").Append(StaffId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Kana: ").Append(Kana).Append("\n");
            sb.Append("  Career: ").Append(Career).Append("\n");
            sb.Append("  Summary: ").Append(Summary).Append("\n");
            sb.Append("  IsFavorite: ").Append(IsFavorite).Append("\n");
            sb.Append("  CanReservate: ").Append(CanReservate).Append("\n");
            sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
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
