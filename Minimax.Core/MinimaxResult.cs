namespace Minimax.Core
{
    public class MinimaxResult
    {
        public double Value { get; set; }
        public IPosition SelectedPosition { get; set; }
        public int NumberOfPositions { get; set; }

        public MinimaxResult(double value)
        {
            Value = value;
        }

        public MinimaxResult(double value, IPosition selectedPosition)
        {
            Value = value;
            SelectedPosition = selectedPosition;
        }
    }
}
