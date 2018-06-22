using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamFeed : MonoBehaviour {

    //  public RawImage rawimage;

     void Start () 
     {
         WebCamTexture webcamTexture = new WebCamTexture();
		 MeshRenderer renderer = this.GetComponent<MeshRenderer>();
		 renderer.material.mainTexture = webcamTexture;


        //  rawimage.texture = webcamTexture;
        //  rawimage.material.mainTexture = webcamTexture;
         webcamTexture.Play();
     }

}
