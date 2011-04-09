using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

/*
 * This is NON-FUNCTIONING code.
 * The only way to exit is to kill the process.
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
        public static extern bool GetWindowText(IntPtr hWnd, [OutAttribute()] StringBuilder strNewWindowName, Int32 maxCharCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", CharSet = CharSet.Ansi)]
        static extern int GetWindowTextLength(IntPtr hWnd);

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
        private void getHandle()
        {
            Process[] sProcessArr = Process.GetProcessesByName("spotify");
            //Check if the Spotify Process is running
            if (sProcessArr.Length > 0)
            {
                Process sProcess = sProcessArr[0];
                //Dump the Handle
                this.SpotifyHandle = sProcess.MainWindowHandle;
            }
        }

        //Get the title of the spotify window and store it.
        private void getWindowTitle()
        {
            int length = GetWindowTextLength(SpotifyHandle);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(SpotifyHandle, sb, sb.Capacity);
            wTitle = sb.ToString();
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
