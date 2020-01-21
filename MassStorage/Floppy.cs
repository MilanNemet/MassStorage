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

    }
}
