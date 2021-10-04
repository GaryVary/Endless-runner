using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class ScoreObject : BaseObject
    {
        [SerializeField]
        private int _scoreValue = 100;

        public int ScoreValue
        {
            get
            {
                return _scoreValue;
            }
        }
    }
}
