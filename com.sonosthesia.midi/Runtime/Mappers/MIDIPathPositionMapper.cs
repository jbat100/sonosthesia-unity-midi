using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.Utils;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIPathPositionMapper : MIDIDrivenMapper<Vector3>
    {
        [SerializeField] private Path _path;

        [SerializeField] private bool _normlized;
        
        public override Vector3 Map(MIDINote note)
        {
            float value = SelectValueFromNote(note);
            return _path.Position(value, _normlized);
        }
    }    
}


