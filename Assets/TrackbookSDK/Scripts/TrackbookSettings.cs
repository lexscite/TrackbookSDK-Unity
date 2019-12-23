using UnityEngine;

namespace Trackbook
{
    public class TrackbookSettings : ScriptableObject
    {
        [SerializeField]
        public string appId;
        [SerializeField]
        public string apiKey;
        [SerializeField]
        public string postScheduleFileName;
    }
}