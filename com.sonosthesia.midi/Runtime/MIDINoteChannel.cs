using System;
using System.Collections.Generic;
using Sonosthesia.AdaptiveMIDI;
using Sonosthesia.Flow;
using UniRx;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDINoteChannel : DynamicChannel<MIDINote>
    {
        [SerializeField] private MIDIInput _input;

        private readonly CompositeDisposable _subscriptions = new ();

        private readonly Dictionary<int, Guid> _notes = new ();

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