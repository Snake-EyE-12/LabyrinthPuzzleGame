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

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }
        public static Range operator +(Range range, int baseValue)
        {
            return new Range(baseValue + range.Min, baseValue + range.Max);
        }
    }
}