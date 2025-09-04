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

namespace AiNetStudio.WinGui.Controls
{
    public partial class BrowserControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WinGUIMain? MainFormReference { get; set; }

        private WebView2? _webView2;

        public BrowserControl(WinGUIMain mainForm)
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.Dpi;

            MainFormReference = mainForm;

            this.Load += WelcomeControl_Load;

            btnGo.Click += btnGo_Click;

        }

        private void btnGo_Click(object? sender, EventArgs e)
        {
            //READ value in txtURL
            string url = txtURL.Text;

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

                // Auto-navigate to the current text if present
                var startUrl = (txtURL.Text ?? string.Empty).Trim();
                if (!string.IsNullOrWhiteSpace(startUrl))
                {
                    if (!startUrl.StartsWith("http://", System.StringComparison.OrdinalIgnoreCase) &&
                        !startUrl.StartsWith("https://", System.StringComparison.OrdinalIgnoreCase))
                    {
                        startUrl = "https://" + startUrl;
                    }
                    _webView2.CoreWebView2.Navigate(startUrl);
                }
            }
        }

    }
}
