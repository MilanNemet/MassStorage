namespace MassStorage
{
    class Floppy : Storage
    {
        public Floppy() : base(1440)
        {

        }

        private bool irásvédett;
        public bool Irásvédett
        {
            get { return irásvédett; }
            set { irásvédett = value; }
        }

        public override void Format()
        {
            if (!irásvédett)
            {
                base.Format(); 
            }
        }
        public override void Hozzáad(string fileName, int fileSize)
        {
            if (!irásvédett)
            {
                base.Hozzáad(fileName, fileSize); 
            }
        }
    }
}
