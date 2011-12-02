using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading;

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
        //TODO: Needs to be renamed
        public class ThreadInfo
        {
            public string title;
            public IntPtr hWnd;
            public IntPtr spID;
        }
        public delegate bool CallBackPtr(IntPtr hwnd, ref ThreadInfo threadInfo);
        private IntPtr SpotifyHandle;

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId", CharSet = CharSet.Ansi)]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);

        [DllImport("user32", EntryPoint = "EnumWindows", CharSet = CharSet.Ansi)]
        public static extern int EnumWindows(CallBackPtr callPtr, ref ThreadInfo threadInfo);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Ansi)]
        private static extern bool GetWindowText(IntPtr hWnd, [OutAttribute()] StringBuilder strNewWindowName, Int32 maxCharCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", CharSet = CharSet.Ansi)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        //Constructor
        public SpotiCAD()
        {
            //Get Spotify Process ID
            this.SpotifyHandle = getProcess();
            //Grab the right sub-process, the one with the title
            ThreadInfo thread = threadInfo((int)SpotifyHandle);
            //Spit out the Window Title
            string[] words = thread.title.Split('–');
            foreach (string word in words)
            {
                Console.WriteLine(word.Trim());
            }
            //Keep the Console open
            Console.ReadLine();
        }

        public static bool processCheck(IntPtr hwnd, ref ThreadInfo threadInfo)
        {
            //Get Process ID from the windowHandler
            int pid = 0;
            GetWindowThreadProcessId(hwnd.ToInt32(), out pid);
            //Check if the process is a sub-process of the process: spotify
            if (pid == (int)threadInfo.spID)
            {
                //Check if the process' title contains the word Spotify (Caps Sensitive)
                if (getWindowTitle(hwnd).Contains("Spotify"))
                {
                    //Throw the info into the threadInfo object
                    threadInfo.hWnd = hwnd;
                    threadInfo.title = getWindowTitle(hwnd);
                    //Break out of our loop searching all processes
                    return false;
                }
            }
            return true;
        }

        //TODO: Needs to be renamed
        public ThreadInfo threadInfo(int pid)
        {
            ThreadInfo threadInfo = new ThreadInfo();
            //Put the spotify process ID into the threadInfo so Report method can use it.
            threadInfo.spID = SpotifyHandle;
            EnumWindows(new CallBackPtr(processCheck), ref threadInfo);
            //Process have been checked.
            if (threadInfo.hWnd == IntPtr.Zero)
            {
                //Couldn't find a thread with the correct title
                return null;
            }
            //We found it, pass through our dataobject
            return threadInfo;
        }

        //Get Spotify's ProcessID
        private IntPtr getProcess()
        {
            Process[] sProcessArr = Process.GetProcessesByName("spotify");
            //Check if the Spotify Process is running
            if (sProcessArr.Length > 0)
            {
                Process sProcess = sProcessArr[0];
                //Dump the Handle
                return (IntPtr)sProcess.Id;
            }
            //Reaching this means the Spotify Process could not be found
            return IntPtr.Zero;
        }

        //Get the title of the spotify window and store it.
        private static String getWindowTitle(IntPtr hWnd)
        {
            //StringBuilder sb = new StringBuilder(256);
            //GetWindowText(SpotifyHandle, sb, 256);
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        //We grab the title of the spotify window and strip out the track name.
        private void catchTrack()
        {

        }

        //We grab the title of the spotify window and strip out the artist name.
        private void catchArtist()
        {

        }
    }
}
