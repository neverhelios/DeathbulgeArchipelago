using System.Collections.Generic;
using System.IO;
using BepInEx;
using UnityEngine;


namespace DeathbulgeArchipelagoClient;

class ResourcesLoader
{
    private static readonly Dictionary<string, Sprite> cachedSprites = new();

    public static Sprite GetSprite(string sprite)
    {
        if (!cachedSprites.ContainsKey(sprite))
            cachedSprites[sprite] = CreateNewSprite(sprite);

        return cachedSprites[sprite];
    }


    private static Sprite CreateNewSprite(string file)
    {

        string path = Paths.PluginPath + "\\DeathbulgeArchipelagoClient\\resources\\" + file;
        Texture2D texture = new Texture2D(2, 2);
        if (!File.Exists(path)) { return null; }
        byte[] imageAsset = System.IO.File.ReadAllBytes(path);
        ImageConversion.LoadImage(texture, imageAsset);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 128);
        return sprite;
    }
}