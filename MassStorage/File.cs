namespace MassStorage
{
	public class File
	{
		public File(string fileName, int fileSize)
		{
			Nev = fileName;
			Meret = fileSize;
		}

		private string nev;
		public string Nev
		{
			get { return nev; }
			set { nev = value; }
		}

		private int meret;
		public int Meret
		{
			get { return meret; }
			set { meret = value; }
		}

		private bool csakOlvashato = false;
		public bool CsakOlvashato
		{
			get { return csakOlvashato; }
			set { csakOlvashato = value; }
		}

		private bool rendszer = false;
		public bool Rendszer
		{
			get { return rendszer; }
			set { rendszer = value; }
		}

		private bool rejtett = false;
		public bool Rejtett
		{
			get { return rejtett; }
			set { rejtett = value; }
		}
	}
}
