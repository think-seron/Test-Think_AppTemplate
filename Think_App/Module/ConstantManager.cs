using System;
namespace Think_App
{
	public static class ConstantManager
	{
		// FolderName
		public static readonly string FolderName_Main = "SubFolder";
		public static readonly string FolderName_HairImages = "HairImages";
		public static readonly string FolderName_HairThumbnailImages = "HairThumbnailImages";
		public static readonly string FolderName_EditData = "EditData";
		public static readonly string FolderName_SavedImage = "SavedImage";

		// FileName
		public static readonly string FileName_Gender = "Gender.txt";
		public static readonly string FileName_LastImage = "LastImage.png";
		public static readonly string FileName_Map = "map.png";
		public static readonly string FileName_Tel = "tel.png";

		// Suffix
		public static readonly string Suffix_EditInfo = "_info";
		public static readonly string Suffix_Hair = "_hair";
		public static readonly string Suffix_Face = "_face";
		public static readonly string Suffix_Image = "";

		// 文字列
		public static readonly string String_Gender_Male = "M";
		public static readonly string String_Gender_Female = "F";
		public static readonly string String_Gender_Other = "O";
	}

	// enum定義
	public enum Gender
	{
		Male,
		Female,
		Other
	};

	public enum AppleSignInCredentialState
	{
		Authorized,
		Revoked,
		NotFound,
		Unknown
	}
}
