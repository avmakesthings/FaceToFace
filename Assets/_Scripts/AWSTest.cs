using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWSTest : MonoBehaviour {

	public GameObject AWSClientObject;

	// Use this for initialization
	void  Start() {
		StartCoroutine("TestClient");
	}

	IEnumerator TestClient(){
		yield return new WaitForSeconds(1);
		// Test POC AWS client
		AWSClient awsClient = AWSClientObject.GetComponent<AWSClient>();

		awsClient.ListStreams((ListStreamsResponse r)=>{
			foreach(string streamName in r.StreamNames){
				Debug.Log(streamName);
			}
			Debug.Log("This prints last.");
		});

		Debug.Log("You would think this would print last, but you'd be wrong.");
	}
}
