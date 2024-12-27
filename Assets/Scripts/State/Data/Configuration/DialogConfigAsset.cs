using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    [CreateAssetMenu(fileName = "Dialog config", menuName = "Dialog config")]
    public class DialogConfigAsset : ScriptableObject
    {
        [SerializeField] public List<DialogConfig> DialogConfigs;
        [SerializeField] public List<DialogNodeAction> Actions;

    }
}
