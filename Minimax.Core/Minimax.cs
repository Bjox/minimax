using System;

namespace Minimax.Core
{
    public static class Minimax
    {
        public static double MinimaxAlphaBeta(
            IPosition position,
            int depth,
            double alpha,
            double beta,
            bool maximize)
        {
            if (depth == 0 || position.GameOver)
            {
                return position.Evaluate();
            }
            if (maximize)
            {
                var maxEval = double.NegativeInfinity;
                foreach (var child in position.GenerateChildPositions())
                {
                    var eval = MinimaxAlphaBeta(child, depth - 1, alpha, beta, false);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
            else // minimize
            {
                var minEval = double.PositiveInfinity;
                foreach (var child in position.GenerateChildPositions())
                {
                    var eval = MinimaxAlphaBeta(child, depth - 1, alpha, beta, true);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
        }
    }
}
