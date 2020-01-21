using System.Collections.ObjectModel;
using System.Linq;

namespace MassStorage
{
    public class Storage
    {
        protected static int serial = 0;
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

        public void Format()
        {
            FileLista.Clear();
        }

        public void Hozzáad(string fileName, int fileSize)
        {
            Match találat = Tartalmaz(fileName);
            if (!találat.IsMatch && fileSize <= SzabadKapacitás)
            {
                FileLista.Add(new File(fileName, fileSize));
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

        public void Töröl(string fileName)
        {
            Match találat = Tartalmaz(fileName);
            if (találat.IsMatch)
            {
                FileLista.RemoveAt(találat.MatchIndex);
            }
        }

        protected Match Tartalmaz(string fileName)
        {
            var fileIndex = FileLista.ToList().FindIndex(i => i.Nev == fileName);
            Match match = new Match(fileIndex);
            return match;
        }
    }
}
