using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CitizenPortraitGenerator : EditorWindow
{
    private Object[] selectedFiles;
    private string outputDirectory = "Assets/ScriptableObjects";

    [MenuItem("Tools/Citizen Portrait Generator")]
    public static void ShowWindow()
    {
        GetWindow<CitizenPortraitGenerator>("Citizen Portrait Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Aseprite File Selector", EditorStyles.boldLabel);

        if (GUILayout.Button("Select Aseprite Files"))
        {
            selectedFiles = Selection.objects.Where(o => o is GameObject).ToArray();
            Debug.Log($"{selectedFiles.Length} Aseprite files selected.");
        }

        if (selectedFiles != null && selectedFiles.Length > 0)
        {
            GUILayout.Label($"Selected Files: {selectedFiles.Length}", EditorStyles.label);

            if (GUILayout.Button("Generate Citizen Portraits"))
            {
                GenerateCitizenPortraits();
            }
        }

        outputDirectory = EditorGUILayout.TextField("Output Directory", outputDirectory);
    }

    private void GenerateCitizenPortraits()
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
            AssetDatabase.Refresh();
        }

        foreach (var obj in selectedFiles)
        {
            if (obj is GameObject texture)
            {
                string assetPath = AssetDatabase.GetAssetPath(texture);
                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().ToArray();

                if (sprites.Length == 3) // Ensure there are exactly 3 frames
                {
                    string fileName = texture.name;
                    string assetFilePath = Path.Combine(outputDirectory, $"{fileName.Replace("_Portraits","").Trim()}.asset");
                    CitizenPortraitObject citizenPortrait = AssetDatabase.LoadAssetAtPath<CitizenPortraitObject>(assetFilePath);

                    if (citizenPortrait == null)
                    {
                        citizenPortrait = ScriptableObject.CreateInstance<CitizenPortraitObject>();
                        AssetDatabase.CreateAsset(citizenPortrait, assetFilePath);
                        Debug.Log($"Created new Citizen Portrait: {fileName}");
                    }
                    else
                    {
                        Debug.Log($"Updated existing Citizen Portrait: {fileName}");
                    }

                    citizenPortrait.neutral = sprites[0];
                    citizenPortrait.happy = sprites[1];
                    citizenPortrait.sad = sprites[2];

                    EditorUtility.SetDirty(citizenPortrait);
                }
                else
                {
                    Debug.LogWarning($"Skipping {texture.name}. It does not have exactly 3 frames.");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Citizen Portraits generated/updated successfully!");
    }
}
