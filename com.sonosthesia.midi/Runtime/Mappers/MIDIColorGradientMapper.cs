using Sonosthesia.AdaptiveMIDI.Messages;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIColorGradientMapper : MIDIDrivenMapper<Color>
    {
        [SerializeField] private Gradient _gradient;

        public override Color Map(MIDINote midiNote)
        {
            float value = SelectValueFromNote(midiNote);
            return _gradient.Evaluate(value);
        }
    }    
}

