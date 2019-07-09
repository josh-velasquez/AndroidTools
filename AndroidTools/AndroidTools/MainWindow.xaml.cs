using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Windows;

namespace AndroidTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void RunScript(string commands)
        {
            try
            {
                var actualCommand = commands.Split('\n');
                using (var powerShell = PowerShell.Create())
                {
                    foreach (var command in actualCommand)
                    {
                        powerShell.AddScript(command);
                    }
                    powerShell.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void OnShowNavBarClick(object sender, RoutedEventArgs e)
        {
            var showNavBarCommand = "adb shell wm overscan 0,0,0,0";
            RunScript(showNavBarCommand);
        }

        private void OnHideNavBarClick(object sender, RoutedEventArgs e)
        {
            var hideNavBarCommand = "adb shell wm overscan 0,0,0,-100";
            RunScript(hideNavBarCommand);
        }

        private void OnWifiOnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append("adb shell input keyevent 20 & adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnWifiOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append("adb shell input keyevent 20 & adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell settings put global airplane_mode_on 1\n");
            stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell settings put global airplane_mode_on 0\n");
            stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            RunScript(stringBuilder.ToString());
        }

        private void OnAllowDrawOverAppClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.settings.APPLICATION_DETAILS_SETTINGS package:" + DrawOverAppsSource.Text + "\n");
            stringBuilder.Append("sleep 5\n");
            stringBuilder.Append("adb shell input tap 160 1760\n");
            stringBuilder.Append("sleep 1\n");
            stringBuilder.Append("adb shell input tap 175 335");
        }

        private void OnUninstallAppClick(object sender, RoutedEventArgs e)
        {
            var uninstallCommand = "adb uninstall " + UninstallSource.Text;
            RunScript(uninstallCommand);
        }

        private void OnDisbaleUsbDebuggingClick(object sender, RoutedEventArgs e)
        {
            var disableUsbDebugging = "adb shell settings put global adb_enabled 0";
            RunScript(disableUsbDebugging);
        }

        private void OnSendCustomBroadcastClick(object sender, RoutedEventArgs e)
        {
            var broadcast = "adb shell am broadcast -a " + BroadcastInput.Text;
            RunScript(broadcast);
        }

        private void OnSetHomeAppClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.settings.HOME_SETTINGS\n");
            stringBuilder.Append("sleep 2\n");
            stringBuilder.Append("for n in {1..4}; do adb shell input keyevent 20; done\n");
            stringBuilder.Append("adb shell input keyevent 66\n");
            stringBuilder.Append("adb shell input keyevent 20\n");
            stringBuilder.Append("adb shell input keyevent 66");
            RunScript(stringBuilder.ToString());
        }

        private void OnEnableBluetoothClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.bluetooth.adapter.action.REQUEST_ENABLE\n");
            stringBuilder.Append("sleep 2\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 66\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisableBluetoothClick(object sender, RoutedEventArgs e)
        {
            /**
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.bluetooth.adapter.action.REQUEST_DISABLE\n");
            stringBuilder.Append("sleep 2\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 66\n");
            RunScript(stringBuilder.ToString());
    */
        }

        private void OnEnableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.ACCESS_COARSE_LOCATION\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.ACCESS_COARSE_LOCATION\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnEnablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.READ_PHONE_STATE");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.READ_PHONE_STATE");
            RunScript(stringBuilder.ToString());
        }

        private void OnEnableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnInstallAppClick(object sender, RoutedEventArgs e)
        {
            var installCommand = "adb install " + InstallSource.Text;
            RunScript(installCommand);
        }

        private void OnPushClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb push " + TransferSource.Text + " /mnt/sdcard/Android/data/" + TransferDestination.Text;
            RunScript(transferCommand);
        }

        private void OnPullClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb pull /mnt/sdcard/Android/data/" + TransferSource.Text + " " + TransferDestination.Text;
            RunScript(transferCommand);
        }

        private void OnRebootClick(object sender, RoutedEventArgs e)
        {
            var rebootCommand = "adb reboot";
            RunScript(rebootCommand);
        }

        private void OnScreenshotClick(object sender, RoutedEventArgs e)
        {
            var screenshotCommand = "adb shell screencap /mnt/sdcard/Pictures/Screenshots/" + ScreenshotName.Text + ".png";
            RunScript(screenshotCommand);
        }

        private void OnRecordScreenClick(object sender, RoutedEventArgs e)
        {
            var duration = int.Parse(ScreenRecordDuration.Text);
            var recordScreenCommand = "adb shell screenrecord --time-limit " + duration + " /mnt/sdcard/Movies/" + ScreenRecordPath.Text + ".mp4";
            RunScript(recordScreenCommand);
        }
    }
}
