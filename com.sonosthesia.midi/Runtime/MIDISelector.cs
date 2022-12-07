using System;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDISelector : Selector<MIDINote>
    {
        private enum Selection
        {
            None,
            Unit,
            Channel,
            Note,
            Velocity
        }

        [SerializeField] private Selection _selection;

        protected override float InternalSelect(MIDINote value)
        {
            return _selection switch
            {
                Selection.None => 0f,
                Selection.Unit => 1f,
                Selection.Channel => value.Channel / 16f,
                Selection.Note => value.Note / 127f,
                Selection.Velocity => value.Velocity,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }
    }
}