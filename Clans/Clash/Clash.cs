using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;


namespace Clans {
    class Clash {
        private string _execPath;
        private string _configPath;
        private Process _process;

        public Clash(string configPath) {
            _execPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\clash.exe"); ;
            _configPath = configPath;

            _process = new Process();
            _process.StartInfo.FileName = _execPath;
            _process.StartInfo.Arguments = $"-f {_configPath}";
            _process.StartInfo.WorkingDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources");
            _process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.RedirectStandardOutput = true;

            _process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            _process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

            _process.StartInfo.CreateNoWindow = true;

            StringBuilder output = new StringBuilder();
            StringBuilder error = new StringBuilder();

            _process.OutputDataReceived += (sender, e) => {
                if (e.Data != null) Console.WriteLine(e.Data.ToString());
            };
            _process.ErrorDataReceived += (sender, e) => {
                if (e.Data != null) Console.WriteLine(e.Data.ToString());
            };
        }

        public void Start() {
            try {
                _process.Start();
                _process.BeginErrorReadLine();
                _process.BeginOutputReadLine();
            }
            catch (System.ComponentModel.Win32Exception e) {
                throw new ProxyException(ProxyExceptionType.FailToRun, _process.StartInfo.Arguments, e);
            }
        }

        public void Stop() {
            _process.CancelErrorRead();
            _process.CancelOutputRead();
            _process.Kill();
        }

        public void ReloadConfig(string configPath) {
            _configPath = configPath;
            _process.StartInfo.Arguments = $"-f {_configPath}";
            Stop();
            Start();
        }
    }
}
