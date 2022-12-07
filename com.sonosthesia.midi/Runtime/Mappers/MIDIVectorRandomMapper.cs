using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.Flow;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIVectorRandomMapper : Mapper<MIDINote, Vector3>
    {
        [SerializeField] float _range = 1f;
        
        public override Vector3 Map(MIDINote midiNote)
        {
            return _range * Random.insideUnitSphere;
        }
    }
}


