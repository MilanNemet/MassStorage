using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            if (int.TryParse(TBox_NewStorageSize.Text, out int result))
            {
                Tárolók.Add(new Storage(result));
                TBox_NewStorageSize.Text = "";
            }
            else
            {
                MessageBox.Show("Hibás méret kezdőérték!", "Hibaüzenet!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            var store = Tárolók[LBox_Storages.SelectedIndex];

            if (store is Floppy)
            {
                store.Format();
            }
            else if (store is DVD_R)
            {
                store.Format();
            }
            else
            {
                store.Format(); 
            }
        }


        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            var fn = TBox_FileName;
            var fs = TBox_FileSize;
            bool parseResult = int.TryParse(fs.Text, out int result);

            if (parseResult &&
                result > 0 &&
                !string.IsNullOrEmpty(fn.Text) &&
                !string.IsNullOrWhiteSpace(fn.Text))
            {
                Tárolók[LBox_Storages.SelectedIndex].Hozzáad(fn.Text, result);
                fn.Text = "";
                fs.Text = "";
            }
            else if (parseResult && result < 1)
            {
                MessageBox.Show
                (
                    "A fájl mérete nem lehet kisebb, mint 1kB!",
                    "Hiba!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            else
            {
                MessageBox.Show
                (    
                    $"Hibás beviteli érték a következő helyen: {(parseResult ? "Fájlnév" : "Méret")}", 
                    "Hiba!", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                );
            }
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            if (Tárolók[LBox_Storages.SelectedIndex].Töröl(TBox_FileName.Text))
            {
                MessageBox.Show($"Fájl törölve: {TBox_FileName.Text}", "Sikeres művelet!", MessageBoxButton.OK, MessageBoxImage.Information);
                TBox_FileName.Text = "";
                TBox_FileSize.Text = "";
            }
            else
            {
                MessageBox.Show($"Nincs ilyen fájl: {TBox_FileName.Text}", "Sikertelen művelet!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private bool isAwait = false;
        private void SearchFile(object sender, RoutedEventArgs e)
        {
            if (isAwait)
            {
                tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
                isAwait = false;
            }
            var file = Tárolók[LBox_Storages.SelectedIndex].Keres(TBox_SearchName.Text);
            TB_ActionResult.Text = (file == null ? "Nincs ilyen fájl!" : "Találat!");
            TB_ActionResult.Foreground = (file == null ? Brushes.Red : Brushes.Green);

            if (file != null)
            {
                var helper = Tárolók[LBox_Storages.SelectedIndex].FileLista.IndexOf(file);
                LBox_Files.SelectedIndex = helper;
                LBox_Files.ScrollIntoView(file);
            }
        }

        private async void ResetBox(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isAwait = true;
            try
            {
                await Task.Delay(2000, tokenSource.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }
            if (!string.IsNullOrEmpty(TB_ActionResult.Text))
            {
                TB_ActionResult.Text = ""; 
            }
            isAwait = false;
        }

        private void SuitSelection(object sender, SelectionChangedEventArgs e)
        {
            var lbi = (sender as ListBox).SelectedItem as Storage;
            if (lbi is Floppy)
            {
                ChBox_Floppy.Visibility = Visibility.Visible;
                ChBox_DVD_R.Visibility = Visibility.Collapsed;
                var floppy = lbi as Floppy;
                if (floppy.Irásvédett)
                {
                    LockButtons(this, new RoutedEventArgs());
                }
                else
                {
                    UnlockButtons(this, new RoutedEventArgs());
                }
            }
            else if (lbi is DVD_R)
            {
                ChBox_Floppy.Visibility = Visibility.Collapsed;
                ChBox_DVD_R.Visibility = Visibility.Visible;
                var dvd = lbi as DVD_R;
                if (dvd.Zárolt)
                {
                    LockButtons(this, new RoutedEventArgs());
                    ChBox_DVD_R.IsEnabled = false;
                }
                else
                {
                    UnlockButtons(this, new RoutedEventArgs());
                    ChBox_DVD_R.IsEnabled = true;
                }
            }
            else
            {
                ChBox_Floppy.Visibility = Visibility.Collapsed;
                ChBox_DVD_R.Visibility = Visibility.Collapsed;
                UnlockButtons(this, new RoutedEventArgs());
            }
        }

        private void LockButtons(object sender, RoutedEventArgs e)
        {
            BTN_Format.IsEnabled = false;
            BTN_NewFile.IsEnabled = false;
            BTN_DeleteFile.IsEnabled = false;
            if (sender is CheckBox && (sender as CheckBox).Name == "ChBox_DVD_R")
            {
                var dvdObj = LBox_Storages.SelectedItem as DVD_R;
                if (!dvdObj.Zárolt)
                {
                    dvdObj.Zárolás();
                    ChBox_DVD_R.IsEnabled = false;
                }
            }
        }

        private void UnlockButtons(object sender, RoutedEventArgs e)
        {
            BTN_Format.IsEnabled = true;
            BTN_NewFile.IsEnabled = true;
            BTN_DeleteFile.IsEnabled = true;
        }
    }
}
