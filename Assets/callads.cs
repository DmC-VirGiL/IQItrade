using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callads : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject fbAudiance;
   
    void Start()
    {
        fbAudiance = GameObject.Find("Canvas/MainUI/GraphHolder");
       
    }
    public void showinterstitialads()
    {
        fbAudiance.GetComponent<InterstitialAdScene>().ShowInterstitial();
       
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }


}
