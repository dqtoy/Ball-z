﻿using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

namespace AppAdvisory.BallX
{
    public class UnityAds : MonoBehaviour
    {

        public static UnityAds instance;
        public bool rewardZone;
        public bool isAdShowing = false;

        private bool rewardCoins = false;

        void Awake()
        {
            instance = this;

            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize("1792778", false);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Advertisement.Initialize("1792779", false);
            }
            else
            {
                Advertisement.Initialize("1792779", false);
            }

        }
        public void RewardingCoins(bool reward)
        {
            rewardCoins = reward;
        }

        public void ShowAd(string zone = "")
        {
#if UNITY_EDITOR
            //StartCoroutine(WaitForAd());
#endif
            isAdShowing = true;
            if (string.Equals(zone, ""))
                zone = null;
            else
                rewardZone = true;

            ShowOptions options = new ShowOptions();
            options.resultCallback = AdCallbackhandler;

            if (Advertisement.IsReady(zone))
            {
                Advertisement.Show(zone, options);
                Debug.Log("Show AD");
            }



        }

        void AdCallbackhandler(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("Ad Finished. Rewarding player...");
                    if (rewardZone)
                    {
                        if (rewardCoins)
                        {
                            Utils.AddCoins(20);
                            UIManager.UpdateShopCoins();
                            AppsFlyerMMP.WatchVideo();
                            rewardCoins = false;
                        }
                        else
                        {
                            GameManager.instance.ContinuePlaying();
                            UIManager.instance.OnContinuePlayingButton();
                        }
                        
                        isAdShowing = false;
                    }


                    break;
                case ShowResult.Skipped:
                    Debug.Log("Ad skipped. Son, I am dissapointed in you");
                    break;
                case ShowResult.Failed:
                    Debug.Log("I swear this has never happened to me before");
                    break;
            }
        }

        IEnumerator WaitForAd()
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            yield return null;

            while (Advertisement.isShowing)
                yield return null;

            Time.timeScale = currentTimeScale;
        }
    }
}
