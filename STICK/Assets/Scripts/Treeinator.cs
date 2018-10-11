using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treeinator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 randRotation = new Vector3(transform.rotation.x,  transform.rotation.y, Random.Range(0, 360));
        gameObject.transform.rotation = Quaternion.Euler(randRotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
