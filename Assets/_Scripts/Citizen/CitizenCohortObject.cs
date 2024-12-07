using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Neighbourhood", menuName = "GameModels/Cohort")]
public class CitizenCohortObject : ScriptableObject
{

    public List<CitizenObject> citizens;

}
