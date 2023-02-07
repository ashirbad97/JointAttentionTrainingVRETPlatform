using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Objective is to add some custom properties to the endTargets to determine if they are left or right
//Attach this to the endTargets
public class TargetEndPointProperty : MonoBehaviour
{
    //Set the directions of the endTargets
    public enum endTargetDirection
    {
        Left,
        Right
    }
    //Shows a drop-down list to selet the direction
    [SerializeField] public endTargetDirection direction;
}
