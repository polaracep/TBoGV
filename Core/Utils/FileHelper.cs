using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public static class FileHelper
{
	public static Dictionary<SaveType, string> folderPath = new Dictionary<SaveType, string>
	{
		{SaveType.USER, "userSave/" },
		{SaveType.AUTO, "autoSave/" },
		{SaveType.GENERIC, "./" },
	};

	private static JsonSerializerSettings settings = new JsonSerializerSettings
	{
		Formatting = Formatting.Indented,
		Converters = { new StringEnumConverter() }
	};
	public static void Save<T>(string filePath, T data, SaveType saveType)
	{
		/*
				try
				{
				*/
		if (!Directory.Exists(folderPath[saveType]))
		{
			Directory.CreateDirectory(folderPath[saveType]);
		}

		string json = JsonConvert.SerializeObject(data, settings);
		if (saveType == SaveType.AUTO)
		{
			foreach (var file in Directory.EnumerateFiles(folderPath[saveType]))
			{
				File.Delete(file);
			}
			string hash = json.Hash();
			filePath = "tbogv_" + hash + ".json";
		}

		string fullPath = folderPath[saveType] + filePath;

		File.WriteAllText(fullPath, json, Encoding.UTF8);
		/*
	}
	catch (Exception e)
	{
		Console.WriteLine($"Error saving file {filePath}: {e.Message}");
	}
	*/
	}

	static string Hash(this string input)
		=> Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(input)));

	public static T Load<T>(string filePath, SaveType saveType)
	{
		try
		{
			if (saveType == SaveType.AUTO)
			{
				foreach (string fileName in Directory.EnumerateFiles(folderPath[saveType]))
				{
					if (!fileName.Contains("tbogv_"))
						File.Delete(fileName);

					// we found a file that could be a save
					string nameChecksum = fileName.Split('/').Last().Substring(6);
					nameChecksum = nameChecksum.Remove(nameChecksum.Length - 5);

					string json = File.ReadAllText(fileName, Encoding.UTF8);

					if (!((bool?)Settings.SpeedrunMode.Value ?? false) || nameChecksum == json.Trim().Hash())
					{
						JsonSerializerSettings settings = new JsonSerializerSettings
						{
							Formatting = Formatting.Indented,
							Converters = { new StringEnumConverter() }
						};

						return JsonConvert.DeserializeObject<T>(json, settings);
					}
					else
						File.Delete(fileName);


				}
			}

			if (File.Exists(folderPath[saveType] + filePath))
			{
				string json = File.ReadAllText(folderPath[saveType] + filePath, Encoding.UTF8);

				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					Formatting = Formatting.Indented,
					Converters = { new StringEnumConverter() }
				};

				return JsonConvert.DeserializeObject<T>(json, settings);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error loading file {folderPath[saveType] + filePath}: {e.Message}");
		}
		return default;
	}
	public static void ResetSaves()
	{
		try
		{
			foreach (var path in folderPath.Values)
			{
				if (!string.IsNullOrEmpty(path) && Directory.Exists(path) && path != folderPath[SaveType.GENERIC])
				{
					DirectoryInfo directory = new DirectoryInfo(path);
					foreach (FileInfo file in directory.GetFiles())
					{
						file.Delete();
					}
					foreach (DirectoryInfo subDirectory in directory.GetDirectories())
					{
						subDirectory.Delete(true);
					}
					Console.WriteLine($"All saves in '{path}' have been deleted.");
				}
			}
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error resetting saves: {e.Message}");
		}
	}

}
public enum SaveType : int
{
	USER,
	AUTO,
	GENERIC
}