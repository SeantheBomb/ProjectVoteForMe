using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PortraitLoader {


    public static Dictionary<string, CitizenPortraitObject> portraits;


    public static CitizenPortraitObject GetPortrait(string key)
    {
        if (portraits == null)
        {
            portraits = new Dictionary<string, CitizenPortraitObject>();
        }

        key = "Portraits/"+key.Replace("_Portraits","").Replace("\t", "").Trim();

        if (portraits.ContainsKey(key))
        {
            return portraits[key];
        }

        CitizenPortraitObject sprite = Resources.Load<CitizenPortraitObject>(key);
        if (sprite == null)
        {
            Debug.LogError($"DisplayDialog: Failed to load portrait {key}");
            //return null;
        }

        portraits.Add(key, sprite);
        return sprite;
    }

}
