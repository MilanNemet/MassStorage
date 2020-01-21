using System.Collections.ObjectModel;
using System.Windows;

namespace MassStorage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Storage> Tárolók { get; private set; } = new ObservableCollection<Storage>();


        #region MainWindow
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Tárolók;
        }
        #endregion


        private void CreateNewStorage(object sender, RoutedEventArgs e)
        {
            Tárolók.Add(new Storage(int.Parse(TBox_NewStorageSize.Text)));
            TBox_NewStorageSize.Text = "";
        }
        private void CreateNewFloppy(object sender, RoutedEventArgs e)
        {
            Tárolók.Add(new Floppy());
        }
        private void CreateNewDVD_R(object sender, RoutedEventArgs e)
        {
            Tárolók.Add(new DVD_R());
        }
        private void FormatStorage(object sender, RoutedEventArgs e)
        {
            Tárolók[LBox_Storages.SelectedIndex].Format();
        }


        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            Tárolók[LBox_Storages.SelectedIndex].Hozzáad(TBox_FileName.Text, int.Parse(TBox_FileSize.Text));
            TBox_FileName.Text = "";
            TBox_FileSize.Text = "";
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            Tárolók[LBox_Storages.SelectedIndex].Töröl(TBox_FileName.Text);
            TBox_FileName.Text = "";
            TBox_FileSize.Text = "";
        }

        private void SearchFile(object sender, RoutedEventArgs e)
        {
            var file = Tárolók[LBox_Storages.SelectedIndex].Keres(TBox_SearchName.Text);
            TB_SearchResult.Text = (file == null ? "Nincs ilyen fájl!" : "Találat!");

            if (file != null)
            {
                var helper = Tárolók[LBox_Storages.SelectedIndex].FileLista.IndexOf(file);
                LBox_Files.SelectedIndex = helper;
                LBox_Files.ScrollIntoView(file);
            }
        }
    }
}
