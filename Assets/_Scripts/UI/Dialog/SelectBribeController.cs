using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBribeController : MonoBehaviour
{

    public bool IsSubmitted;
    public bool IsBribing;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        IsSubmitted = false;
    }


    public void AttemptBribe()
    {
        gameObject.SetActive(false);
        IsBribing = true;
        IsSubmitted = true;
    }


    public void SkipBribe()
    {
        gameObject.SetActive(false);
        IsBribing = false;
        IsSubmitted = true;
    }

    
}
