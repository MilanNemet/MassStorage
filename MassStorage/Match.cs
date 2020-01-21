namespace MassStorage
{
    public class Match
    {
        public Match(int mI)
        {
            MatchIndex = mI;
            IsMatch = (mI != -1);
        }
        public bool IsMatch { get; private set; }
        public int MatchIndex { get; private set; }
    }
}
