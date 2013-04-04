using UnityEngine;
using UnityEditor;
 
public class TexturePostProcessor : AssetPostprocessor {
 
	void OnPreprocessTexture (Texture2D texture) {
		TextureImporter importer = assetImporter as TextureImporter;
		if (importer.assetPath.Contains ("Assets/SpriteAtlases")) {
			importer.textureType = TextureImporterType.Advanced;
			importer.anisoLevel = 0;
			importer.filterMode = FilterMode.Bilinear;
			importer.maxTextureSize = 4096;
			importer.textureFormat = TextureImporterFormat.RGBA32;
		}
	}
 
}