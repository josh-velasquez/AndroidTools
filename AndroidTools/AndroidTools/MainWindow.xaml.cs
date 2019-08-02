using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace AndroidTools
{
    // Input codes
    // https://stackoverflow.com/questions/7789826/adb-shell-input-events
    // https://gist.github.com/Pulimet/5013acf2cd5b28e55036c82c91bd56d8

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetDeviceName();
        }

        private void GetDeviceName()
        {
            var deviceName = "None";
            try
            {
                var deviceInfoCommand = "adb devices -l";
                var results = RunScriptWithOutput(deviceInfoCommand);
                foreach (var name in results)
                {
                    if (name.Contains("device product"))
                    {
                        deviceName = name;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            ConnectedDevice.Text = deviceName;
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

        private List<string> RunScriptWithOutput(string commands)
        {
            var powerShellOutputsList = new List<string>();
            try
            {
                var actualCommand = commands.Split('\n');
                var test = new StringBuilder();
                using (var powerShell = PowerShell.Create())
                {
                    foreach (var command in actualCommand)
                    {
                        powerShell.AddScript(command);
                    }
                    var powerShellOutput = powerShell.Invoke();
                    if (powerShellOutput.Count != 0)
                    {
                        foreach (var line in powerShellOutput)
                        {
                            powerShellOutputsList.Add(line.ToString());
                        }
                    }
                }

                return powerShellOutputsList;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return powerShellOutputsList;
        }

        private void Test(string commands)
        {
            using (Process proc = new Process())
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("F:\\Android\\android-sdk\\platform-tools\\adb.exe");
                procStartInfo.RedirectStandardInput = true;
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                proc.StartInfo = procStartInfo;
                proc.Start();
                var actualCommand = commands.Split('\n');
                foreach (var line in actualCommand)
                {
                    proc.StandardInput.WriteLine(line);
                }
                proc.WaitForExit();
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
            stringBuilder.Append(
                "adb shell input keyevent 23 \"&\" adb shell input keyevent 19");
            RunScript(stringBuilder.ToString());
        }

        private void OnWifiOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append(
                "adb shell input keyevent 23 \"&\" adb shell input keyevent 19");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            //stringBuilder.Append("adb shell settings put global airplane_mode_on 1\n");
            //stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            stringBuilder.Append("adb shell am start -a android.settings.AIRPLANE_MODE_SETTINGS\n");
            stringBuilder.Append("adb shell input keyevent 19\n");
            stringBuilder.Append("adb shell input keyevent 23\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            //stringBuilder.Append("adb shell settings put global airplane_mode_on 0\n");
            //stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            stringBuilder.Append("adb shell am start -a android.settings.AIRPLANE_MODE_SETTINGS\n");
            stringBuilder.Append("adb shell input keyevent 19\n");
            stringBuilder.Append("adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnAllowDrawOverAppClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.settings.APPLICATION_DETAILS_SETTINGS package:" + AppName.Text + "\n");
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
            
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.settings.BLUETOOTH_SETTINGS\n");
            stringBuilder.Append("adb shell input keyevent 19\n");
            stringBuilder.Append("adb shell input keyevent 23\n");
            RunScript(stringBuilder.ToString());
    
        }

        private void OnEnableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.ACCESS_COARSE_LOCATION\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.ACCESS_COARSE_LOCATION\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnEnablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.READ_PHONE_STATE");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.READ_PHONE_STATE");
            RunScript(stringBuilder.ToString());
        }

        private void OnEnableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnDisableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnInstallAppClick(object sender, RoutedEventArgs e)
        {
            var installCommand = "adb install " + InstallSource.Text;
            RunScript(installCommand);
        }

        private void OnPushClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb push \"" + TransferSource.Text + "\" \"" + TransferDestination.Text + "\"";
            RunScript(transferCommand);
        }

        private void OnPullClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb pull \"" + TransferSource.Text + "\" \"" + TransferDestination.Text + "\"";
            RunScript(transferCommand);
        }

        private void OnRebootClick(object sender, RoutedEventArgs e)
        {
            var rebootCommand = "adb reboot";
            RunScript(rebootCommand);
        }

        private void OnScreenshotClick(object sender, RoutedEventArgs e)
        {
            var screenshotCommand = "adb shell screencap /mnt/sdcard/Pictures/" + ScreenshotName.Text + ".png";
            RunScript(screenshotCommand);
        }

        private void OnRecordScreenClick(object sender, RoutedEventArgs e)
        {
            var duration = int.Parse(ScreenRecordDuration.Text);
            var recordScreenCommand = "adb shell screenrecord --time-limit " + duration + " /mnt/sdcard/Movies/" + ScreenRecordPath.Text + ".mp4";
            RunScript(recordScreenCommand);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            var backCommand = "adb shell input keyevent 4";
            RunScript(backCommand);
        }

        private void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            var homeCommand = "adb shell input keyevent 3";
            RunScript(homeCommand);
        }

        private void OnMultitaskButtonClick(object sender, RoutedEventArgs e)
        {
            var multitaskCommand = "adb shell input keyevent 187";
            RunScript(multitaskCommand);
        }

        private void OnBrightenClick(object sender, RoutedEventArgs e)
        {
            var brightenCommand = "adb shell input keyevent 221";
            RunScript(brightenCommand);
        }

        private void OnDimClick(object sender, RoutedEventArgs e)
        {
            var dimCommand = "adb shell input keyevent 220";
            RunScript(dimCommand);
        }

        private void OnPowerClick(object sender, RoutedEventArgs e)
        {
            var powerCommand = "adb shell input keyevent 26";
            RunScript(powerCommand);
        }

        private void OnVolumeUpClick(object sender, RoutedEventArgs e)
        {
            var upVolumeCommand = "adb shell input keyevent 24";
            RunScript(upVolumeCommand);
        }

        private void OnVolumeDownClick(object sender, RoutedEventArgs e)
        {
            var downVolumeCommand = "adb shell input keyevent 25";
            RunScript(downVolumeCommand);
        }

        private void OnExpandStatusBarClick(object sender, RoutedEventArgs e)
        {
            var expandCommand = "adb shell cmd statusbar expand-settings";
            RunScript(expandCommand);
        }

        private void OnCollapseStatusBarClick(object sender, RoutedEventArgs e)
        {
            var collapseCommand = "adb shell cmd statusbar collapse";
            RunScript(collapseCommand);
        }

        private void OnExecuteAdbCommandClick(object sender, RoutedEventArgs e)
        {
            RunScript(AdbCommand.Text);
        }

        private void OnGetSharedPrefClick(object sender, RoutedEventArgs e)
        {
            var sharedPrefCommand = SharedPrefDestination.Text;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell\n");
            stringBuilder.Append("run-as " + SharedPrefAppName.Text + "\n");
            stringBuilder.Append("cd shared_prefs/\n");
            stringBuilder.Append("cat " + SharedPrefAppName.Text + ".PREFERENCES.xml\n");
            stringBuilder.Append("exit\n");
            stringBuilder.Append("exit");
            var result = RunScriptWithOutput(stringBuilder.ToString());
            Debug.WriteLine("RESULT: " + result);
        }

        private void OnRefreshDeviceClick(object sender, RoutedEventArgs e)
        {
            GetDeviceName();
        }

        private void OnRebootBootloaderClick(object sender, RoutedEventArgs e)
        {
            var rebootCommand = "adb reboot bootloader";
            RunScript(rebootCommand);
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            var settingsCommand = "adb shell am start -a android.settings.SETTINGS";
            RunScript(settingsCommand);
        }
    }
}
