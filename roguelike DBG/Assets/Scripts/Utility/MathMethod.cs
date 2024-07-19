using System;
using Character;

namespace Utility
{
    public static class MathMethod
    {
        public static void Cal(ref float num, float changeNum, CalEnum calMode, bool reverse)
        {
            switch (calMode)
            {
                case CalEnum.Plus:
                    num += reverse? -changeNum: changeNum;
                    break;
                case CalEnum.Minus:
                    num -= reverse? -changeNum: changeNum;
                    break;
                case CalEnum.Multiply:
                    num *= reverse? 1/changeNum : changeNum;
                    break;
                case CalEnum.Divide:
                    num /= reverse? 1/changeNum : changeNum;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void Cal(ref Pair<string, float> num, float changeNum, CalEnum calMode, bool reverse)
        {
            num = calMode switch
            {
                CalEnum.Plus => new Pair<string, float>(num.Key, num.Value + (reverse? -changeNum: changeNum)),
                CalEnum.Minus => new Pair<string, float>(num.Key, num.Value - (reverse? -changeNum: changeNum)),
                CalEnum.Multiply => new Pair<string, float>(num.Key, num.Value * (reverse? 1/changeNum : changeNum)),
                CalEnum.Divide => new Pair<string, float>(num.Key, num.Value / (reverse? 1/changeNum : changeNum)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}