using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDialogController : MonoBehaviour
{

    public System.Action OnDialogComplete;

    public TMP_Text text;
    public Image image;
    public TMP_Text header;

    public Transform playerPosition, citizenPosition;

    public CitizenPortraitObject portrait;


    public bool IsComplete;

    Dictionary<string, Sprite> portraits;

    bool isAcknowledged;


    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void Show(DisplayDialog dialog)
    {
        IsComplete = false;
        image.transform.position = dialog.isPlayer ? playerPosition.position : citizenPosition.position;
        portrait = PortraitLoader.GetPortrait(dialog.portrait);
        image.sprite = GetSentiment(dialog.sentiment);
        header.text = dialog.title;
        gameObject.SetActive(true);
        StartCoroutine(Display(dialog));
    }

    public void Complete()
    {
        IsComplete = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            isAcknowledged = true;
        }
    }


    IEnumerator Display(DisplayDialog dialog)
    {
        foreach(string d in dialog.dialog)
        {
            isAcknowledged = false;
            text.text = "";
            foreach(char c in d)
            {
                if (isAcknowledged)
                {
                    Complete();
                    yield break;
                }
                text.text += c;
                switch (c)
                {
                    case ',':
                        yield return new WaitForSeconds(0.25f);
                        break;
                    case '.':
                    case '!':
                    case '?':
                        yield return new WaitForSeconds(0.5f);
                        break;
                    case ' ':
                        yield return new WaitForSeconds(0.1f);
                        break;
                    default:
                        yield return new WaitForSeconds(0.05f);
                        break;
                }
            }
            yield return new WaitUntil(() => isAcknowledged);
            yield return new WaitForSeconds(0.1f);
        }
        Complete();
    }

    public Sprite GetSentiment(int sentiment)
    {
        if (portrait == null)
            return null;

        if (sentiment > 0)
            return portrait.happy;
        if(sentiment < 0)
            return portrait.sad;
        else
            return portrait.neutral;
    }

    //bool IsAcknowledged()
    //{
    //    return Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E);
    //}


//    public Sprite GetPortrait(string key)
//    {
//        if(portraits == null)
//        {
//            portraits = new Dictionary<string, Sprite>();
//        }

//        if(portraits.ContainsKey(key))
//        {
//            return portraits[key];
//        }

//        Sprite sprite = Resources.Load<Sprite>(key);
//        if(sprite == null)
//        {
//            Debug.LogError($"DisplayDialog: Failed to load portrait {key}");
//            return null;
//        }

//        portraits.Add(key, sprite);
//        return sprite;
//    }
}


[System.Serializable]
public class DisplayDialog
{
    public string title;
    public string portrait;
    public bool isPlayer;
    public int sentiment;

    [TextArea]
    public string[] dialog;

}
