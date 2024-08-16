using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


class CustomPostprocessor : AssetPostprocessor
{
    private const string PathPrefix = "Assets/_Project";
    private static string MusicPath => PathPrefix + "/Audio/Music";
    private static string PlatformPath => PathPrefix + "/Platform";
    private static string SpriteSources => PathPrefix + "/Sprites/Sources";
    private static string Graphics => PathPrefix + "/Graphics";
    // audio asset preprocessor
    void OnPreprocessAudio()
    {
        // check if it's a music file
        if (assetPath.StartsWith(MusicPath))
        {
            PreprocessMusic();
        }
    }

    // texture asset preprocessor
    void OnPreprocessTexture()
    {
        // check if it's a platform image (icons, splash, etc.)
        if (assetPath.StartsWith(PlatformPath)) 
        {
            PreprocessPlatformImages();
        }

        // check if it's a sprite source image
        if (assetPath.StartsWith(SpriteSources))
        {
            PreprocessSpriteSource();
        }

        // check if it's a sprite texture atlas
        if (assetPath.StartsWith(Graphics))
        {
            PreprocessSpriteAtlas();
        }
    }


    // preprocess music (2D, stream from disc, hardware decoding)
    void PreprocessMusic()
    {
        AudioImporter importer = (AudioImporter)assetImporter;
    }


    // preprocess platform icons, etc.
    void PreprocessPlatformImages()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
    }


    // preprocess sprite source images (uncompressed, no mips, non-pow2, etc.)
    void PreprocessSpriteSource()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
    }


    // preprocess sprite texture atlases (uncompressed, mips, POT, filter, etc.)
    void PreprocessSpriteAtlas()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
        // importer.textureCompression = TextureImporterCompression.Uncompressed;
        // importer.filterMode = FilterMode.Point;
        // importer.spritePixelsPerUnit = 16;
        // importer.textureType = TextureImporterType.Sprite;
    }
}