using System.Collections.Generic;

namespace Minimax.Core
{
    public interface IPosition
    {
        bool GameOver { get; }
        double Evaluate();
        void Init();
        IEnumerable<IPosition> GenerateChildPositions();
    }
}
