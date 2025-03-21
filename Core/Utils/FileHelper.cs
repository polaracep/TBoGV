using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public static class FileHelper
{
	public static Dictionary<SaveType, string> folderPath = new Dictionary<SaveType, string>
	{
		{SaveType.USER, "userSave/" },
		{SaveType.AUTO, "autoSave/" },
		{SaveType.GENERIC, "" },
	};

	private static JsonSerializerSettings settings = new JsonSerializerSettings
	{
		Formatting = Formatting.Indented,
		Converters = { new StringEnumConverter() }
	};
	public static void Save<T>(string filePath, T data, SaveType saveType)
	{
		try
		{
			string fullPath = folderPath[saveType] + filePath;
			string directory = Path.GetDirectoryName(fullPath);

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}


			string json = JsonConvert.SerializeObject(data, settings);
			File.WriteAllText(fullPath, json, Encoding.UTF8);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error saving file {filePath}: {e.Message}");
		}
	}



	public static T Load<T>(string filePath, SaveType saveType)
	{
		try
		{
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
				if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
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