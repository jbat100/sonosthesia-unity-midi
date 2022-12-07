using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIFloatAnimationCurveMapper : MIDIDrivenMapper<float>
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        public override float Map(MIDINote midiNote)
        {
            float value = SelectValueFromNote(midiNote);
            return _animationCurve.Evaluate(value);
        }
    }
}


