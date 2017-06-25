using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
            if(fi != null)
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

        private void lb_filelist_Drop(object sender, DragEventArgs e)
        {
            /*
            //check what formats are available
            string[] dformats = e.Data.GetFormats();
            for (int i = 0; i < dformats.Length; i++)
            {
                tb_1.Text += dformats[i] + '\n';
                //lb_filelist.Items.Add(dformats[i] + '\n');
            }
            */

            //check whether filedrop data is present (or can be converted to)
            if(e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenamearray = e.Data.GetData(DataFormats.FileDrop) as string[];
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
            if (lb_filelist.SelectedItem != null && tb_outputloc.Text != "")
            {
                goConvert(lb_filelist.SelectedItem as System.IO.FileInfo, tb_outputloc.Text);
            }

        }

        private void btn_setexedir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_setoutputloc_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
