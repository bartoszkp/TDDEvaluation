using System;
using System.Diagnostics;
using System.IO;

namespace SignalsIntegrationTests.Infrastructure
{
    public class ServiceManager
    {
        private const string ExecutableHostPathSettingName = "SignalsHostExecutablePath";
        private const string ExecutableDatabaseMaintenanceToolPathSettingName = "DatabaseMaintenanceToolExecutablePath";

        private Process serviceProcess = null;

        public static void RebuildDatabase()
        {
            var databaseMaintenanceToolExecutable = GetExecutableFromSetting(ExecutableDatabaseMaintenanceToolPathSettingName);

            var process = Process.Start(databaseMaintenanceToolExecutable.FullName, "rebuild");

            process.WaitForExit();
        }

        /// <summary>
        /// Either run VS as administrator or execute this command in a console with administrative priviledges:
        /// 
        /// netsh http add urlacl url=http://+:8080/signals user=[DOMAIN]\[USER]
        /// 
        /// where [DOMAIN] and [USER] should be replaced with values appropriate for the credentials
        /// you want to use when working with this solution (%USERDOMAIN% and %USERNAME% works fine).
        /// </summary>
        public void StartService()
        {
            if (serviceProcess != null)
            {
                throw new InvalidOperationException();
            }

            var serviceExecutable = GetExecutableFromSetting(ExecutableHostPathSettingName);

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                ErrorDialog = false,
                RedirectStandardInput = true,
                FileName = serviceExecutable.FullName,
                WorkingDirectory = serviceExecutable.DirectoryName,
                UseShellExecute = false
            };

            serviceProcess = Process.Start(psi);
        }

        public void RestartService()
        {
            if (serviceProcess != null)
            {
                if (!serviceProcess.HasExited)
                {
                    serviceProcess.Kill();
                }

                serviceProcess = null;
            }

            StartService();
        }

        public bool IsAlive()
        {
            return !serviceProcess.HasExited;
        }

        public void StopService()
        {
            if (serviceProcess == null)
            {
                throw new InvalidOperationException();
            }

            serviceProcess.StandardInput.WriteLine();

            var finished = serviceProcess.WaitForExit(100);

            if (!finished)
            {
                serviceProcess.Kill();
            }

            serviceProcess = null;
        }

        private static FileInfo GetExecutableFromSetting(string settingName)
        {
            FileInfo executableFileInfo = new FileInfo(Properties.Settings.Default[settingName] as string);

            if (!executableFileInfo.Exists)
            {
                throw new InvalidOperationException(
                    "Executable host: '"
                    + executableFileInfo.FullName
                    + "' does not exist. Please set the Visual Studio Project's setting '"
                    + ExecutableHostPathSettingName
                    + "' to contain full path to the correct executable file");
            }

            return executableFileInfo;
        }
    }
}
