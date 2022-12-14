using UnityEngine;
using UnityEngine.UI;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using AudienceNetwork.Utility;

public class InterstitialAdScene : MonoBehaviour
{
   // public string keyInter = "2453819458017049_2454052164660445";
    public string keyInter = "";
    private InterstitialAd interstitialAd;
    private bool isLoaded;
#pragma warning disable 0414
    private bool didClose;
#pragma warning restore 0414
    // UI elements in scene
    //public Text statusLabel;

    private void Awake()
    {
       
    }
    public void InisilizeAndLoadInst()
    {
        if (!AdUtility.IsInitialized())
        {
            AdUtility.Initialize();
        }
        LoadInterstitial();
    }
    private void Start()
    {
       
    }

    // Load button
    public void LoadInterstitial()
    {
       //s statusLabel.text = "Loading interstitial ad...";

        // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        interstitialAd = new InterstitialAd(keyInter);
        
        interstitialAd.Register(gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        interstitialAd.InterstitialAdDidLoad = delegate () {
            Debug.Log("Interstitial ad loaded.");
            isLoaded = true;
            didClose = false;
            string isAdValid = interstitialAd.IsValid() ? "valid" : "invalid";
           // statusLabel.text = "Ad loaded and is " + isAdValid + ". Click show to present!";
        };
        interstitialAd.InterstitialAdDidFailWithError = delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
           // statusLabel.text = "Interstitial ad failed to load. Check console for details.";
        };
        interstitialAd.InterstitialAdWillLogImpression = delegate () {
            Debug.Log("Interstitial ad logged impression.");
        };
        interstitialAd.InterstitialAdDidClick = delegate () {
            Debug.Log("Interstitial ad clicked.");
        };
        interstitialAd.InterstitialAdDidClose = delegate() {
            Debug.Log("Interstitial ad did close.");
            didClose = true;
            if (interstitialAd != null) {
                interstitialAd.Dispose();
            }
        };

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Interstitial activity has
         * been destroyed without being properly closed. This can happen if an
         * app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        interstitialAd.interstitialAdActivityDestroyed = delegate() {
            if (!didClose) {
                Debug.Log("Interstitial activity destroyed without being closed first.");
                Debug.Log("Game should resume.");
            }
        };
#endif

        // Initiate the request to load the ad.
        interstitialAd.LoadAd();
    }

    // Show button
    public void ShowInterstitial()
    {
        if(keyInter!="")
        {
            if (isLoaded)
            {

                interstitialAd.Show();
                isLoaded = false;
                // statusLabel.text = "";
            }
            else
            {
                //  statusLabel.text = "Ad not loaded. Click load to request an ad.";
                LoadInterstitial();
            }
        }
       
    }

    void OnDestroy()
    {
        // Dispose of interstitial ad when the scene is destroyed
        if (interstitialAd != null) {
            interstitialAd.Dispose();
        }
        Debug.Log("InterstitialAdTest was destroyed!");
    }

    // Next button
    public void NextScene()
    {
        SceneManager.LoadScene("AdViewScene");
    }
}
