using UnityEngine;

namespace Obvious.Soap
{
    [CreateAssetMenu(fileName = "scriptable_event_" + nameof(TowerSocket), menuName = "Soap/ScriptableEvents/"+ nameof(TowerSocket))]
    public class ScriptableEventTowerSocket : ScriptableEvent<TowerSocket>
    {
        
    }
}
