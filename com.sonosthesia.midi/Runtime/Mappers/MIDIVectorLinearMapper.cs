using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIVectorLinearMapper : MIDILinearMapper<Vector3>
    {
        protected override Vector3 Lerp(Vector3 start, Vector3 end, float value)
        {
            return Vector3.Lerp(start, end, value);
        }
    }
}