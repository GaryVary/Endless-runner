using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIScoreRow : MonoBehaviour
    {
        [SerializeField]
        private Text _placeOnLeaderboard;
        [SerializeField]
        private Text _nickname;
        [SerializeField]
        private Text _score;

        public string PlaceOnLeaderboard
        {
            set
            {
                _placeOnLeaderboard.text = value;
            }
        }

        public string Nickname
        {
            set
            {
                _nickname.text = value;
            }
        }

        public string Score
        {
            set
            {
                _score.text = value;
            }
        }
    }
}