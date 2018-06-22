using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

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
		string streamName = "AmazonRekognitionStreamOut";

		awsClient.ReadStream(streamName, (response)=>{
			List<Amazon.Kinesis.Model.Record> records = response.Records;
			foreach(Amazon.Kinesis.Model.Record awsRecord in records){
				string recordString = Encoding.ASCII.GetString(awsRecord.Data.ToArray());
				RekogRecord record = RekogRecord.CreateFromJSON(recordString);
				Debug.Log(record);
			}
		});
	}
}
