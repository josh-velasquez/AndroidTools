using AndroidTools.Model;
using AndroidTools.Utils;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

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
            SetComboBoxPermission();
            GetDeviceNameAndVersion();
        }

        private void SetComboBoxPermission()
        {
            PermissionComboBox.Items.Add("-- select a permission --");
            PermissionComboBox.SelectedIndex = 0;
            foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
            {
                PermissionComboBox.Items.Add(permission);
            }
        }

        private void GetDeviceNameAndVersion()
        {
            var deviceName = "None";
            var deviceVersion = "N/A";
            try
            {
                var deviceInfoCommand = EnumUtil.GetEnumDescription(Command.ListAndroidDevices);
                var results = PowerShellUtil.RunScriptWithOutput(deviceInfoCommand);
                foreach (var name in results)
                {
                    if (name.Contains("device product"))
                    {
                        deviceName = name;
                    }
                }

                var deviceVersionCommand = EnumUtil.GetEnumDescription(Command.AndroidDeviceVersion);
                deviceVersion = PowerShellUtil.RunScriptWithOutput(deviceVersionCommand)[0];

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            ConnectedDevice.Text = deviceName;
            AndroidVersion.Text = deviceVersion;
        }

        private void OnShowNavBarClick(object sender, RoutedEventArgs e)
        {
            var showNavBarCommand = EnumUtil.GetEnumDescription(Command.ShowOverScan);
            PowerShellUtil.RunScript(showNavBarCommand);
        }

        private void OnHideNavBarClick(object sender, RoutedEventArgs e)
        {
            var hideNavBarCommand = EnumUtil.GetEnumDescription(Command.HideOverScan);
            PowerShellUtil.RunScript(hideNavBarCommand);
        }

        private void OnWifiOnClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append(
                "adb shell input keyevent 23 \"&\" adb shell input keyevent 19");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnWifiOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append(
                "adb shell input keyevent 23 \"&\" adb shell input keyevent 19");
            PowerShellUtil.RunScript(stringBuilder.ToString());
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
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOffClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            //stringBuilder.Append("adb shell settings put global airplane_mode_on 0\n");
            //stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            stringBuilder.Append("adb shell am start -a android.settings.AIRPLANE_MODE_SETTINGS\n");
            stringBuilder.Append("adb shell input keyevent 19\n");
            stringBuilder.Append("adb shell input keyevent 23");
            PowerShellUtil.RunScript(stringBuilder.ToString());
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
            var uninstallCommand = EnumUtil.GetEnumDescription(Command.UninstallApp) + UninstallSource.Text;
            PowerShellUtil.RunScript(uninstallCommand);
        }

        private void OnDisbaleUsbDebuggingClick(object sender, RoutedEventArgs e)
        {
            var disableUsbDebugging = EnumUtil.GetEnumDescription(Command.DisableUsbDebugging);
            PowerShellUtil.RunScript(disableUsbDebugging);
        }

        private void OnSendCustomBroadcastClick(object sender, RoutedEventArgs e)
        {
            var broadcast = EnumUtil.GetEnumDescription(Command.SendBroadcast) + BroadcastInput.Text;
            PowerShellUtil.RunScript(broadcast);
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
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnEnableBluetoothClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.bluetooth.adapter.action.REQUEST_ENABLE\n");
            stringBuilder.Append("sleep 2\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 22\n");
            stringBuilder.Append("adb shell input keyevent 66\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnDisableBluetoothClick(object sender, RoutedEventArgs e)
        {
            
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell am start -a android.settings.BLUETOOTH_SETTINGS\n");
            stringBuilder.Append("adb shell input keyevent 19\n");
            stringBuilder.Append("adb shell input keyevent 23\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnEnableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.ACCESS_COARSE_LOCATION\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnDisableLocationClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.ACCESS_COARSE_LOCATION\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnEnablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.READ_PHONE_STATE");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnDisablePhoneClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.READ_PHONE_STATE");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnEnableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm grant " + AppName.Text + " android.permission.READ_EXTERNAL_STORAGE\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnDisableStorageClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm revoke " + AppName.Text + " android.permission.READ_EXTERNAL_STORAGE\n");
            PowerShellUtil.RunScript(stringBuilder.ToString());
        }

        private void OnInstallAppClick(object sender, RoutedEventArgs e)
        {
            var installCommand = EnumUtil.GetEnumDescription(Command.InstallApp) + InstallSource.Text;
            PowerShellUtil.RunScript(installCommand);
        }

        private void OnPushClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb push \"" + TransferSource.Text + "\" \"" + TransferDestination.Text + "\"";
            PowerShellUtil.RunScript(transferCommand);
        }

        private void OnPullClick(object sender, RoutedEventArgs e)
        {
            var transferCommand = "adb pull \"" + TransferSource.Text + "\" \"" + TransferDestination.Text + "\"";
            PowerShellUtil.RunScript(transferCommand);
        }

        private void OnRebootClick(object sender, RoutedEventArgs e)
        {
            var rebootCommand = "adb reboot";
            PowerShellUtil.RunScript(rebootCommand);
        }

        private void OnScreenshotClick(object sender, RoutedEventArgs e)
        {
            var screenshotCommand = "adb shell screencap /mnt/sdcard/Pictures/" + ScreenshotName.Text + ".png";
            PowerShellUtil.RunScript(screenshotCommand);
        }

        private void OnRecordScreenClick(object sender, RoutedEventArgs e)
        {
            var duration = int.Parse(ScreenRecordDuration.Text);
            var recordScreenCommand = "adb shell screenrecord --time-limit " + duration + " /mnt/sdcard/Movies/" + ScreenRecordPath.Text + ".mp4";
            PowerShellUtil.RunScript(recordScreenCommand);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            var backCommand = EnumUtil.GetEnumDescription(Command.BackButton);
            PowerShellUtil.RunScript(backCommand);
        }

        private void OnHomeButtonClick(object sender, RoutedEventArgs e)
        {
            var homeCommand = EnumUtil.GetEnumDescription(Command.HomeButton);
            PowerShellUtil.RunScript(homeCommand);
        }

        private void OnMultitaskButtonClick(object sender, RoutedEventArgs e)
        {
            var multitaskCommand = EnumUtil.GetEnumDescription(Command.MultitaskButton);
            PowerShellUtil.RunScript(multitaskCommand);
        }

        private void OnBrightenClick(object sender, RoutedEventArgs e)
        {
            var brightenCommand = EnumUtil.GetEnumDescription(Command.Brighten);
            PowerShellUtil.RunScript(brightenCommand);
        }

        private void OnDimClick(object sender, RoutedEventArgs e)
        {
            var dimCommand = EnumUtil.GetEnumDescription(Command.Dim);
            PowerShellUtil.RunScript(dimCommand);
        }

        private void OnPowerClick(object sender, RoutedEventArgs e)
        {
            var powerCommand = EnumUtil.GetEnumDescription(Command.PowerButton);
            PowerShellUtil.RunScript(powerCommand);
        }

        private void OnVolumeUpClick(object sender, RoutedEventArgs e)
        {
            var upVolumeCommand = EnumUtil.GetEnumDescription(Command.UpVolumeButton);
            PowerShellUtil.RunScript(upVolumeCommand);
        }

        private void OnVolumeDownClick(object sender, RoutedEventArgs e)
        {
            var downVolumeCommand = EnumUtil.GetEnumDescription(Command.DownVolumeButton);
            PowerShellUtil.RunScript(downVolumeCommand);
        }

        private void OnExpandStatusBarClick(object sender, RoutedEventArgs e)
        {
            var expandCommand = EnumUtil.GetEnumDescription(Command.ExpandStatusBar);
            PowerShellUtil.RunScript(expandCommand);
        }

        private void OnCollapseStatusBarClick(object sender, RoutedEventArgs e)
        {
            var collapseCommand = EnumUtil.GetEnumDescription(Command.CollapseStatusBar);
            PowerShellUtil.RunScript(collapseCommand);
        }

        private void OnExecuteAdbCommandClick(object sender, RoutedEventArgs e)
        {
            PowerShellUtil.RunScript(AdbCommand.Text);
        }

        private void OnRefreshDeviceClick(object sender, RoutedEventArgs e)
        {
            GetDeviceNameAndVersion();
        }

         private void OnCallClick(object sender, RoutedEventArgs e)
         {
             var callCommand = EnumUtil.GetEnumDescription(Command.CallNumber) + Number.Text;
             PowerShellUtil.RunScript(callCommand);
         }

        private void OnMessageClick(object sender, RoutedEventArgs e)
        {
            var messageCommand = "adb shell am start -a android.intent.action.SENDTO -d sms:+" + Number.Text + " --es sms_body \"" + Message.Text + "\" --ez exit_on_sent true";
            PowerShellUtil.RunScript(messageCommand);
        }

        private void OnCutClick(object sender, RoutedEventArgs e)
        {
            var cutCommand = EnumUtil.GetEnumDescription(Command.Cut);
            PowerShellUtil.RunScript(cutCommand);
        }

        private void OnCopyClick(object sender, RoutedEventArgs e)
        {
            var copyCommand = EnumUtil.GetEnumDescription(Command.Copy);
            PowerShellUtil.RunScript(copyCommand);
        }

        private void OnPasteClick(object sender, RoutedEventArgs e)
        {
            var pasteCommand = EnumUtil.GetEnumDescription(Command.Paste);
            PowerShellUtil.RunScript(pasteCommand);
        }

        private void OnInsertTextClick(object sender, RoutedEventArgs e)
        {
            var text = InsertText.Text.Replace(" ", "%s");
            var insertTextCommand = "adb shell input text \"" + text + "\"";
            PowerShellUtil.RunScript(insertTextCommand);
        }

        private void OnRebootBootloaderClick(object sender, RoutedEventArgs e)
        {
            var rebootCommand = EnumUtil.GetEnumDescription(Command.RebootBootloader);
            PowerShellUtil.RunScript(rebootCommand);
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            var settingsCommand = EnumUtil.GetEnumDescription(Command.Settings);
            PowerShellUtil.RunScript(settingsCommand);
        }

        private void OnSeeLogCatClick(object sender, RoutedEventArgs e)
        {
            Logcat logcatWindow = new Logcat(filterTag.Text);
            logcatWindow.Show();
        }
    }
}
