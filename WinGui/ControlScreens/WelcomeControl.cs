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

using AiNetStudio.WinGui.Forms;
//using ClosedXML.Excel;
//using Microsoft.Win32;
//using SkinDialogs;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.IO;

namespace AiNetStudio.WinGui.Controls
{
    public partial class WelcomeControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        private WebView2? _webView2;

        public WelcomeControl(WinGUIMain mainForm)
        {
            MainFormReference = mainForm;

            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.Dpi;

            this.Load += WelcomeControl_Load;

        }

        private void btnGo_Click(object? sender, EventArgs e)
        {
            string url = "https://google.com";

            // Navigate WebView2 to the entered URL (prefix https:// if scheme missing)
            if (_webView2?.CoreWebView2 != null)
            {
                var navigateUrl = (url ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(navigateUrl))
                {
                    if (!navigateUrl.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) &&
                        !navigateUrl.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase))
                    {
                        navigateUrl = "https://" + navigateUrl;
                    }
                    _webView2.CoreWebView2.Navigate(navigateUrl);
                }
            }
        }

        private async void WelcomeControl_Load(object? sender, EventArgs e)
        {
            //ADD BROWSER to pnlBrowser
            pnlBrowser.Dock = DockStyle.Fill;

            if (_webView2 == null)
            {
                _webView2 = new WebView2
                {
                    Dock = DockStyle.Fill
                };
                pnlBrowser.Controls.Add(_webView2);

                // Initialize CoreWebView2 and optionally wire pre-navigation logic
                await _webView2.EnsureCoreWebView2Async();
                _webView2.CoreWebView2.NavigationStarting += (s, args) =>
                {
                    // Example OnBeforeNavigate-style interception:
                    // if (args.Uri.Contains("blockedsite.com", System.StringComparison.OrdinalIgnoreCase)) args.Cancel = true;
                };

                // Load local HTML page (html/welcome.html) from the app's root folder
                var exeDir = AppContext.BaseDirectory;
                var htmlPath = Path.Combine(exeDir, "wwwroot", "welcome.html");

                if (File.Exists(htmlPath))
                {
                    var fileUri = new Uri(htmlPath);
                    _webView2.CoreWebView2.Navigate(fileUri.AbsoluteUri);
                }
            }
        }

    }
}
