using System;
using System.Windows.Forms;

namespace DisableStandby
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool Disabled = false;

            NotifyIcon ico = new NotifyIcon();
            ico.Text = "Standby Inhibitor";
            ico.Icon = DisableStandby.Properties.Resources.Battery_Options;
            ico.ContextMenuStrip = new ContextMenuStrip();

            ico.ContextMenuStrip.Items.Add("Enable Standby");
            ico.ContextMenuStrip.Items[0].Click += (o, e) =>
                {
                    Disabled = !fnEnableStandby(ico);
                };

            ico.ContextMenuStrip.Items.Add("Disable Standby");
            ico.ContextMenuStrip.Items[1].Click += (o, e) =>
                {
                    Disabled = fnDisableStandby(ico);
                };

            ico.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            ico.ContextMenuStrip.Items.Add("Exit");
            ico.ContextMenuStrip.Items[3].Click += (o, e) =>
                {
                    Application.Exit();
                };

            ico.MouseClick += (o, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                        return;

                    if (Disabled)
                    {
                        ico.BalloonTipIcon = ToolTipIcon.Info;
                        ico.BalloonTipTitle = "Standby Disabled";
                        ico.BalloonTipText =
                            "Standby is currently disabled on your computer.";

                        ico.ShowBalloonTip(2000);
                    }
                    else
                    {
                        ico.BalloonTipIcon = ToolTipIcon.Info;
                        ico.BalloonTipTitle = "Standby Enabled";
                        ico.BalloonTipText =
                            "Standby is currently enabled on your computer.";

                        ico.ShowBalloonTip(2000);
                    }
                };

            ico.Visible = true;

            Disabled = fnDisableStandby(ico);

            Application.ApplicationExit += (o, e) =>
                {
                    Disabled = !fnEnableStandby(ico);
                };

            Application.Run();
        }

        static bool fnDisableStandby(NotifyIcon ico)
        {
            bool res = SierraLib.Win32API.SystemPowerState.PreventStandby();
            if (!res)
            {
                ico.BalloonTipIcon = ToolTipIcon.Error;
                ico.BalloonTipTitle = "Error Disabling Standby";
                ico.BalloonTipText =
                    "There was a problem disabling Standby on your computer\n" +
                    "Please make sure that you are running this application on Windows XP Service Pack 3 or higher.";

                ico.ShowBalloonTip(2000);
            }
            else
            {
                ico.BalloonTipIcon = ToolTipIcon.Info;
                ico.BalloonTipTitle = "Standby Disabled";
                ico.BalloonTipText =
                    "Standby has been successfully disabled on your computer.";

                ico.ShowBalloonTip(2000);
            }

            return res;
        }

        static bool fnEnableStandby(NotifyIcon ico)
        {
            bool res = SierraLib.Win32API.SystemPowerState.Reset();
            if (!res)
            {
                ico.BalloonTipIcon = ToolTipIcon.Error;
                ico.BalloonTipTitle = "Error Enabling Standby";
                ico.BalloonTipText =
                    "There was a problem enabling Standby on your computer\n" +
                    "Please make sure that you are running this application on Windows XP Service Pack 3 or higher.";

                ico.ShowBalloonTip(2000);
            }
            else
            {
                ico.BalloonTipIcon = ToolTipIcon.Info;
                ico.BalloonTipTitle = "Standby Enabled";
                ico.BalloonTipText =
                    "Standby has been successfully re-enabled on your computer.";

                ico.ShowBalloonTip(2000);
            }

            return res;
        }
    }
}
