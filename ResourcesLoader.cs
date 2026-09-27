using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;

class ResourcesLoader
{
    private const int SpriteSize = 128;

    private static readonly Dictionary<string, Sprite> cachedSprites = [];

    public static Sprite GetSprite(string sprite)
    {
        if (!cachedSprites.TryGetValue(sprite, out var cached))
        {
            cached = CreateNewSprite(sprite);
            cachedSprites[sprite] = cached;
        }
        return cached;
    }


    private static Sprite CreateNewSprite(string file)
    {
        string path = Path.Combine(Paths.PluginPath, "DeathbulgeArchipelagoClient", "resources", file);
        if (!File.Exists(path))
        {
            Plugin.Logger.LogError($"Wrong path {path}, using default sprite");
            return CreateDefaultSprite();
        }

        Texture2D texture = new(2, 2);
        ImageConversion.LoadImage(texture, File.ReadAllBytes(path));
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), SpriteSize);
    }

    private static Sprite CreateDefaultSprite()
    {
        Texture2D texture = new(1, 1);
        texture.SetPixel(0, 0, Color.green);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
}
