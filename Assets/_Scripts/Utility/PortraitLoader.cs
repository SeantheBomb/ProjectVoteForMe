using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PortraitLoader {


    public static Dictionary<string, Sprite> portraits;


    public static Sprite GetPortrait(string key)
    {
        if (portraits == null)
        {
            portraits = new Dictionary<string, Sprite>();
        }

        if (portraits.ContainsKey(key))
        {
            return portraits[key];
        }

        Sprite sprite = Resources.Load<Sprite>(key);
        if (sprite == null)
        {
            Debug.LogError($"DisplayDialog: Failed to load portrait {key}");
            //return null;
        }

        portraits.Add(key, sprite);
        return sprite;
    }

}
