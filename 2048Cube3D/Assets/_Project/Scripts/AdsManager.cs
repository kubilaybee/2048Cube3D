//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using GoogleMobileAds.Api;

//public class AdsManager : MonoBehaviour
//{
//    public Text bannerStatText, fullScreenStatText;

//    // test ads google admob link: https://developers.google.com/admob/android/test-ads
//    // Define the variables
//    private BannerView bannerAds;       //bannerReklam
//    private InterstitialAd intersAds;   //geçişReklam

//    public string appID = "";
//    public string intersID = "ca-app-pub-3940256099942544/1033173712";
//    public string bannerID = "ca-app-pub-3940256099942544/6300978111";
//    public AdPosition position;

//    private void Start()
//    {
//        MobileAds.Initialize(InitializationStatus => {});            // WTF Error!? **FIXED
//    }

//    public void BannerAds()
//    {
//        bannerAds = new BannerView(bannerID, AdSize.Banner, position);
//        bannerStatText.text = "Banner Created!";

//        AdRequest newAds = new AdRequest.Builder().Build();
//        bannerStatText.text = "Banner Request!";
//        bannerAds.LoadAd(newAds);

//        bannerAds.Show();

//        bannerStatText.text = "Banner Showed!";
//    }

//    public void IntersAds()
//    {
//        fullScreenStatText.text = "Full Screen !";
//        intersAds = new InterstitialAd(intersID);
//        AdRequest newAds = new AdRequest.Builder().Build();
//        fullScreenStatText.text = "Full Screen Request!";
//        intersAds.LoadAd(newAds);

//        fullScreenStatText.text = "IntersAds Loaded !";
//    }

//    public void IntersAdsShow()
//    {
//        if (intersAds.IsLoaded())
//        {
//            fullScreenStatText.text = "Full Screen Opening..";
//            intersAds.Show();
//        }
//        else
//        {
//            fullScreenStatText.text = "intersAds is not open !";
//        }
//    }

//    public void HideBanner()
//    {
//        bannerStatText.text = "Banner Ads Hided!";
//    }
//}
