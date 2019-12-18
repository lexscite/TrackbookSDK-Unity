using UnityEngine;

namespace Trackbook
{
    public class TrackbookSettings : ScriptableObject
    {
        [SerializeField]
        public string appId;
        [SerializeField]
        public string secret;
        [SerializeField]
        public string host;
        [SerializeField]
        public string postScheduleFileName;
    }
}