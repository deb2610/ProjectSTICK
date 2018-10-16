using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if (other.gameObject.name.Contains("Player"))
        {
            // TODO: End the game
        }
=======
        // TODO: End the game
>>>>>>> 4e89573d98f8b7219c4ef7494f58945c89bc0d15
    }
}
