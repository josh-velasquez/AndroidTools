using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidTools.Model
{
    enum Command
    {
        [Description("adb shell wm overscan 0,0,0,0")]
        ShowOverScan,
        [Description("adb shell wm overscan 0,0,0,-100")]
        HideOverScan,
        [Description("adb devices -l")]
        ListAndroidDevices,
        [Description("adb shell getprop ro.build.version.release")]
        AndroidDeviceVersion,
        [Description("adb reboot")]
        Reboot,
        [Description("adb shell input keyevent 4")]
        BackButton,
        [Description("adb shell input keyevent 3")]
        HomeButton,
        [Description("adb shell input keyevent 187")]
        MultitaskButton,
        [Description("adb shell input keyevent 221")]
        Brighten,
        [Description("adb shell input keyevent 220")]
        Dim,
        [Description("adb shell input keyevent 26")]
        PowerButton,
        [Description("adb shell input keyevent 24")]
        UpVolumeButton,
        [Description("adb shell input keyevent 25")]
        DownVolumeButton,
        [Description("adb shell cmd statusbar expand-setting")]
        ExpandStatusBar,
        [Description("adb shell cmd statusbar collapse")]
        CollapseStatusBar,
        [Description("adb shell am start android.intent.action.CALL -d tel:+")]
        CallNumber,
        [Description("adb shell input keyevent 277")]
        Cut,
        [Description("adb shell input keyevent 278")]
        Copy,
        [Description("adb shell input keyevent 279")]
        Paste,
        [Description("adb reboot bootloader")]
        RebootBootloader,
        [Description("adb shell am start -a android.settings.SETTINGS")]
        Settings,
        [Description("adb install ")]
        InstallApp,
        [Description("adb shell am broadcast -a ")]
        SendBroadcast,
        [Description("adb shell settings put global adb_enabled 0")]
        DisableUsbDebugging,
        [Description("adb uninstall ")]
        UninstallApp
    }
}
