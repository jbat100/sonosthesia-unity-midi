using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIFloatLinearMapper : MIDILinearMapper<float>
    {
        protected override float Lerp(float start, float end, float value)
        {
            return Mathf.Lerp(start, end, value);
        }
    }
}

