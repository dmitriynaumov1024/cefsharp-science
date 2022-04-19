using System;
using System.Drawing;
using System.Windows.Forms;
using CefSharp;
using CefSharpAdvanced;

namespace CefWf
{
    public class MainForm : Form
    {
        public ChromiumWebBrowser Browser;

        public MainForm()
        {
            this.Text = "CEF from scratch";
            this.ClientSize = new Size(1000, 720);

            this.Browser = new ChromiumWebBrowser("about:blank");
            this.Controls.Add(this.Browser);

            var menuStrip = new MenuStrip() { Dock = DockStyle.Top };
            var toolsMenuItem = new ToolStripMenuItem("Tools");
            toolsMenuItem.DropDownItems.Add("Show devtools", null, (sender, args)=> {
                this.Browser.ShowDevTools();
            });
            menuStrip.Items.Add(toolsMenuItem);
            this.Controls.Add(menuStrip);

            this.ResizeBegin += (sender, args) => SuspendLayout();
            this.ResizeEnd += (sender, args) => ResumeLayout(true);
        }
    }
}
