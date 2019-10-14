using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidTools.Model
{
    enum Permission
    {
        [Description("android.permission.ACCESS_FINE_LOCATION")]
        AccessFineLocation,
        [Description("android.permission.ACCESS_COARSE_LOCATION")]
        AccessCoarseLocation,
        AccessWifiState,
        Location,
        Phone,
        Storage,
        DrawOverApps,
        Bluetooth,
        Internet,
        NFC,
        Camera,
        [Description("android.permission.READ_PHONE_STATE")]
        ReadPhoneState,
        [Description("android.permission.WRITE_EXTERNAL_STORAGE")]
        WriteExternalStorage,
        [Description("android.permission.READ_EXTERNAL_STORAGE")]
        ReadExternalStorage
    }
}
