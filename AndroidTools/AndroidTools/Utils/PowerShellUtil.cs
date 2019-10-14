using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AndroidTools.Utils
{
    static class PowerShellUtil
    {
        public static void RunScript(string commands)
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

        public static List<string> RunScriptWithOutput(string commands)
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
    }
}
