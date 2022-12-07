using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class IMIDIColorLinearMapper : MIDILinearMapper<Color>
    {
        protected override Color Lerp(Color start, Color end, float value)
        {
            return Color.Lerp(start, end, value);
        }
    }
}


