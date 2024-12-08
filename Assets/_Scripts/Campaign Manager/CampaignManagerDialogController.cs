using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampaignManagerDialogController : MonoBehaviour
{

    public CampaignManagerObject manager;

    public MacroMenuController macro;

    public TMP_Text text;

    bool isAcknowledged;


    // Start is called before the first frame update
    void Start()
    {
        macro = GetComponentInParent<MacroMenuController>();
        if(GameManager.CurrentSession.CurrentLevelIndex == -1)
        {
            StartCoroutine(Display(manager.intro));
        }
        else
        {
            var interstitial = manager.interstitials[GameManager.CurrentSession.CurrentLevelIndex];
            var report = macro.GetTotalResults();
            bool isWinning = report.playerTally > report.opposingTally;
            string[] output = isWinning ? interstitial.winningDialog : interstitial.losingDialog;
            StartCoroutine(Display(output));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            isAcknowledged = true;
        }
    }

    IEnumerator Display(string[] dialog)
    {
        foreach (string d in dialog)
        {
            string output = d.Replace("[Name]", GameManager.CurrentSession.playerName);
            isAcknowledged = false;
            text.text = "";
            foreach (char c in output)
            {
                if (isAcknowledged)
                {
                    //Complete();
                    yield return null;
                    continue;
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
        //Complete();
    }
}
