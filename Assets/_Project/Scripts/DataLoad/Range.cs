namespace Capstone.DataLoad
{
    [System.Serializable]
    public class Range
    {
        public int Min;
        public int Max;
        public bool inRange(int value)
        {
            return value >= Min && value <= Max;
        }
    }
}