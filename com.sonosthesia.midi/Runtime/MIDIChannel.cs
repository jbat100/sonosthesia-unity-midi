using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIChannel : DynamicChannel<MIDINote>
    {
        [SerializeField] private MIDINoteReceiver _receiver;

        private readonly CompositeDisposable _subscriptions = new ();

        private readonly Dictionary<int, Guid> _notes = new ();

        protected override void OnEnable()
        {
            base.OnEnable();
            _subscriptions.Clear();
            if (_receiver == null)
            {
                return;
            }
            _subscriptions.Add(_receiver.NoteOnObservable.Subscribe(note =>
            {
                if (_notes.ContainsKey(note.Note))
                {
                    Debug.LogError($"{this} unexpected note on while ongoing {note}");
                    return;
                }
                Guid id = BeginEnvent(note);
                _notes[note.Note] = id;
            }));
            _subscriptions.Add(_receiver.NoteOffObservable.Subscribe(note =>
            {
                if (_notes.TryGetValue(note.Note, out Guid id))
                {
                    EndEvent(id, note);
                    _notes.Remove(note.Note);
                    return;
                }
                Debug.LogError($"{this} unexpected note off while not ongoing {note}");
            }));
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            _subscriptions.Clear();
        }
    }
}