using System.IO;
using UnityEngine;

namespace Anathema.Saving
{
	public class SaveProfile
	{
		private const string SaveExtension = ".anathema";
		private const string SaveDirectoryName = "Saves";
		private string profileName;
		public bool enablePrettyPrint;

		private static string SaveDirectory
		{
			get
			{
				string unityFolder = Application.persistentDataPath;
				string savesFolder = Path.Combine(unityFolder, SaveDirectoryName);
				if (!Directory.Exists(savesFolder))
				{
					Directory.CreateDirectory(savesFolder);
				}
				return savesFolder;
			}
		}
		private string SavePath
		{
			get
			{
				return Path.Combine(SaveDirectory, profileName + SaveExtension);
			}
		}
		public string ProfileName { get { return profileName; } }
		public SaveProfile(string profileName, bool enablePrettyPrint) : this(profileName)
		{
			this.enablePrettyPrint = enablePrettyPrint;
		}
		public SaveProfile(string profileName)
		{ 
			this.profileName = profileName;
		}
		public GameData Load()
		{
			string saveJson = System.IO.File.ReadAllText(SavePath);
			GameData levelSave = JsonUtility.FromJson<GameData>(saveJson);
			levelSave.ProfileName = profileName;

			return levelSave;
		}
		public void Save(GameData levelSave)
		{
			System.IO.File.WriteAllText(SavePath, JsonUtility.ToJson(levelSave, enablePrettyPrint));
		}
		private static string[] GetSavePaths(string profilePath){
			return Directory.GetFiles(profilePath, "*" + SaveExtension);
		}
		public static string[] GetProfileNames(){
			string[] files = Directory.GetFiles(SaveDirectory, "*" + SaveExtension);
			string[] names = new string[files.Length];
			for (int i = 0; i < files.Length; i++){
				FileInfo fileInfo = new FileInfo(files[i]);
				names[i] = Path.GetFileNameWithoutExtension(fileInfo.Name);
			}
			return names;
		}
	}
}