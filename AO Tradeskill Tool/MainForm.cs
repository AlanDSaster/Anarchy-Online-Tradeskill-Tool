using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AO_Bag_Tool {

    public partial class MainForm : Form {

        public Point ToolSquare;
        public Point TargetSquare;
        public int NumberOfItemsInBag;

        public MainForm() {

            InitializeComponent();

        }

        //Functions and Subroutines
        //This is a replacement for Cursor.Position in WinForms
        //this section taken from https://stackoverflow.com/questions/8272681/how-can-i-simulate-a-mouse-click-at-a-certain-position-on-the-screen
        
            
        //Mouse Controls
        [System.Runtime.InteropServices.DllImport( "user32.dll" )]
        static extern bool SetCursorPos( int x, int y );

        [System.Runtime.InteropServices.DllImport( "user32.dll" )]
        public static extern void mouse_event( int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo );

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;

        //This simulates a left mouse click
        public static void LeftMouseClick( int xpos, int ypos ) {
            SetCursorPos( xpos, ypos );
            mouse_event( MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0 );
            mouse_event( MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0 );
        }

        public static void RightMouseClick( int xpos, int ypos ) {
            SetCursorPos( xpos, ypos );
            mouse_event( MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0 );
            mouse_event( MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0 );
        }

        public static void ShiftRightMouseClick( int xpos, int ypos ) {
            SetCursorPos( xpos, ypos );                       
            mouse_event( MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0 );
            mouse_event( MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0 );            
        }

        //Keyboard Controls
        [DllImport( "user32.dll" )]
        public static extern void keybd_event( byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo );

        public const int VK_UP = 0x26; //up key
        public const int VK_DOWN = 0x28;  //down key
        public const int VK_LEFT = 0x25; //left key
        public const int VK_RIGHT = 0x27; //right key
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int VK_SHIFT = 0x10; //shift key

        int KeyHoldShift() {

            //Press the key
            keybd_event( ( byte ) VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY | 0, 0 );
            return 0;

        }

        int KeyReleaseShift() {

            //Release the key
            keybd_event( ( byte ) VK_SHIFT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0 );
            return 0;

        }

        //ELEMENT HANDLERS

        private void ButtonSetBagCoordinates_Click( object sender, EventArgs e ) {
            //set bag location

            FormOverlay overlay = new FormOverlay( this, LabelBagCoordinates, ToolSquare );
            
        }

        private void ButtonSetInventoryCoordinates_Click( object sender, EventArgs e ) {
            //set inventory location

            FormOverlay overlay = new FormOverlay( this, LabelInventoryCoordinates, TargetSquare );

        }

        private void ButtonTransferItems_Click( object sender, EventArgs e ) {

            int NumberOfItemsInBag = (int) NumericUpDownItemsInContainer.Value;

            ToolSquare = StringToPoint( LabelBagCoordinates.Text );
            TargetSquare = StringToPoint( LabelInventoryCoordinates.Text );

            for(int i = 0; i < NumberOfItemsInBag; i++) {

                System.Threading.Thread.Sleep( 250 );

                
                ClickLeftOnLocation( ToolSquare );
                Console.WriteLine( "Clicking on location: " + ToolSquare.ToString() );
                System.Threading.Thread.Sleep( 250 );

                KeyHoldShift();

                ClickRightOnLocation( TargetSquare );
                Console.WriteLine( "Clicking on location: " + TargetSquare.ToString() );
                System.Threading.Thread.Sleep( 250 );

                KeyReleaseShift();

            }

        }

        private Point StringToPoint( string coordinates ) {

            int x;
            int y;

            string s;
            int index_of_comma;


            index_of_comma = coordinates.IndexOf( "," );
            s = coordinates.Substring( 0, index_of_comma );
            x = Convert.ToInt32(s);

            s = coordinates.Substring( index_of_comma + 2 );
            y = Convert.ToInt32( s );

            return new Point( x, y );

        }

        private void ClickLeftOnLocation(Point LocationToClick) {
            
            LeftMouseClick( LocationToClick.X, LocationToClick.Y );
            
        }

        private void ClickRightOnLocation( Point LocationToClick ) {

            RightMouseClick( LocationToClick.X, LocationToClick.Y );

        }

        private void AboutToolStripMenuItem_Click( object sender, EventArgs e ) {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        private void CloseToolStripMenuItem_Click( object sender, EventArgs e ) {
            this.Dispose();
        }

        private void ReadMeToolStripMenuItem_Click( object sender, EventArgs e ) {

            Form1 help_howto = new Form1();
            help_howto.ShowDialog();

        }

        private void MainForm_Load( object sender, EventArgs e ) {
            
        }
    }

}
