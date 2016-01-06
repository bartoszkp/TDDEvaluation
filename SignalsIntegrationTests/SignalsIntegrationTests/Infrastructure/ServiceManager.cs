using System;
using System.Diagnostics;
using System.IO;

namespace SignalsIntegrationTests.Infrastructure
{
    public class ServiceManager
    {
        private const string ExecutableHostPathSettingName = "SignalsHostExecutablePath";

        private Process serviceProcess = null;

        /// <summary>
        /// Either run VS as administrator or execute this command in a console with administrative priviledges:
        /// 
        /// netsh http add urlacl url=http://+:8080/signals user=[DOMAIN]\[USER]
        /// 
        /// where [DOMAIN] and [USER] should be replaced with values appropriate for the credentials
        /// you want to use when working with this solution.
        /// </summary>
        public void StartService()
        {
            if (serviceProcess != null)
            {
                throw new InvalidOperationException();
            }

            FileInfo executableFileInfo = new FileInfo(Properties.Settings.Default[ExecutableHostPathSettingName] as string);

            if (!executableFileInfo.Exists)
            {
                throw new InvalidOperationException(
                    "Executable host: '"
                    + executableFileInfo.FullName
                    + "' does not exist. Please set the Visual Studio Project's setting '"
                    + ExecutableHostPathSettingName
                    + "' to contain full path to the correct executable file");
            }

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                ErrorDialog = false,
                RedirectStandardInput = true,
                FileName = executableFileInfo.FullName,
                WorkingDirectory = executableFileInfo.DirectoryName,
                UseShellExecute = false
            };

            serviceProcess = Process.Start(psi);
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
    }
}
