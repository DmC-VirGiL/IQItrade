using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using appnext;
using UnityEngine.UI;

public class AppnextAds : MonoBehaviour
{
    // Start is called before the first frame update
    Interstitial interstitial;
    BannerView view;
    public Text info;
    void Start()
    {
        view = new BannerView("832b5c43-b03b-4602-847f-d958b27efead");
        interstitial = new Interstitial("853edd67-702a-437f-b2cd-c17535ad9acc");
        view.loadAd();
        interstitial.loadAd();
        configads();
    }
    public void BannerShow()
    {
        view.showAd();
    }
    private void Awake()
    {
        
    }
    public void ShowAppnextAds()
    {
       
        interstitial.setButtonColor("#ffffff");
        interstitial.setButtonText("Install");    
        interstitial.setMute(false);
        interstitial.setAutoPlay(true);
        interstitial.setCreativeType(Interstitial.TYPE_MANAGED);
        interstitial.setOrientation(Ad.ORIENTATION_DEFAULT);
        interstitial.showAd();
    }
    void configads()
    {
          interstitial.onAdLoadedDelegate += onAdLoaded;
        //Get notified when the ad was clicked:
        interstitial.onAdClickedDelegate += onAdClicked;
         //Get notified when the ad was closed:
        
        //Get notified when an error occurred:
        interstitial.onAdErrorDelegate += onAdError;
    }
    // Update is called once per frame
    void onAdLoaded(Ad ads)
    {
        if(ads!=null)
        {
            info.text = "Loding..";
        }
    }
    void onAdClicked(Ad ads)
    {
        if (ads != null)
        {
            info.text = "Clicking..";
        }
    }
    void onAdError(Ad ad, string error)
    {
        info.text = "Error:" + error;
    }
    void Update()
    {
        
    }
}
