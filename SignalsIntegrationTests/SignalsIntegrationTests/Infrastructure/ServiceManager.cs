using System;
using System.Diagnostics;
using System.IO;

namespace SignalsIntegrationTests.Infrastructure
{
    public class ServiceManager
    {
        private const string ExecutableHostPathSettingName = "SignalsHostExecutablePath";

        private Process serviceProcess = null;

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
                Verb = "runas",
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
