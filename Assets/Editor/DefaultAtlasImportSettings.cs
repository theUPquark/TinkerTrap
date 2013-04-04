using UnityEngine;
using UnityEditor;
 
public class TexturePostProcessor : AssetPostprocessor {
 
	void OnPostprocessTexture (Texture2D texture) {
		TextureImporter importer = assetImporter as TextureImporter;
		if (importer.assetPath.Contains ("Assets/SpriteAtlases")) {
			importer.anisoLevel = 0;
			importer.filterMode = FilterMode.Bilinear;
			importer.maxTextureSize = 4096;
			importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
			
			Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
			if (asset) {
				EditorUtility.SetDirty(asset);
			} else {
				texture.anisoLevel = 0;
				texture.filterMode = FilterMode.Bilinear;
				texture.maxTextureSize = 4096;
				texture.textureFormat = TextureImporterFormat.AutomaticCompressed;
			}
		}
	}
 
}