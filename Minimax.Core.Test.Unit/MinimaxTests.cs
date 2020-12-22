using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Minimax.Core.Test.Unit
{
    [TestClass]
    public class MinimaxTests
    {
        [TestMethod]
        public void Test_MockTree()
        {
            /*
                      3             (max)
                 3          X       (min)
              3    5     4    X     (max)
            -1 3  5 X  -6-4  X X    (min)
            */
            var position = new MockPosition()
            {
                Children = new List<MockPosition>()
                {
                    new MockPosition()
                    {
                        Children = new List<MockPosition>()
                        {
                            new MockPosition()
                            {
                                Children = new List<MockPosition>()
                                {
                                    new MockPosition(-1),
                                    new MockPosition(3)
                                }
                            },
                            new MockPosition()
                            {
                                Children = new List<MockPosition>()
                                {
                                    new MockPosition(5),
                                    new MockPosition(shouldBePruned: true)
                                }
                            }
                        }
                    },
                    new MockPosition()
                    {
                        Children = new List<MockPosition>()
                        {
                            new MockPosition()
                            {
                                Children = new List<MockPosition>()
                                {
                                    new MockPosition(-6),
                                    new MockPosition(-4)
                                }
                            },
                            new MockPosition(shouldBePruned: true)
                            {
                                Children = new List<MockPosition>()
                                {
                                    new MockPosition(shouldBePruned: true),
                                    new MockPosition(shouldBePruned: true)
                                }
                            }
                        }
                    }
                }
            };
            Minimax.MinimaxAlphaBeta(position, 3, double.NegativeInfinity, double.PositiveInfinity, true).Should().Be(3);
        }

        class MockPosition : IPosition
        {
            public bool GameOver => false;
            public List<MockPosition> Children;
            private readonly double? Value;
            private readonly bool ShouldBePruned;

            public MockPosition(double? value = null, bool shouldBePruned = false)
            {
                Value = value;
                ShouldBePruned = shouldBePruned;
            }

            public double Evaluate()
            {
                if (ShouldBePruned)
                {
                    throw new Exception("Position should be pruned, but Evaluate() was called");
                }
                if (Value == null || Children != null)
                {
                    throw new Exception("Should not evaluate non-leaf positions");
                }
                return Value.Value;
            }

            public IEnumerable<IPosition> GenerateChildPositions()
            {
                return Children;
            }
        }
    }
}
