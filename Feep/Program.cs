﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Feep
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Viewer viewer;

            if (args.Length > 0)
            {
                int PointX, PointY, Width, Height;
                string screen = "", backColor = "";
                viewer = new Viewer(args[0]);
                System.Text.StringBuilder ReturnedString = new System.Text.StringBuilder(255);

                bool atCenter = false;

                GetPrivateProfileString("Screen", "BackColor", "#000000", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                backColor = ReturnedString.ToString();
                GetPrivateProfileString("Screen", "Options", "None", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                screen = ReturnedString.ToString();
                GetPrivateProfileString("Location", "X", "", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                atCenter = !Int32.TryParse(ReturnedString.ToString(), out PointX);
                GetPrivateProfileString("Location", "Y", "", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                atCenter = !Int32.TryParse(ReturnedString.ToString(), out PointY);
                GetPrivateProfileString("Size", "Width", "", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                atCenter = !Int32.TryParse(ReturnedString.ToString(), out Width);
                GetPrivateProfileString("Size", "Height", "", ReturnedString, 255, System.Windows.Forms.Application.StartupPath + @".\configure.ini");
                atCenter = !Int32.TryParse(ReturnedString.ToString(), out Height);

                if (atCenter || PointX > SystemInformation.VirtualScreen.Right || PointY > SystemInformation.VirtualScreen.Bottom || Width > SystemInformation.VirtualScreen.Width || Height > SystemInformation.VirtualScreen.Height)
                {
                    PointX = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.1);
                    PointY = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.1);
                    Width = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * 0.8);
                    Height = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * 0.8);
                }

                try
                {
                    viewer.BackColor = System.Drawing.ColorTranslator.FromHtml(backColor);
                    viewer.Picture.BackColor = viewer.BackColor;
                }
                catch (Exception)
                {
                    viewer.BackColor = System.Drawing.Color.Black;
                    viewer.Picture.BackColor = viewer.BackColor;
                }


                if (screen != "None")
                {
                    viewer.TopMost = true;
                    viewer.ScreenBeforeX = PointX;
                    viewer.ScreenBeforeY = PointY;
                    viewer.ScreenBeforeWidth = Width;
                    viewer.ScreenBeforeHeight = Height;

                    Screen currentScreen = Screen.FromPoint(new System.Drawing.Point(PointX, PointY));

                    if (screen != "Full")
                    {
                        switch (screen)
                        {
                            case "Left":
                                viewer.screen = Feep.Viewer.ScreenState.Left;
                                viewer.Location = currentScreen.WorkingArea.Location;
                                viewer.Width = currentScreen.WorkingArea.Width / 2;
                                viewer.Height = currentScreen.WorkingArea.Height;
                                viewer.AtBorderline = true;
                                break;
                            case "Right":
                                viewer.screen = Feep.Viewer.ScreenState.Right;
                                viewer.Location = new System.Drawing.Point(currentScreen.WorkingArea.Width / 2, currentScreen.WorkingArea.Location.Y);
                                viewer.Width = currentScreen.WorkingArea.Width / 2;
                                viewer.Height = currentScreen.WorkingArea.Height;
                                viewer.AtBorderline = true;
                                break;
                            case "Top":
                                viewer.screen = Feep.Viewer.ScreenState.Top;
                                viewer.Location = currentScreen.WorkingArea.Location;
                                viewer.Width = currentScreen.WorkingArea.Width;
                                viewer.Height = currentScreen.WorkingArea.Height / 2;
                                viewer.AtBorderline = true;
                                break;
                            case "Bottom":
                                viewer.screen = Feep.Viewer.ScreenState.Bottom;
                                viewer.Location = new System.Drawing.Point(currentScreen.WorkingArea.Location.X, currentScreen.WorkingArea.Height / 2);
                                viewer.Width = currentScreen.WorkingArea.Width;
                                viewer.Height = currentScreen.WorkingArea.Height / 2;
                                viewer.AtBorderline = true;
                                break;
                        }
                    }
                    else
                    {
                        viewer.screen = Feep.Viewer.ScreenState.Full;
                        viewer.Location = currentScreen.Bounds.Location;
                        viewer.Width = currentScreen.Bounds.Width;
                        viewer.Height = currentScreen.Bounds.Height;
                    }
                }
                else
                {
                    viewer.screen = Feep.Viewer.ScreenState.None;
                    viewer.Location = new System.Drawing.Point(PointX, PointY);
                    viewer.Width = Width;
                    viewer.Height = Height;
                }

                Application.Run(viewer);
            }
            else
            {
                Application.Exit();
            }
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

    }
}