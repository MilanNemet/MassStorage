namespace MassStorage
{
    class DVD_R : Storage
    {
        public DVD_R() : base(4700000)
        {

        }

        private bool zárolt = false;
        public bool Zárolt
        {
            get { return zárolt; }
            private set { zárolt = true; }
        }

        public void Zárolás()
        {
            MaximálisKapacitás = FoglaltKapacitás;
            Zárolt = true;
        }
    }
}
