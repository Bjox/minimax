using System;

namespace Minimax.Core
{
    public static class Minimax
    {
        public enum Target
        {
            Minimize, Maximize
        }

        public static MinimaxResult MinimaxAlphaBeta(IPosition position, int depth, Target target)
        {
            return MinimaxAlphaBeta(position, depth, double.NegativeInfinity, double.PositiveInfinity, target == Target.Maximize);
        }

        private static MinimaxResult MinimaxAlphaBeta(
            IPosition position,
            int depth,
            double alpha,
            double beta,
            bool maximize)
        {
            var numberOfPositions = 0;

            position.Init();
            if (depth == 0 || position.GameOver)
            {
                return new MinimaxResult(position.Evaluate()) { NumberOfPositions = 1 };
            }

            IPosition selectedPosition = null;
            if (maximize)
            {
                var maxEval = double.NegativeInfinity;
                foreach (var child in position.GenerateChildPositions())
                {
                    var childResult = MinimaxAlphaBeta(child, depth - 1, alpha, beta, false);
                    numberOfPositions += childResult.NumberOfPositions;
                    var eval = childResult.Value;
                    if (eval > maxEval)
                    {
                        maxEval = eval;
                        selectedPosition = child;
                    }
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return new MinimaxResult(maxEval, selectedPosition) { NumberOfPositions = numberOfPositions };
            }
            else // minimize
            {
                var minEval = double.PositiveInfinity;
                foreach (var child in position.GenerateChildPositions())
                {
                    var childResult = MinimaxAlphaBeta(child, depth - 1, alpha, beta, true);
                    numberOfPositions += childResult.NumberOfPositions;
                    var eval = childResult.Value;
                    if (eval < minEval)
                    {
                        minEval = eval;
                        selectedPosition = child;
                    }
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return new MinimaxResult(minEval, selectedPosition) { NumberOfPositions = numberOfPositions };
            }
        }
    }
}
