using UnityEngine;
using System;

namespace Obvious.Soap
{
    [CreateAssetMenu(fileName = "scriptable_event_" + nameof(Enum), menuName = "Soap/ScriptableEvents/"+ nameof(Enum))]
    public class ScriptableEventEnum : ScriptableEvent<Enum>
    {
        
    }
}
