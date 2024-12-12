using Game.State.Data;
using UnityEngine;

namespace Game.State.Config
{
    [CreateAssetMenu(fileName = "StateDataContainer", menuName = "Configuration/Default StateData Container")]
    public class StateDataContainer : DataContainerAbstract<StateData>
    {
    }
}