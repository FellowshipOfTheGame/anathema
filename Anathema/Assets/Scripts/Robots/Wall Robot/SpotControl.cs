using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotControl : MonoBehaviour
{
    /// <summary>
    /// This class is used to store the current spot information, so that it can be accessed by any movement state
    /// </summary>


    public int currentSpot;

    //bool to know if the robot is walking in a platform
    public bool platform;
    void Awake()
    {
        currentSpot = 1;
        platform = false;
    }

}
