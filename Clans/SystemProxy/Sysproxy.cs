using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Clans {
    class Sysproxy {
        private readonly static string[] _lanIP = {
            "<local>",
            "localhost",
            "127.*",
            "10.*",
            "172.16.*",
            "172.17.*",
            "172.18.*",
            "172.19.*",
            "172.20.*",
            "172.21.*",
            "172.22.*",
            "172.23.*",
            "172.24.*",
            "172.25.*",
            "172.26.*",
            "172.27.*",
            "172.28.*",
            "172.29.*",
            "172.30.*",
            "172.31.*",
            "192.168.*"
        };
        enum RET_ERRORS : int {
            RET_NO_ERROR = 0,
            INVALID_FORMAT = 1,
            NO_PERMISSION = 2,
            SYSCALL_FAILED = 3,
            NO_MEMORY = 4,
            INVAILD_OPTION_COUNT = 5,
        };

        private string _execPath;
        private int _port;
        private static string _queryStr;
        public bool Enabled {
            get {
                execSysproxy("query");
                return _queryStr.StartsWith("3");
            }
            set {
                List<string> bypassList = new List<string>(_lanIP);
                int flag = value ? 3 : 1;
                string bypassString = string.Join(";", bypassList.ToArray());
                string serverStr = $"127.0.0.1:{_port}";
                execSysproxy($"set {flag} {serverStr} {bypassString} -");
            }
        }

        private void execSysproxy(string arguments) {
            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false)) {
                using (var process = new Process()) {
                    process.StartInfo.FileName = _execPath;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WorkingDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources");
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardOutput = true;

                    process.StartInfo.StandardOutputEncoding = Encoding.Unicode;
                    process.StartInfo.StandardErrorEncoding = Encoding.Unicode;

                    process.StartInfo.CreateNoWindow = true;

                    StringBuilder output = new StringBuilder();
                    StringBuilder error = new StringBuilder();

                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null) {
                            outputWaitHandle.Set();
                        }
                        else {
                            output.AppendLine(e.Data);
                        }
                    };
                    process.ErrorDataReceived += (sender, e) => {
                        if (e.Data == null) {
                            errorWaitHandle.Set();
                        }
                        else {
                            error.AppendLine(e.Data);
                        }
                    };
                    try {
                        process.Start();

                        process.BeginErrorReadLine();
                        process.BeginOutputReadLine();

                        process.WaitForExit();
                    }
                    catch (System.ComponentModel.Win32Exception e) {
                        throw new ProxyException(ProxyExceptionType.FailToRun, process.StartInfo.Arguments, e);
                    }
                    var stderr = error.ToString();
                    var stdout = output.ToString();

                    var exitCode = process.ExitCode;
                    if (exitCode != (int)RET_ERRORS.RET_NO_ERROR) {
                        throw new ProxyException(ProxyExceptionType.SysproxyExitError, stderr);
                    }

                    if (arguments == "query") {
                        if (string.IsNullOrWhiteSpace(stdout) || string.IsNullOrEmpty(stdout)) {
                            throw new ProxyException(ProxyExceptionType.QueryReturnEmpty);
                        }
                        _queryStr = stdout;
                    }
                }
            }
        }

        public Sysproxy(int port) {
            _execPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources\\sysproxy.exe");
            _port = port;
        }

    }
}
