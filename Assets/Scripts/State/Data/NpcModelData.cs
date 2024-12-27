using System;

namespace Game.State.Data
{
    [Serializable]
    public class NpcModelData : IModelData
    {
        public int _uid;
        public string DialogConfigId;
        public int DialogUId;
        public int UId => _uid;
    }
}