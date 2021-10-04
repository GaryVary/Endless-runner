using Assets.Scripts.Delegates;
using Assets.Scripts.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Scripts.Credentials;

namespace Assets.Scripts.Web
{
    public static class LeaderboardClient
    {
        private const string LeaderboardUrl = "http://dreamlo.com/lb/";
        private const string PublicLeaderboardKey = "61539a5a8f40bb0e2877be8e";

        public static event GenericDelegate<List<Entry>> ScoreDownloadedEvent;

        public static IEnumerator UploadScore(string nickname, string value)
        {
            using (var uploadScoreRequest = UnityWebRequest.Get(LeaderboardUrl + PrivateCredentials.PrivateLeaderBoardUrl + "/add/" + nickname + "/" + value))
            {
                yield return uploadScoreRequest.SendWebRequest();
            }
        }

        public static IEnumerator GetTopFiveScore()
        {
            using (var getScoreRequest = UnityWebRequest.Get(LeaderboardUrl + PublicLeaderboardKey + "/json/5"))
            {
                yield return getScoreRequest.SendWebRequest();

                if (getScoreRequest.error == null)
                {
                    var deserializedRequestResult = JsonUtility.FromJson<Root>(getScoreRequest.downloadHandler.text).dreamlo.leaderboard.entry;

                    ScoreDownloadedEvent.Invoke(deserializedRequestResult);
                }
            }
        }
    }
}
