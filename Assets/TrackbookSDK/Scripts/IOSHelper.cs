#if UNITY_IOS
using System.Runtime.InteropServices;
using UnityEngine;

namespace FakesbookSDK.Utilities
{
    // Unmanaged code hepler (see Assets/Plugins/iOS/IOSHelper.mm)
    // Mocks iPhone 6 in Editor
    internal static class IOSHelper
    {
        [DllImport("__Internal")]
        private static extern string _getPkgName();
        private static string _packageName;
        internal static string PackageName
        {
            get
            {
                if (_packageName == null)
                {
                    if (Application.isEditor)
                    {
                        _packageName = Application.identifier;
                    }
                    else
                    {
                        _packageName = _getPkgName();
                    }
                }

                return _packageName;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getPkgVersionCode();
        private static string _packageVersionCode;
        internal static string PackageVersionCode
        {
            get
            {
                if (_packageVersionCode == null)
                {
                    if (Application.isEditor)
                    {
                        _packageVersionCode = "99999";
                    }
                    else
                    {
                        _packageVersionCode = _getPkgVersionCode();
                    }
                }

                return _packageVersionCode;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getPkgShortVersionName();
        private static string _shortVersionName;
        internal static string ShortVersionName
        {
            get
            {
                if (_shortVersionName == null)
                {
                    if (Application.isEditor)
                    {
                        _shortVersionName = Application.version;
                    }
                    else
                    {
                        _shortVersionName = _getPkgShortVersionName();
                    }
                }

                return _shortVersionName;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getOSVersion();
        private static string _osVersion;
        internal static string OSVersion
        {
            get
            {
                if (_osVersion == null)
                {
                    if (Application.isEditor)
                    {
                        _osVersion = "12.4.2";
                    }
                    else
                    {
                        _osVersion = _getOSVersion();
                    }
                }

                return _osVersion;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getDeviceModelName();
        private static string _deviceModelName;
        internal static string DeviceModelName
        {
            get
            {
                if (_deviceModelName == null)
                {
                    if (Application.isEditor)
                    {
                        _deviceModelName = "iPhone7,2";
                    }
                    else
                    {
                        _deviceModelName = _getDeviceModelName();
                    }
                }

                return _deviceModelName;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getLocale();
        private static string _locale;
        internal static string Locale
        {
            get
            {
                if (_locale == null)
                {
                    if (Application.isEditor)
                    {
                        _locale = "en_US";
                    }
                    else
                    {
                        _locale = _getLocale();
                    }
                }

                return _locale;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getTimeZone();
        private static string _timeZone;
        internal static string TimeZone
        {
            get
            {
                if (_timeZone == null)
                {
                    if (Application.isEditor)
                    {
                        _timeZone = "Europe/Moscow";
                    }
                    else
                    {
                        _timeZone = _getTimeZone();
                    }
                }

                return _timeZone;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getTimeZoneAbr();
        private static string _timeZoneAbbreviation;
        internal static string TimeZoneAbbreviation
        {
            get
            {
                if (_timeZoneAbbreviation == null)
                {
                    if (Application.isEditor)
                    {
                        _timeZoneAbbreviation = "GMT+3";
                    }
                    else
                    {
                        _timeZoneAbbreviation = _getTimeZoneAbr();
                    }
                }

                return _timeZoneAbbreviation;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getCarrierName();
        private static string _carrierName;
        internal static string CarrierName
        {
            get
            {
                if (_carrierName == null)
                {
                    if (Application.isEditor)
                    {
                        _carrierName = "Carrier";
                    }
                    else
                    {
                        _carrierName = _getCarrierName();
                    }
                }

                return _carrierName;
            }
        }

        [DllImport("__Internal")]
        private static extern string _getScreenDensity();
        private static string _screenDensity;
        internal static string ScreenDensity
        {
            get
            {
                if (_screenDensity == null)
                {
                    if (Application.isEditor)
                    {
                        _screenDensity = "2.00";
                    }
                    else
                    {
                        _screenDensity = _getScreenDensity();
                    }
                }

                return _screenDensity;
            }
        }

        [DllImport("__Internal")]
        private static extern int _getCPUCores();
        private static int? _cpuCores;
        internal static int CPUCores
        {
            get
            {
                if (_cpuCores == null)
                {
                    if (Application.isEditor)
                    {
                        _cpuCores = 2;
                    }
                    else
                    {
                        _cpuCores = _getCPUCores();
                    }
                }

                return (int)_cpuCores;
            }
        }

        [DllImport("__Internal")]
        private static extern float _getTotalDiskSpace();
        private static float? _totalDiskSpace;
        internal static float TotalDiskSpace
        {
            get
            {
                if (_totalDiskSpace == null)
                {
                    if (Application.isEditor)
                    {
                        _totalDiskSpace = 60;
                    }
                    else
                    {
                        _totalDiskSpace = _getTotalDiskSpace();
                    }
                }

                return (float)_totalDiskSpace;
            }
        }

        [DllImport("__Internal")]
        private static extern float _getRemainingDiskSpace();
        private static float? _remainingDiskSpace;
        internal static float RemainingDiskSpace
        {
            get
            {
                if (_remainingDiskSpace == null)
                {
                    if (Application.isEditor)
                    {
                        _remainingDiskSpace = 29;
                    }
                    else
                    {
                        _remainingDiskSpace = _getRemainingDiskSpace();
                    }
                }

                return (float)_remainingDiskSpace;
            }
        }
    }
}
#endif