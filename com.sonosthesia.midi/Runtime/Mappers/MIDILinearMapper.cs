using Sonosthesia.MIDI;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public abstract class MIDILinearMapper<T> : MIDIDrivenMapper<T>
    {
        [SerializeField] private T _start;

        [SerializeField] private T _end;
        
        public override T Map(MIDINote midiNote)
        {
            float value = SelectValueFromNote(midiNote);
            return Lerp(_start, _end, value);
        }

        protected abstract T Lerp(T start, T end, float value);
    }    
}


