using UnityEngine;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class SP_Helper
{
#if UNITY_EDITOR
	private static Dictionary<string, GameObject> cachedPrefabs = new Dictionary<string, GameObject>();
#endif
	
	public static void SafeDestroy(Object o)
	{
#if UNITY_EDITOR
		if (Application.isPlaying == false)
		{
			Object.DestroyImmediate(o); return;
		}
#endif
		Object.Destroy(o);
	}
	
	public static bool Zero(float v)
	{
		return Mathf.Approximately(v, 0.0f);
	}
	
	public static float Divide(float a, float b)
	{
		return Zero(b) == false ? a / b : 0.0f;
	}
	
	public static T[] FindAll<T>()
		where T : Object
	{
		return (T[])GameObject.FindObjectsOfType(typeof(T));
	}
	
#if UNITY_EDITOR
	public static List<T> LoadAllPrefabComponents<T>()
		where T : Component
	{
		var paths      = AssetDatabase.GetAllAssetPaths();
		var components = new System.Collections.Generic.List<T>();
		
		foreach (var path in paths)
		{
			if (EndsWith(path, ".prefab") == true)
			{
				var cachedPrefab = default(GameObject);
				var found        = cachedPrefabs.TryGetValue(path, out cachedPrefab);
				
				if (cachedPrefab == null)
				{
					if (found == true) cachedPrefabs.Remove(path);
					
					cachedPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
					
					if (cachedPrefab != null) cachedPrefabs.Add(path, cachedPrefab);
				}
				
				if (cachedPrefab != null) components.AddRange(cachedPrefab.GetComponentsInChildren<T>(true));
			}
		}
		
		return components;
	}
#endif
	
#if UNITY_EDITOR
	public static T[] LoadAllObjectsAtGUID<T>(string guid)
		where T : Object
	{
		return LoadAllObjectsAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
	}
#endif

#if UNITY_EDITOR
	public static T[] LoadAllObjectsAtPath<T>(string path)
		where T : Object
	{
		return AssetDatabase.LoadAllAssetsAtPath(path).Select(o => o as T).Where(s => s != null).ToArray();
	}
#endif
	
#if UNITY_EDITOR
	public static TextureImporter SaveTextureAsset(Texture texture, string path, bool overwrite = false)
	{
		var bytes = ((Texture2D)texture).EncodeToPNG();
		var fs    = new System.IO.FileStream(path, overwrite == true ? System.IO.FileMode.Create : System.IO.FileMode.CreateNew);
		var bw    = new System.IO.BinaryWriter(fs);
		
		bw.Write(bytes);
		
		bw.Close();
		fs.Close();
		
		var importer = GetAssetImporter<TextureImporter>(path);
		
		if (importer == null)
		{
			ReimportAsset(path);
			
			importer = GetAssetImporter<TextureImporter>(path);
		}
		
		return importer;
	}
#endif

#if UNITY_EDITOR
	public static T GetAssetImporter<T>(string path)
		where T : AssetImporter
	{
		return (T)AssetImporter.GetAtPath(path);
	}
#endif

#if UNITY_EDITOR
	public static void ReimportAsset(string path)
	{
		AssetDatabase.ImportAsset(path);
	}
#endif
	
#if UNITY_EDITOR
	public static string GetContextDirectory()
	{
		var dir = AssetDatabase.GetAssetPath(Selection.activeObject); if (string.IsNullOrEmpty(dir) == true) dir = "Assets";
		
		if (System.IO.Directory.Exists(dir) == false)
		{
			dir = dir.Substring(0, dir.LastIndexOf('/'));
		}
		
		return dir;
	}
#endif
	
#if UNITY_EDITOR
	public static void SetDirty<T>(T t)
		where T : Object
	{
		if (t != null)
		{
			UnityEditor.EditorUtility.SetDirty(t);
		}
	}
#endif
	
#if UNITY_EDITOR
	public static string GetPathDirectory(string path)
	{
		if (System.IO.Directory.Exists(path) == false)
		{
			var slash = path.LastIndexOf('/');
			
			if (slash != -1)
			{
				path = path.Substring(0, slash);
			}
		}
		
		return path;
	}
#endif
	
	public static bool EndsWith(string a, string b)
	{
		var al = a.Length;
		var bl = b.Length;
		
		if (al >= bl)
		{
			while (bl > 0)
			{
				if (a[--al] != b[--bl])
				{
					return false;
				}
			}
		}
		
		return true;
	}
	
#if UNITY_EDITOR
	public static Texture2D CreateTempTexture(int width, int height, string encoded)
	{
		var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		
		texture.hideFlags = HideFlags.HideAndDontSave;
		texture.LoadImage(System.Convert.FromBase64String(encoded));
		texture.Apply();
		
		return texture;
	}
#endif
}