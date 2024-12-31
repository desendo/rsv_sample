using System;

namespace Game.State.Data
{
    [Serializable]
    public class IntegerFillData
    {
        public int Max;
        public int Current;
        public float Runtime;
        public float CycleTime;
        public bool Started;
    }
}