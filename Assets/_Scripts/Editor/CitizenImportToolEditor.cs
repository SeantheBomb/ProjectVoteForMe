using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using System.Linq;
using System.Runtime.Remoting.Messaging;

public class CitizenImportToolEditor : EditorWindow
{

    string inputFilePath;
    string outputDirectoryPath = "Assets/_Models/Citizens";


    [MenuItem("Tools/Import Citizen")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CitizenImportToolEditor));
    }

    void OnGUI()
    {
        // The actual window code goes here
        if (string.IsNullOrWhiteSpace(inputFilePath))
        {
            GUILayout.Label("Select a file to import");
        }
        else
        {
            GUILayout.Label("Import: " + inputFilePath);
        }

        if(GUILayout.Button("Select File To Import"))
        {
            inputFilePath = EditorUtility.OpenFilePanel("Select Citizen Data TSV", "", "tsv");
        }

        GUILayout.Label("Output Directory");
        outputDirectoryPath = GUILayout.TextField(outputDirectoryPath);

        if(string.IsNullOrWhiteSpace(inputFilePath) ==false )
        {
            if (GUILayout.Button("Import"))
            {
                GenerateCitizen(ImportData());
            }
        }
    }



    string[] ImportData()
    {
        string input = File.ReadAllText(inputFilePath);
        string[] result = input.Split("\n");
        return result;
    }

    string[] GetFields(string row)
    {
        return row.Split('\t');
    }

    void GenerateCitizen(string[] tsv)
    {
        CitizenObject citizen = ScriptableObject.CreateInstance<CitizenObject>();
        for (int i = 0; i < tsv.Length; i++)
        {
            string[] fields = GetFields(tsv[i]);
            if(fields.Length < 2)
            {
                continue;
            }
            string header = fields[1];
            Debug.Log($"CitizenImport: Reached header {header}");
            if (header.Contains("Bio"))
            {
                GenerateBio(tsv, ref citizen, ref i);
                //continue;
            }
            //if(citizen == null)
            //{
            //    Debug.LogError("CitizenObject is null!");
            //    continue;
            //}
            if (header.Contains("Sentiment"))
            {
                GenerateSentiment(citizen, tsv, ref i);
            }
            if (header.Contains("Proposal"))
            {
                //int index = int.header.Split(' ').Last()
                GenerateProposal(citizen, tsv, ref i);
            }
            if (header.Contains("Result"))
            {
                GenerateResult(citizen, tsv, ref i);
            }
        }

        SaveOrCreateObject(citizen);

    }

    void GenerateBio(string[] tsv, ref CitizenObject citizen, ref int row)
    {
        CitizenBio bio = new CitizenBio();
        for (row++; row < tsv.Length; row++)
        {
            string[] fields = GetFields(tsv[row]);
            if(fields.Length < 2 || string.IsNullOrWhiteSpace(fields[0]))
            {
                citizen.bio = bio;
                return;
            }
            Debug.Log($"CitizenImport: Reached bio {tsv[row]}");
            switch (fields[0])
            {
                case "Name":
                    bio.name = fields[1];
                    break;
                case "Portrait":
                    bio.portrait = fields[1];
                    break;
                case "Avatar":
                    bio.avater = fields[1];
                    break;
                case "Description":
                    bio.description = fields[1];
                    break;
                case "Dossier":
                    bio.dossier = fields[1];
                    break;
            }
        }
        //citizen = FindOrCreateObject(ConvertNameToId(bio.name));
        citizen.bio = bio;
    }

    void GenerateSentiment(CitizenObject citizen, string[] tsv, ref int row)
    {
        CitizenSentiment sentiment = new CitizenSentiment();
        for (row++; row < tsv.Length; row++)
        {
            string[] fields = GetFields(tsv[row]);
            if (fields.Length < 2 || string.IsNullOrWhiteSpace(fields[0]))
            {
                citizen.sentiment = sentiment;

                return;
            }
            Debug.Log($"CitizenImport: Reached sentiment {tsv[row]}");
            switch (fields[0])
            {
                case "Intro":
                    sentiment.intro = fields[1];
                    break;
                case "Positive":
                    sentiment.positive = fields[1];
                    break;
                case "Negative":
                    sentiment.negative = fields[1];
                    break;
                case "Start Opinion":
                    sentiment.startingOpinion = int.Parse(fields[1]);
                    break;

            }
        }
        citizen.sentiment = sentiment;
    }

    void GenerateProposal(CitizenObject citizen, string[] tsv, ref int row)
    {
        CitizenProposal proposal = new CitizenProposal();
        for (row++; row < tsv.Length; row++)
        {
            string[] fields = GetFields(tsv[row]);
            if (fields.Length < 2 || string.IsNullOrWhiteSpace(fields[0]))
            {
                if (citizen.proposals == null)
                    citizen.proposals = new List<CitizenProposal>();
                citizen.proposals.Add(proposal);

                return;
            }
            Debug.Log($"CitizenImport: Reached proposal {tsv[row]}");

            //Positive
            //Negative
            //"+2"
            //"+1"
            //"0"
            //"-1"
            //"-2"
            switch (fields[0])
            {
                case "Positive":
                    proposal.positive = fields[1];
                    break;
                case "Negative":
                    proposal.negative = fields[1];
                    break;
                case "\"+2\"":
                    proposal.plusTwo = fields[1];
                    break;
                case "\"+1\"":
                    proposal.plusOne = fields[1];
                    break;
                case "\"0\"":
                    proposal.zero = fields[1];
                    break;
                case "\"-2\"":
                    proposal.minusTwo = fields[1];
                    break;
                case "\"-1\"":
                    proposal.minusOne = fields[1];
                    break;
            }
        }

        if (citizen.proposals == null)
            citizen.proposals = new List<CitizenProposal>();
        citizen.proposals.Add( proposal);
    }

    void GenerateResult(CitizenObject citizen, string[] tsv, ref int row)
    {
        CitizenResult result = new CitizenResult();
        for (row++; row < tsv.Length; row++)
        {
            string[] fields = GetFields(tsv[row]);
            if (fields.Length < 2 || string.IsNullOrWhiteSpace(fields[0]))
            {
                citizen.result = result;

                return;
            }
            Debug.Log($"CitizenImport: Reached result {tsv[row]}");
            switch (fields[0])
            {
                case "Yes":
                    result.Yes = fields[1];
                    break;
                case "No":
                    result.No = fields[1];
                    break;
            }
        }
        citizen.result = result;
    }


    CitizenObject FindOrCreateObject(string name)
    {
        CitizenObject result;
        string[] find = AssetDatabase.FindAssets(name, new string[] {outputDirectoryPath});
        if(find.Length == 0)
        {
            result = ScriptableObject.CreateInstance<CitizenObject>();
            AssetDatabase.CreateAsset(result, GetObjectPath(result));
            return result;
        }

        result = AssetDatabase.LoadAssetAtPath<CitizenObject>(find[0]);
        return result;
    }

    void SaveOrCreateObject(CitizenObject citizen)
    {
        if (Directory.Exists(outputDirectoryPath)==false)
        {
            Directory.CreateDirectory(outputDirectoryPath);
        }

        string[] find = AssetDatabase.FindAssets(name, new string[] { outputDirectoryPath });
        bool hasMatch = false;
        foreach(string s in find)
        {
            hasMatch = GetObjectFile(citizen).Equals(Path.GetFileName(s));
            Debug.Log($"CitizenImport: Check if {GetObjectFile(citizen)} exists, matches {Path.GetFileName(s)}? {hasMatch}");
            if (hasMatch)
            {
                AssetDatabase.SetMainObject(citizen, GetObjectPath(citizen));
                break;
            }
        }
        if (hasMatch == false)
        {
            AssetDatabase.CreateAsset(citizen, GetObjectPath(citizen));
        }

        AssetDatabase.SaveAssets();
    }

    string ConvertNameToId(string name)
    {
        return name.Trim().Replace(' ', '_');
    }

    string GetObjectId(CitizenObject obj)
    {
        return ConvertNameToId(obj.bio.name);
    }

    string GetObjectFile(CitizenObject obj)
    {
        return $"Citizen_{GetObjectId(obj)}.asset";
    }

    string GetObjectPath(CitizenObject obj)
    {
        return Path.Combine(outputDirectoryPath, GetObjectFile(obj));
    }
}
