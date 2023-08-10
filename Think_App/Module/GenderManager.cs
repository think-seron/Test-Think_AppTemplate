using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Think_App
{
	public static class GenderManager
	{
		public static Gender ConvertStringToGender(string str)
		{
			Gender result;
			if (str == ConstantManager.String_Gender_Male)
			{
				result = Gender.Male;
			}
			else if (str == ConstantManager.String_Gender_Female)
			{
				result = Gender.Female;
			}
			else if (str == ConstantManager.String_Gender_Other)
			{
				result = Gender.Other;
			}
			else
			{
				// 取得できなかった場合、無難に女性扱いしておく。
				Debug.WriteLine("性別を表す文字列が間違っています！");
				result = Gender.Female;
			}

			return result;
		}

		public static string ConvertGenderToString(Gender gender)
		{
			string result;
			if (gender == Gender.Male)
			{
				result = ConstantManager.String_Gender_Male;
			}
			else if (gender == Gender.Female)
			{
				result = ConstantManager.String_Gender_Female;
			}
			else if (gender == Gender.Other)
			{
				result = ConstantManager.String_Gender_Other;
			}
			else
			{
				// 取得できなかった場合、無難に女性扱いしておく。
				Debug.WriteLine("性別データが間違っています！");
				result = ConstantManager.String_Gender_Female;
			}
			return result;
		}

		public static async Task SaveGenderAsync(Gender gender)
		{
			// 性別を文字列に変換。
			string genderString = ConvertGenderToString(gender);
			// 文字列をストレージに保存。
			await StorageManager.UserDataWriteAsync(ConstantManager.FolderName_Main,
			                                        ConstantManager.FileName_Gender,
			                                        genderString);
		}

		public static async Task<Gender> LoadGenderAsync()
		{
			// ストレージから性別を表す文字列を取得。
			var genderString = await StorageManager.UserDataReadAsync<string>(ConstantManager.FolderName_Main,
			                                                                  ConstantManager.FileName_Gender);
			// 文字列を性別に変換。
			var gender = ConvertStringToGender(genderString);
			// 性別を返す。
			return gender;
		}
	}
}
