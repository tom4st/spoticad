using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

/*
    SpotiCAD - Interfaces Spotify to CD Art Display
    Copyright (C) 2011  KodessR <kodessr@gmail.com>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * */
namespace SpotiCAD
{
    class Program
    {
        static void Main(string[] args)
        {
            SpotiCAD spotiCAD = new SpotiCAD();
        }
    }

    class SpotiCAD
    {

        private IntPtr SpotifyHandle;
        private String wTitle;

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Ansi)]
        private static extern bool GetWindowText(IntPtr hWnd, [OutAttribute()] StringBuilder strNewWindowName, Int32 maxCharCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", CharSet = CharSet.Ansi)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        //Constructor
        public SpotiCAD()
        {
            this.SpotifyHandle = IntPtr.Zero;
            //Start our inf loop
            while (true)
            {
            }
        }

        //Get the Handle from the Process
        private IntPtr getHandle()
        {
            Process[] sProcessArr = Process.GetProcessesByName("spotify");
            //Check if the Spotify Process is running
            if (sProcessArr.Length > 0)
            {
                Process sProcess = sProcessArr[0];
                //Dump the Handle
                return sProcess.MainWindowHandle;
            }
            //Reaching this means Spotify isn't running.
            return IntPtr.Zero;
        }

        //Get the title of the spotify window and store it.
        private String getWindowTitle()
        {
            int length = GetWindowTextLength(SpotifyHandle);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(SpotifyHandle, sb, sb.Capacity);
            return sb.ToString();
        }

        //We grab the title of the spotify window and strip out the track name.
        private void getTrack()
        {

        }

        //We grab the title of the spotify window and strip out the artist name.
        private void getArtist()
        {

        }
    }
}
