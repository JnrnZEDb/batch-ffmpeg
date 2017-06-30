using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace batch_ffmpeg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<String> filelistsource = new ObservableCollection<string>();
        ObservableCollection<System.IO.FileInfo> fileinfolist = new ObservableCollection<System.IO.FileInfo>();

        public MainWindow()
        {
            InitializeComponent();

            //allow dropping
            lb_filelist.AllowDrop = true;
            tb_1.AllowDrop = true;

            //set filelist source
            lb_filelist.ItemsSource = fileinfolist;


        }


        public void goConvert(System.IO.FileInfo fi, string outdir)
        {

            //make sure something is selected
            if (fi != null)
            {
                //ensure output path ends in a backslash
                if (outdir.Substring(outdir.Length -1, 1) != @"\") { outdir += @"\"; }

                //switch out extension on the output file to mp3
                string newname = fi.Name.Remove((fi.Name.Length - fi.Extension.Length), fi.Extension.Length);
                newname += ".mp3";

                Process proc = new Process();
                proc.StartInfo.FileName = tb_exedir.Text;
                proc.StartInfo.Arguments = "-i \"" + fi.FullName + "\" \"" + outdir + newname + "\"";
                tb_1.Text += proc.StartInfo.Arguments;
                proc.Start();
                proc.WaitForExit();
            }

        }

        private void lb_filelist_Drop(object sender, System.Windows.DragEventArgs e)
        {
            //check whether filedrop data is present (or can be converted to)
            if(e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                string[] filenamearray = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
                for (int i = 0; i < filenamearray.Length; i++)
                {
                    fileinfolist.Add(new System.IO.FileInfo(filenamearray[i]));
                }
            }


        }

        private void lb_filelist_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lb_filelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btn_go_Click(object sender, RoutedEventArgs e)
        {
            //iterate through the listbox calling goConvert on each item
            if (lb_filelist.Items != null && tb_outputloc.Text != "")
            {
                for (int i = 0; i < lb_filelist.Items.Count; i++)
                {
                    goConvert(lb_filelist.Items[i] as System.IO.FileInfo, tb_outputloc.Text);
                }
            }

        }

        private void btn_setexedir_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg_setexedir = new Microsoft.Win32.OpenFileDialog();
            dlg_setexedir.FileName = "ffmpeg"; //default filename
            dlg_setexedir.DefaultExt = ".exe"; //default extension
            dlg_setexedir.Filter = "Applications|*.exe"; //label|extension

            //show the dialog and if successful then update the text box with the new location
            bool? result = dlg_setexedir.ShowDialog();
            if (result == true)
            {
                tb_exedir.Text = dlg_setexedir.FileName;
            }
        }

        private void btn_setoutputloc_Click(object sender, RoutedEventArgs e)
        {
            //have to use winforms' filderbrowserdialog for this instead of the openfiledialog
            //used for the ffmpeg location because wpf doesnt have a native folder dialog
            FolderBrowserDialog dlg_setoutputloc = new FolderBrowserDialog();
            dlg_setoutputloc.Description = "Set output folder.";
            dlg_setoutputloc.ShowNewFolderButton = true;

            //show the dialog and if successful then update the text box with the new location
            DialogResult result = dlg_setoutputloc.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tb_outputloc.Text = dlg_setoutputloc.SelectedPath;
            }

        }

        private void removeSelectedFromListbox()
        {
            //we can't just iterate through selecteditems because we're going to change
            //the dictionary as we delete things so we need to build a separate copy 
            //then loop through and remove the items from the datasource 

            //build the copy
            List<System.IO.FileInfo> itemsToRemove = new List<System.IO.FileInfo>();
            foreach (System.IO.FileInfo selectedFileInfo in lb_filelist.SelectedItems)
            {
                itemsToRemove.Add(selectedFileInfo);
            }
            //remove the items
            foreach (System.IO.FileInfo selectedItemToRemove in itemsToRemove)
            {
                fileinfolist.Remove(selectedItemToRemove);
            }
        }

        private void lb_filelist_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Back:
                case Key.Delete:
                    removeSelectedFromListbox();
                    break;
                default:
                    break;
            }
        }

        private void tb_1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }


}
