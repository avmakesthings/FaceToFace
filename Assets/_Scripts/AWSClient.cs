//
// Copyright 2014-2015 Amazon.com, 
// Inc. or its affiliates. All Rights Reserved.
// 
// Licensed under the AWS Mobile SDK For Unity 
// Sample Application License Agreement (the "License"). 
// You may not use this file except in compliance with the 
// License. A copy of the License is located 
// in the "license" file accompanying this file. This file is 
// distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, express or implied. See the License 
// for the specific language governing permissions and 
// limitations under the License.
//

using UnityEngine;
using UnityEngine.UI;
using Amazon.Kinesis;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using Amazon.Kinesis.Model;
using System.IO;
using System.Collections.Generic;

public struct PutRecordResponse{
	public int HttpStatusCode;	
	public string SequenceNumber;
	public ResponseMetadata ResponseMetadata;
}
public delegate void HandlePutRecordResponse(PutRecordResponse response);

public struct ListStreamsResponse{
	public int HttpStatusCode;	
	public ResponseMetadata ResponseMetadata;
	public List<string> StreamNames;
}
public delegate void HandleListStreamsResponse(ListStreamsResponse response);

public struct DescribeStreamResponse{
	public int HttpStatusCode;	
	public ResponseMetadata ResponseMetadata;
	public StreamDescription StreamDescription;
}
public delegate void HandleDescribeStreamResponse(DescribeStreamResponse response);

public class AWSClient : MonoBehaviour
{
	private string IdentityPoolId = "us-west-2:13e0e993-0cc5-48c9-b4c3-4430cadad5f0";
	public Amazon.RegionEndpoint RegionEndpoint = RegionEndpoint.USWest2;

	void Start()
	{
		UnityInitializer.AttachToGameObject(this.gameObject);
	}

	#region private members

	private IAmazonKinesis _kinesisClient;
	private AWSCredentials _credentials;

	private AWSCredentials Credentials
	{
		get
		{
			if (_credentials == null)
				AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
				_credentials = new CognitoAWSCredentials(
					IdentityPoolId,
					RegionEndpoint
				); 
			return _credentials;
		}
	}

	private IAmazonKinesis Client
	{
		get
		{
			if (_kinesisClient == null)
			{
				_kinesisClient = new AmazonKinesisClient(
					Credentials, 
					RegionEndpoint
				);
			}
			return _kinesisClient;
		}
	}

	#endregion

	# region Put Record
	/// <summary>
	/// Example method to demostrate Kinesis PutRecord. Puts a record with the data specified
	/// in the "Record Data" Text Input Field to the stream specified in the "Stream Name"
	/// Text Input Field.
	/// </summary>
	public void PutRecord(string record, string streamName, HandlePutRecordResponse cb)
	{	
		using (var memoryStream = new MemoryStream())
		using (var streamWriter = new StreamWriter(memoryStream))
		{
			streamWriter.Write(record);
			Client.PutRecordAsync(new PutRecordRequest
			{
				Data = memoryStream,
				PartitionKey = "partitionKey",
				StreamName = streamName
			},
			(responseObject) =>
			{
				if (responseObject.Exception == null)
				{
					cb(new PutRecordResponse(){
						ResponseMetadata = responseObject.Response.ResponseMetadata,
						HttpStatusCode = (int)responseObject.Response.HttpStatusCode,
						SequenceNumber = responseObject.Response.SequenceNumber
					});
				}
				else
				{
					Debug.LogError(responseObject.Exception);
					cb(new PutRecordResponse());
				}
			}
			);
		}
	}

	# endregion

	# region List Streams
	/// <summary>
	/// Example method to demostrate Kinesis ListStreams. Prints all of the Kinesis Streams
	/// that your Cognito Identity has access to.
	/// </summary>
	public void ListStreams( HandleListStreamsResponse cb)
	{
		Debug.Log("Listing Streams");
		Client.ListStreamsAsync(new ListStreamsRequest(),
		(responseObject) =>
		{
			if (responseObject.Exception == null)
			{
				Debug.Log("Got Response!");
				cb(new ListStreamsResponse(){
					ResponseMetadata = responseObject.Response.ResponseMetadata,
					HttpStatusCode = (int)responseObject.Response.HttpStatusCode,
					StreamNames = responseObject.Response.StreamNames
				});
			}
			else
			{
				Debug.LogError(responseObject.Exception);
				cb(new ListStreamsResponse());
			}
		}
		);
	}

	# endregion

	# region Describe Stream
	/// <summary>
	/// Example method to demostrate Kinesis DescribeStream. Prints information about the
	/// stream specified in the "Stream Name" Text Input Field.
	/// </summary>
	public void DescribeStream(string StreamName, HandleDescribeStreamResponse cb)
	{
		Client.DescribeStreamAsync(new DescribeStreamRequest()
		{
			StreamName = StreamName
		},
		(responseObject) =>
		{
			if (responseObject.Exception == null)
			{
				cb(new DescribeStreamResponse(){
					ResponseMetadata = responseObject.Response.ResponseMetadata,
					HttpStatusCode = (int)responseObject.Response.HttpStatusCode,
					StreamDescription = responseObject.Response.StreamDescription
				});
			}
			else
			{
				Debug.LogError(responseObject.Exception);
				cb(new DescribeStreamResponse());
			}
		}
		);
	}

	# endregion
}
