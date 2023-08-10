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
    public class Staff
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
        /// 予約可能なスタッフかどうか
        /// </summary>
        /// <value>予約可能なスタッフかどうか</value>
        [DataMember(Name = "canReservate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "canReservate")]
        public bool? CanReservate { get; set; }
        /// <summary>
        /// Gets or Sets ThumbnailImage
        /// </summary>
        [DataMember(Name = "thumbnailImage", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "thumbnailImage")]
        public InlineResponse2001DataList ThumbnailImage { get; set; }

        /// <summary>
        /// スタッフ説明
        /// </summary>
        /// <value>スタッフ説明</value>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 得意なイメージ
        /// </summary>
        /// <value>得意なイメージ</value>
        [DataMember(Name = "goodImagine", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "goodImagine")]
        public string GoodImagine { get; set; }

        /// <summary>
        /// 得意な技術
        /// </summary>
        /// <value>得意な技術</value>
        [DataMember(Name = "goodSkill", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "goodSkill")]
        public string GoodSkill { get; set; }

        /// <summary>
        /// 女性ヘアスタイル情報
        /// </summary>
        /// <value>女性ヘアスタイル情報</value>
        [DataMember(Name = "womanHairStyleList", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "womanHairStyleList")]
        public List<InlineResponse2002DataHomeSalonInfoWomanHairStyleList> WomanHairStyleList { get; set; }

        /// <summary>
        /// 男性ヘアスタイル情報
        /// </summary>
        /// <value>男性ヘアスタイル情報</value>
        [DataMember(Name = "manHairStyleList", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "manHairStyleList")]
        public List<InlineResponse2002DataHomeSalonInfoWomanHairStyleList> ManHairStyleList { get; set; }


        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Staff {\n");
            sb.Append("  StaffId: ").Append(StaffId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Kana: ").Append(Kana).Append("\n");
            sb.Append("  Career: ").Append(Career).Append("\n");
            sb.Append("  Summary: ").Append(Summary).Append("\n");
            sb.Append("  IsFavorite: ").Append(IsFavorite).Append("\n");
            sb.Append("  ThumbnailImage: ").Append(ThumbnailImage).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  GoodImagine: ").Append(GoodImagine).Append("\n");
            sb.Append("  GoodSkill: ").Append(GoodSkill).Append("\n");
            sb.Append("  WomanHairStyleList: ").Append(WomanHairStyleList).Append("\n");
            sb.Append("  ManHairStyleList: ").Append(ManHairStyleList).Append("\n");
            sb.Append("  CanReservate: ").Append(CanReservate).Append("\n");
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
