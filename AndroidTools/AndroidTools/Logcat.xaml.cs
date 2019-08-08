using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Text;
using System.Windows;


namespace AndroidTools
{
    /// <summary>
    /// Interaction logic for Logcat.xaml
    /// </summary>
    public partial class Logcat : Window
    {
        public Logcat(string filter)
        {
            InitializeComponent();
            var logcatCommand = "adb logcat -s " + filter;
            PopulateLogcatTextBox(logcatCommand);
        }

        private void PopulateLogcatTextBox(string command)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = command;
            var process = new Process();
            process.StartInfo = processStartInfo;
            bool processStarted = process.Start();

            var streamWriter = process.StandardInput;
            var outputReader = process.StandardOutput;
            Debug.WriteLine("THIS: " + streamWriter);
            
            logcatBox.Text += outputReader.ToString() + "\n";
            var errorReader = process.StandardError;
            Debug.WriteLine("THAT: " + outputReader + " " + errorReader);
            process.WaitForExit();
        }

        private List<string> RunScriptWithOutput(string commands)
        {
            var powerShellOutputsList = new List<string>();
            try
            {
                var actualCommand = commands.Split('\n');
                using (var powerShell = PowerShell.Create())
                {
                    foreach (var command in actualCommand)
                    {
                        powerShell.AddScript(command);
                    }
                    //var powerShellOutput = powerShell.Invoke();
                    var output = new PSDataCollection<PSObject>();
                    var p = powerShell.BeginInvoke<PSObject, PSObject>(null, output);
                    //powerShell.BeginInvoke();
                    /*
                    if (powerShellOutput.Count != 0)
                    {
                        foreach (var line in powerShellOutput)
                        {
                            powerShellOutputsList.Add(line.ToString());
                        }
                    }
                    */
                }

                return powerShellOutputsList;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return powerShellOutputsList;
        }

    }
}
