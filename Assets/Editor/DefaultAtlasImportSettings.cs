using UnityEngine;
using UnityEditor;
 
public class TexturePostProcessor : AssetPostprocessor {
 
	void OnPostprocessTexture (Texture2D texture) {
		TextureImporter importer = (TextureImporter)assetImporter;
		if (importer.assetPath.Contains ("Assets/SpriteAtlases")) {
			importer.anisoLevel = 0;
			importer.filterMode = FilterMode.Bilinear;
			importer.maxTextureSize = 4096;
			importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
		}
	}
 
}