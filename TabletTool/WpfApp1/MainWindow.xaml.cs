using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
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

        private void OnHideNavBar(object sender, RoutedEventArgs e)
        {
            string hideNavBarCommand = "adb shell wm overscan 0,0,0,-100";
            RunScript(hideNavBarCommand);
        }

        private void OnShowNavBar(object sender, RoutedEventArgs e)
        {
            string showNavBarCommand = "adb shell wm overscan 0,0,0,0";
            RunScript(showNavBarCommand);
        }

        private void OnAllowDrawOverApps(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.settings.APPLICATION_DETAILS_SETTINGS package:com.zephyrsleep.tablet\n");
            stringBuilder.Append("sleep 5\n");
            stringBuilder.Append("adb shell input tap 160 1760\n");
            stringBuilder.Append("sleep 1\n");
            stringBuilder.Append("adb shell input tap 175 335");
        }

        private void OnDisablePermissions(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.ACCESS_COARSE_LOCATION\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.READ_PHONE_STATE\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm revoke com.zephyrsleep.tablet android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnApplyPermissions(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.ACCESS_FINE_LOCATION\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.ACCESS_COARSE_LOCATION\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.READ_PHONE_STATE\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.WRITE_EXTERNAL_STORAGE\n");
            stringBuilder.Append("adb shell pm grant com.zephyrsleep.tablet android.permission.READ_EXTERNAL_STORAGE\n");
            RunScript(stringBuilder.ToString());
        }

        private void OnUninstallApp(object sender, RoutedEventArgs e)
        {
            string uninstallCommand = "adb uninstall com.zephyrsleep.tablet";
            RunScript(uninstallCommand);
        }

        private void OnTurnWifiOff(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append("adb shell input keyevent 20 & adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnTurnWifiOn(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(
                "adb shell am start -a android.intent.action.MAIN -n com.android.settings/.wifi.WifiSettings\n");
            stringBuilder.Append("adb shell input keyevent 20 & adb shell input keyevent 23");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOn(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell settings put global airplane_mode_on 1\n");
            stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            RunScript(stringBuilder.ToString());
        }

        private void OnAirplaneModeOff(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("adb shell settings put global airplane_mode_on 0\n");
            stringBuilder.Append("adb shell am broadcast -a android.intent.action.AIRPLANE_MODE");
            RunScript(stringBuilder.ToString());
        }

        private void OnSetHomeApp(object sender, RoutedEventArgs e)
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

        private void OnEnableUsbDebugging(object sender, RoutedEventArgs e)
        {
            /*
            var enableUsbDebugging = "adb shell settings put global adb_enabled 1";
            RunScript(enableUsbDebugging);
            */
        }

        private void OnDisableUsbDebugging(object sender, RoutedEventArgs e)
        {
            var disableUsbDebugging = "adb shell settings put global adb_enabled 0";
            RunScript(disableUsbDebugging);
        }
    }
}
