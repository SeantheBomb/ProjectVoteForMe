using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerCanvas", menuName = "GameModels/PlayerCanvas")]
public class PlayerCanvasObject : ScriptableObject
{

    public string portrait;

    public int money;

    public List<PlayerProposalObject> proposals;
}
