using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotControl : MonoBehaviour
 {
	/// <summary>
	/// This class is used to store the current spot information, so that it can be accessed by any movement state
	/// </summary>
	public int currentSpot;

	void Awake()
	{
		currentSpot = 0;
	}

}
