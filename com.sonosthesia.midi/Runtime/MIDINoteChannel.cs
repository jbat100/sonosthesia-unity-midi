using System;
using System.Collections.Generic;
using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.Flow;
using UniRx;
using UnityEngine;
using Sonosthesia.AdaptiveMIDI.Messages;

namespace Sonosthesia.MIDI
{
    public class MIDINoteChannel : DynamicChannel<MIDINote>
    {
        [SerializeField] private MIDIInput _input;

        [SerializeField] private int _channel;

        [SerializeField] private int _lowerPitch;

        [SerializeField] private int _upperPitch = 127;
        
        private readonly CompositeDisposable _subscriptions = new ();

        private readonly Dictionary<int, Guid> _notes = new ();

        protected virtual bool ShouldFilterNote(MIDINote note)
        {
            if (_channel >= 0 && note.Channel != _channel)
            {
                return true;
            }
            if (note.Note < _lowerPitch || note.Note > _upperPitch)
            {
                return true;
            }
            return false;
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _subscriptions.Clear();
            if (_input == null)
            {
                return;
            }
            _subscriptions.Add(_input.NoteObservable.Subscribe(note =>
            {
                if (ShouldFilterNote(note))
                {
                    return;
                }
                if (_notes.TryGetValue(note.Note, out Guid id))
                {
                    if (note.Velocity == 0)
                    {
                        EndEvent(id, note);
                    }
                    else
                    {
                        UpdateEvent(id, note);
                    }
                }
                else
                {
                    id = BeginEnvent(note);
                    _notes[note.Note] = id;   
                }
            }));
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            _subscriptions.Clear();
        }
    }
}