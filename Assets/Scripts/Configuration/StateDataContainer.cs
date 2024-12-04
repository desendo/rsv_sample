using Game.State.Data;
using UnityEngine;

namespace Game.Configuration
{
    [CreateAssetMenu(fileName = "StateDataContainer", menuName = "Configuration/Default StateData Container")]
    public class StateDataContainer : DataContainerAbstract<StateData>
    {
    }
}