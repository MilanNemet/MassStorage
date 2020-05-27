using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace MassStorage
{
    public class Storage : INotifyPropertyChanged
    {
        protected static int serial = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        public Storage(int size)
        {
            kapacitas = size;
            Serial = ++serial;
        }

        public int Serial { get; private set; }
        private int kapacitas;

        public int MaximálisKapacitás
        {
            get { return kapacitas; }
            protected set { kapacitas = value; }
        }

        public int FoglaltKapacitás
        {
            get 
            {
                if (FileLista == null || FileLista.Count == 0)
                {
                    return 0;
                }
                else
                {
                    return FileLista.Sum(i => i.Meret);
                }
            }
        }

        public int SzabadKapacitás
        {
            get { return MaximálisKapacitás - FoglaltKapacitás; }
        }


        public ObservableCollection<File> FileLista { get; private set; } = new ObservableCollection<File>();

        virtual public void Format()
        {
            FileLista.Clear();
            OnPropertyChanged("FoglaltKapacitás");
            OnPropertyChanged("SzabadKapacitás");
        }

        virtual public void Hozzáad(string fileName, int fileSize)
        {
            Match találat = Tartalmaz(fileName);

            bool vanIlyenFile = találat.IsMatch;
            bool vanNekiHely = fileSize <= SzabadKapacitás;

            if (!vanIlyenFile && vanNekiHely)
            {
                FileLista.Add(new File(fileName, fileSize));
                OnPropertyChanged("FoglaltKapacitás");
                OnPropertyChanged("SzabadKapacitás");
            }
            else if (vanIlyenFile)
            {
                MessageBox.Show("Van már ilyen nevű fájl!", "Sikertelen művelet!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!vanNekiHely)
            {
                MessageBox.Show("Nem maradt elég szabad hely az új fájlnak!", "Sikertelen művelet!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Ismeretlen hiba!", "Sikertelen művelet!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public File Keres(string fileName)
        {
            File result = null;
            Match találat = Tartalmaz(fileName);
            if (találat.IsMatch)
            {
                result = FileLista[találat.MatchIndex];
            }
            return result;
        }

        virtual public bool Töröl(string fileName)
        {
            Match találat = Tartalmaz(fileName);
            if (találat.IsMatch)
            {
                FileLista.RemoveAt(találat.MatchIndex);
                OnPropertyChanged("FoglaltKapacitás");
                OnPropertyChanged("SzabadKapacitás");
                return true;
            }
            else
            {
                return false;
            }
        }

        protected Match Tartalmaz(string fileName)
        {
            var fileIndex = FileLista.ToList().FindIndex(i => i.Nev == fileName);
            Match match = new Match(fileIndex);
            return match;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
