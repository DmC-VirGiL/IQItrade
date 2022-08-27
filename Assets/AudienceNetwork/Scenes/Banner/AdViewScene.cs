using UnityEngine;
using AudienceNetwork;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudienceNetwork.Utility;
using SimpleJSON;
using UnityEngine.Networking;
using System.Collections;

public class AdViewScene : MonoBehaviour
{
    private AdView adView;
    private AdPosition currentAdViewPosition;
    private ScreenOrientation currentScreenOrientation;
    //public Text statusLabel;
                        
    //private string key = "2453819458017049_2453821358016859";
    private string key = "";
    public InterstitialAdScene interStitial;
    void OnDestroy()
    {
        // Dispose of banner ad when the scene is destroyed
        if (adView) {
            adView.Dispose();
        }
        Debug.Log("AdViewTest was destroyed!");
    }

    private void Awake()
    {
       
    }

    private void Start()
    {
        string url5 = "https://gamelovin.com/tradeoption/Trading/Userprocess/getsettings";
        string json1 = "{\"response\":\"Get\"}";
        StartCoroutine(PostgetsettingCoroutine(url5, json1));

       
    }
    public IEnumerator PostgetsettingCoroutine(string url, string json)
    {

        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(json);

        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();

        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest www =
            new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.LogError(string.Format("{0}: {1}", www.url, www.error));
            ///   error.text = string.Format("{0}: {1}", www.url, www.error);
            Debug.Log("Network Fail");
        }
        else
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text.ToString());
            // Debug.Log(jsonNode["status"]+"and"+ jsonNode["message"]);

            if (jsonNode["status"] == "0")
            {
                if(jsonNode["banner"]!="")
                {
                    Debug.Log("Banner:"+ jsonNode["banner"]);
                    key = jsonNode["banner"];
                    if (!AdUtility.IsInitialized())
                    {
                        AdUtility.Initialize();
                    }

                    LoadBanner();
                }
                if(jsonNode["interstitial"]!="")
                {
                    Debug.Log("interstitial:" + jsonNode["interstitial"]);
                    interStitial.keyInter = jsonNode["interstitial"];
                    interStitial.InisilizeAndLoadInst();
                }
               
            }
            else
            {
                Debug.Log("fail to get setting");
            }


        }




    }

    private void Update()
    {
        if(PlayerPrefs.GetInt("Login")==0)
        {
            adView.Dispose();
        }
    }
    // Load Banner button
    public void LoadBanner()
    {
        if (adView) {
            adView.Dispose();
        }

        //statusLabel.text = "Loading Banner...";

        // Create a banner's ad view with a unique placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        adView = new AdView(key, AdSize.BANNER_HEIGHT_50);

        adView.Register(gameObject);
        currentAdViewPosition = AdPosition.BOTTOM;

        // Set delegates to get notified on changes or when the user interacts with the ad.
        adView.AdViewDidLoad = delegate() {
            currentScreenOrientation = Screen.orientation;
            double height = AudienceNetwork.Utility.AdUtility.Convert(Screen.height);
            this.adView.Show(height - 50);
           // adView.Show(600);

            string isAdValid = adView.IsValid() ? "valid" : "invalid";
           // statusLabel.text = "Banner loaded and is " + isAdValid + ".";
        };
        adView.AdViewDidFailWithError = delegate (string error) {
           // statusLabel.text = "Banner failed to load with error: " + error;
        };
        adView.AdViewWillLogImpression = delegate () {
           // statusLabel.text = "Banner logged impression.";
        };
        adView.AdViewDidClick = delegate () {
           // statusLabel.text = "Banner clicked.";
        };

        // Initiate a request to load an ad.
        adView.LoadAd();
    }

    // Next button
    public void NextScene()
    {
        SceneManager.LoadScene("RewardedVideoAdScene");
    }

    // Change button
    // Change the position of the ad view when button is clicked
    // ad view is at top: move it to bottom
    // ad view is at bottom: move it to 100 pixels along y-axis
    // ad view is at custom position: move it to the top
    public void ChangePosition()
    {
        switch (currentAdViewPosition)
        {
            case AdPosition.TOP:
                SetAdViewPosition(AdPosition.BOTTOM);
                break;
            case AdPosition.BOTTOM:
                SetAdViewPosition(AdPosition.BOTTOM);
                break;
            case AdPosition.CUSTOM:
                SetAdViewPosition(AdPosition.BOTTOM);
                break;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if (adView && Screen.orientation != currentScreenOrientation)
        {
            SetAdViewPosition(currentAdViewPosition);
            currentScreenOrientation = Screen.orientation;
        }
    }

    private void SetAdViewPosition(AdPosition adPosition)
    {
        switch (adPosition)
        {
            case AdPosition.TOP:
                adView.Show(AdPosition.BOTTOM);
                currentAdViewPosition = AdPosition.BOTTOM;
                break;
            case AdPosition.BOTTOM:
                adView.Show(AdPosition.BOTTOM);
                currentAdViewPosition = AdPosition.BOTTOM;
                break;
            case AdPosition.CUSTOM:
                adView.Show(600);
                
                currentAdViewPosition = AdPosition.BOTTOM;
                break;
        }
    }
}
