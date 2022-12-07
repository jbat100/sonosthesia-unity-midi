using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.Flow;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public enum MIDIMapDriver
    {
        None,
        Channel,
        Note,
        Velocity,
        Random,
        Time
    }
    
    public abstract class MIDIDrivenMapper<T> : Mapper<MIDINote, T>
    {
        [SerializeField] private MIDIMapDriver _driver;

        [SerializeField] private float _offset;
        
        [SerializeField] private float _scale = 1f;

        [SerializeField] private float _warp = 1f;
        
        protected float SelectValueFromNote(MIDINote midiNote)
        {
            if (_driver == MIDIMapDriver.Time)
            {
                float time = (Time.time * _scale) + _offset;
                return time - Mathf.Floor(time);
            }
            
            float raw = _driver switch
            {
                MIDIMapDriver.Channel => midiNote.Channel / 15f,
                MIDIMapDriver.Note => midiNote.Note / 127f,
                MIDIMapDriver.Velocity => midiNote.Velocity,
                MIDIMapDriver.Random => Random.value,
                MIDIMapDriver.Time => (Time.time * _warp) - (Mathf.Floor(Time.time * _warp)),
                _ => 0f
            };

            float lerped = Mathf.Lerp(0f, 1f, raw);

            return (lerped * _scale) + _offset;
        }
    }    
}


