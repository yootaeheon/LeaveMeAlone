using System;

namespace Assets.Scripts.Item
{
    public static class ElementTypeEnum
    {
        [Flags] // 비트 플래그를 쉽게 사용하게 해주는 속성
        public enum ElementType
        {
            None = 0,
            Fire = 1 << 0, // 00000001
            Water = 1 << 1, // 00000010
            Wind = 1 << 2, // 00000100
            Earth = 1 << 3, // 00001000
            Ice = 1 << 4, // 00010000
            Lightning = 1 << 5, // 00100000
            Light = 1 << 6, // 01000000
            Dark = 1 << 7  // 10000000
        }
    }
}