using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CitizenSpawnToolEditor : EditorWindow
{
    private GameObject prefab;
    private List<CitizenObject> citizenObjects = new List<CitizenObject>();

    [MenuItem("Tools/Citizen Spawner")]
    public static void ShowWindow()
    {
        GetWindow<CitizenSpawnToolEditor>("Citizen Spawner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Citizen Spawner", EditorStyles.boldLabel);

        // Prefab field
        prefab = (GameObject)EditorGUILayout.ObjectField("Citizen Prefab", prefab, typeof(GameObject), false);

        // CitizenObjects list
        EditorGUILayout.LabelField("Citizen Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Citizen Object"))
        {
            citizenObjects.Add(null);
        }

        if (GUILayout.Button("Add Selected Citizens"))
        {
            foreach(var obj in Selection.objects)
            {
                if(obj is CitizenObject citizen)
                {
                    citizenObjects.Add(citizen);
                }
            }
        }

        for (int i = 0; i < citizenObjects.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            citizenObjects[i] = (CitizenObject)EditorGUILayout.ObjectField(citizenObjects[i], typeof(CitizenObject), false);

            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                citizenObjects.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Spawn button
        if (GUILayout.Button("Spawn Citizens"))
        {
            SpawnCitizens();
        }
    }

    private void SpawnCitizens()
    {
        if (prefab == null)
        {
            Debug.LogError("Please assign a Citizen Prefab.");
            return;
        }

        if (citizenObjects.Count == 0)
        {
            Debug.LogError("Please add at least one Citizen Object.");
            return;
        }

        foreach (CitizenObject citizenObject in citizenObjects)
        {
            if (citizenObject == null) continue;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            if (instance != null)
            {
                CitizenBehaviour citizenBehaviour = instance.GetComponent<CitizenBehaviour>();

                if (citizenBehaviour != null)
                {
                    citizenBehaviour.citizen = citizenObject;
                    citizenBehaviour.gameObject.name = citizenObject.name;
                    citizenBehaviour.GetComponentInChildren<SpriteRenderer>().color = Random.ColorHSV();
                    Undo.RegisterCreatedObjectUndo(instance, "Spawn Citizen");
                    Debug.Log($"Spawned Citizen with {citizenObject.name}");
                }
                else
                {
                    Debug.LogError("Prefab does not contain a CitizenBehaviour component.");
                    DestroyImmediate(instance);
                }
            }
            else
            {
                Debug.LogError("Failed to instantiate prefab.");
            }
        }
    }
}
