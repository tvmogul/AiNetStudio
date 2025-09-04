//
////////////////////////////////////////////////////////////////
//
/**************************************************************
**        __                                          __
**     __/_/__________________________________________\_\__
**  __|_                                                  |__
** (___O)     Ouslan, Inc.                              (O___)
**(_____O)	  ainetstudio.com              			   (O_____)
**(_____O)	  Author: Bill SerGio, Infomercial King™   (O_____)
** (__O)                                                (O__)
**    |___________________________________________________|
**
****************************************************************/
/*
 * (C) Copyright 2024-2025 Ouslan,Inc, All Rights Reserved Worldwide.
 * software-rus.com   
 * tvmogul1@yahoo.com  
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AiNetStudio.WinGui.Dialogs
{
    public partial class SkinDlg : Form
    {
        string textToType1 = "Booting Predictive Neural AI Core";
        string textToType2 = "Calibrating Synaptic Algorithms";
        string textToType3 = "Engaging Deep Learning Protocols";

        #region VARIABLES

        int _charIndex = 0;

        System.Windows.Forms.Timer? tmr;

        public int idist = 0;
        public static int procIdAppNodeJs = 0;
        public static int procIdNodeAPI = 0;
        public static int procIdNodeGraph = 0;
        public static int procIdOrion = 0;

        public static IntPtr hWndAppNodeJs = IntPtr.Zero;
        public static IntPtr hWndNodeAPI = IntPtr.Zero;
        public static IntPtr hWndNodeGraph = IntPtr.Zero;
        public static IntPtr hWndOrion = IntPtr.Zero;

        //https://stackoverflow.com/questions/9387267/transparent-control-over-picturebox
        //using System.ComponentModel;
        //using System.Windows.Forms;
        //using System.Windows.Forms.Design;    // Add reference to System.Design
        //[Designer(typeof(ParentControlDesigner))]
        //class PictureContainer : PictureBox { }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Win32.Rect rectangle);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, ref Win32.Rect rectangle);

        static bool? bIELauncheded = false;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern System.Int32 SystemParametersInfo(System.UInt32 uiAction, System.UInt32 uiParam,
            System.IntPtr pvParam, System.UInt32 fWinIni);
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern bool SystemParametersInfo(int uAction, int uParam, bool lpvParam, int fuWinIni);
        [DllImport("kernel32.dll")]
        static extern bool? SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        private bool?  _capturing;
        private Image? _dropperBoxed;
        private Image? _dropperUnboxed;
        private Cursor? _cursorDefault;
        private Cursor? _cursorDropper;
        private IntPtr? _hPreviousWindow;

        private string? _Handle = string.Empty;
        private string? _Class = string.Empty;
        private string? _Text = string.Empty;
        private string? _Style = string.Empty;
        private string? _Rect = string.Empty;

        string? vlcInstallDirectory;

        [DllImport("gdi32.dll")]
        public static extern IntPtr ExtCreateRegion(int lpXform, int nCount, byte[] lpRgnData);

        [DllImport("user32.dll")]
        internal static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        //static private SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindowsClass();
        //public SHDocVw.ShellWindows SWs;
        //public SHDocVw.InternetExplorer IE;

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this
        // overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        /// <summary>The GetForegroundWindow function returns 
        /// a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(HandleRef hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern Int32 CloseHandle(IntPtr hObject);

        #endregion


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool? IsBankDataLoaded { get; private set; } = false; // Track loading status

        public SkinDlg()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.Dpi;

            //DO NOT RUN AGAIN
            // Start preloading banks in the background
            //_ = BankService.GetBanksAsync();

            vlcInstallDirectory = Application.StartupPath;

            ex1.Parent = picGif;
            exLogo.Parent = picGif;
            pnlLogin.Parent = picGif;
            exLogin1.Parent = picGif;
            exLogin2.Parent = picGif;

            ex1.BackColor = Color.Transparent;
            exLogo.BackColor = Color.Transparent;   
            pnlLogin.BackColor = Color.Transparent;
            exLogin1.BackColor = Color.Transparent;
            exLogin2.BackColor = Color.Transparent;

            exLogin1.Visible = false;
            exLogin2.Visible = false;

            this.Load += Form1_Load!;
            this.Shown += Form1_Shown!;

            picGif.MouseDown += picGif_MouseDown!;

            btnLogin.Click += btnLogin_Click;
            btnClose.Click += btnClose_Click;

        }


        #region LOGIN

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            bool isValid = ValidateLogin(txtUsername.Text, txtPassword.Text);

            if (isValid)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid login credentials.");
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            return username == "admin" && password == "password";
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Start preloading banks in the background
            // Start the task without blocking the UI thread
            //Task.Run(async () =>
            //{
            //    var service = new FdicApiService();
            //    service.DownloadAndStoreAllBanks();

            //    // Now close the splash screen on the UI thread
            //    this.Invoke(() =>
            //    {
            //        this.Close();
            //    });
            //});

            StartAnimations();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //vlc = new DShowPlayer.DShowPlayer();
            CentreWindow(Handle, GetMonitorDimensions());

        }

        #endregion

        #region LOAD ANIMATIONS

        private void StartAnimations()
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "bg_blackbrains.gif");
            if (File.Exists(imagePath))
            {
                picGif.Image = Image.FromFile(imagePath);
                picGif.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 5000;
            tmr.Start();
            tmr.Tick += tmr_Tick1!;

            StartTypewriteText();
        }

        private void StartTypewriteText()
        {
            _charIndex = 0;
            ex1.Text = string.Empty;
            string textToType = textToType1; // "Checking AI Neural Networks";
            Thread t = new Thread(new ParameterizedThreadStart(TypewriteText!));
            t.Start(textToType);
        }

        private void TypewriteText(object text)
        {
            string? _text = text as string;
            if (_text == null) return;
            while (_charIndex < _text.Length)
            {
                Thread.Sleep(50);
                ex1.Invoke(new Action(() =>
                {
                    ex1.Text += _text[_charIndex];
                }));
                _charIndex++;
            }
        }

        void tmr_Tick1(object sender, EventArgs e)
        {
            tmr!.Stop();

            //bg_blackbrains.gif
            //bg_blackelectric.gif

            ex1.BackColor = Color.Transparent;
            ex1.Visible = true;
            _charIndex = 0;
            ex1.Text = string.Empty;
            string textToType = textToType2; //  "Updating Neural Networks";
            Thread t = new Thread(new ParameterizedThreadStart(TypewriteText!));
            t.Start(textToType);

            //picGif.Image = Properties.Resources.bg_blackelectric;
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "bg_blackelectric.gif");
            if (File.Exists(imagePath))
            {
                picGif.Image = Image.FromFile(imagePath);
                picGif.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 4000;
            tmr.Start();
            tmr.Tick += tmr_Tick2!;
        }

        void tmr_Tick2(object sender, EventArgs e)
        {
            tmr!.Stop();

            ex1.Visible = false;
            //labelEx2.Visible = false;

            ex1.BackColor = Color.Transparent;
            ex1.Visible = true;
            _charIndex = 0;
            ex1.Text = string.Empty;
            string textToType = textToType3; //  "Loading Neural Networks";
            Thread t = new Thread(new ParameterizedThreadStart(TypewriteText!));
            t.Start(textToType);

            //bg_blackbrains.gif
            //bg_blackelectric.gif
            //zbg_ufo.gif

            //string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "zbg_ufo.gif");
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "animesher.com_stars-star-gif-1599370.gif");
            if (File.Exists(imagePath))
            {
                picGif.Image = Image.FromFile(imagePath);
                picGif.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 3000;
            tmr.Start();
            tmr.Tick += tmr_Tick3!;
        }

        void tmr_Tick3(object sender, EventArgs e)
        {
            tmr!.Stop();
            //labelEx1.Visible = false;
            picGif.Image = null!;

            //bg_blackbrains.gif
            //bg_blackelectric.gif
            //zbg_ufo.gif
            //deeplearning.gif

            //picGif.Image = Properties.Resources.bg_blackbrains;
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "deeplearning.gif");
            if (File.Exists(imagePath))
            {
                picGif.Image = Image.FromFile(imagePath);
            }

            picGif.SizeMode = PictureBoxSizeMode.StretchImage;

            //tmr = new System.Windows.Forms.Timer();
            //tmr.Interval = 40000;
            //tmr.Start();
            //tmr.Tick += tmr_Tick!;

            //labelEx1.Visible = false;
            //labelEx2.Visible = false;
            //labelEx3.BackColor = Color.Transparent;

            exLogin1.Text = string.Empty;
            exLogin2.Text = string.Empty;

            pnlLogin.Visible = false;
            exLogin1.Visible = true;
            exLogin2.Visible = true;

            ex1.Visible = false;

            //exLogin1
            _charIndex = 0;
            //string textToType = "Welcome to AI Accounting...\r\nWorld's Leading AI Software";
            string textToType = "Welcome to AiNetStudio®\r\n" +
                                "for Building Neural Networks";

            Thread t = new Thread(new ParameterizedThreadStart(TypewriteText2!));
            t.Start(textToType);
        }

        private void TypewriteText2(object text)
        {
            string? _text = text as string;
            if (_text == null) return;
            while (_charIndex < _text.Length)
            {
                Thread.Sleep(50);
                exLogin1.Invoke(new Action(() =>
                {
                    exLogin1.Text += _text[_charIndex];
                }));
                _charIndex++;
            }

            //exLogin2
            _charIndex = 0;
            //string? textToType3 = "Copyright 1991-2024. Ouslan,Inc.";
            string? textToType3 = $"Copyright 2024-{DateTime.Now.Year}. Ouslan,Inc.";
            Thread t3 = new Thread(new ParameterizedThreadStart(TypewriteText3!));
            t3.Start(textToType3);
        }

        private void TypewriteText3(object text)
        {
            string? _text = text as string;
            if (_text == null) return;
            while (_charIndex < _text.Length)
            {
                Thread.Sleep(50);
                exLogin2.Invoke(new Action(() =>
                {
                    exLogin2.Text += _text[_charIndex];
                }));
                _charIndex++;
            }
            Thread.Sleep(3000);

            // Ensure this.Close() is invoked on the UI thread
            this.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            tmr!.Stop();

            //this.ShowInTaskbar = false;
            //this.Opacity = 0;
            //this.Location = new Point(-10000, -10000);

            //IntPtr hWndTray = GetSystemTrayHandle();
            //if (hWndTray != IntPtr.Zero)
            //{
            //    Win32.SetWindowPos(hWndTray, (System.IntPtr)Win32.HWND_BOTTOM, -1000, -1000, 10, 10,
            //        Win32.SWP_HIDEWINDOW | Win32.SWP_NOSIZE | Win32.SWP_NOMOVE | Win32.SWP_NOACTIVATE);
            //}

            //this.Close();

            //labelEx1.Visible = false;
            //labelEx2.Visible = false;
            //labelEx3.BackColor = Color.Transparent;
        }

        #endregion

        /// <summary>
        /// Gets the current DPI scaling factor.
        /// </summary>
        //private float GetDpiScalingFactor()
        //{
        //    using (Graphics g = this.CreateGraphics())
        //    {
        //        float dpiX = g.DpiX;
        //        return dpiX / 96.0f; // 96 DPI is the baseline for 100%
        //    }
        //}


        #region LAUNCH PROCESS

        private static bool IsProcessActiveByName(string processName)
        {
            return Process.GetProcessesByName(processName).Any();
        }

        private static bool IsProcessActiveByID(int processID)
        {
            bool bFound = false;
            if (processID > 0)
            {
                var zProcesses = Process.GetProcesses().Where(pr => pr.Id == processID);
                foreach (var process in zProcesses)
                {
                    IntPtr hFound = process.MainWindowHandle;
                    bFound = true;
                }
            }
            return bFound;
        }

        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

        private void CentreWindow(IntPtr handle, Size monitorDimensions)
        {
            GetWindowRect(new HandleRef(this, handle), out RECT rect);

            var x1Pos = monitorDimensions.Width / 2 - (rect.Right - rect.Left) / 2;
            var x2Pos = rect.Right - rect.Left;
            var y1Pos = monitorDimensions.Height / 2 - (rect.Bottom - rect.Top) / 2;
            var y2Pos = rect.Bottom - rect.Top;

            Win32.SetWindowPos(handle, 0, x1Pos, y1Pos, x2Pos, y2Pos, 0);
        }

        private Size GetMonitorDimensions()
        {
            return SystemInformation.PrimaryMonitorSize;
        }

        public void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        public static void CreateBorder(IntPtr hWnd)
        {
            const float penWidth = 3;
            Win32.Rect rc = new Win32.Rect();
            Win32.GetWindowRect(hWnd, ref rc);

            IntPtr hDC = Win32.GetWindowDC(hWnd);
            if (hDC != IntPtr.Zero)
            {
                using (Pen pen = new Pen(Color.Red, penWidth))
                {
                    using (Graphics g = Graphics.FromHdc(hDC))
                    {
                        g.DrawRectangle(pen, 0, 0, rc.right - rc.left - (int)penWidth, rc.bottom - rc.top - (int)penWidth);
                    }
                }
            }
            Win32.ReleaseDC(hWnd, hDC);
        }

        public static void BorderRefresh(IntPtr hWnd)
        {
            Win32.InvalidateRect(hWnd, IntPtr.Zero, 1 /* TRUE */);
            Win32.UpdateWindow(hWnd);
            Win32.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, Win32.RDW_FRAME | Win32.RDW_INVALIDATE | Win32.RDW_UPDATENOW | Win32.RDW_ALLCHILDREN);
        }

        private string GetWindowText(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            Win32.GetWindowText(hWnd, sb, 256);
            return sb.ToString();
        }

        private string GetClassName(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            Win32.GetClassName(hWnd, sb, 256);
            return sb.ToString();
        }

        private void picGif_MouseDown(object sender, MouseEventArgs e)
        {
            Win32.ReleaseCapture();
            Win32.SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        #endregion

    } // public partial class LoginDlg : Form

} // namespace AiNetProfit.WinGui.Dialogs


