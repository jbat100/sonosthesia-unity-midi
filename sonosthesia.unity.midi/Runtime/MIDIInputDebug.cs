using UniRx;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Sonosthesia.MIDI
{
    [RequireComponent(typeof(RtMIDIInput))]
    public class MIDIInputDebug : MonoBehaviour
    {
        private RtMIDIInput _input;
        private CompositeDisposable _subscriptions = new ();
        
        protected void Awake()
        {
            _input = GetComponent<RtMIDIInput>();
        }

        protected void OnEnable()
        {
            _subscriptions = new CompositeDisposable()
            {
                _input.ClockObservable.Subscribe(m => Debug.Log(m)),
                _input.NoteObservable.Subscribe(m => Debug.Log(m)),
                _input.ControlObservable.Subscribe(m => Debug.Log(m)),
                _input.PolyphonicAftertouchObservable.Subscribe(m => Debug.Log(m)),
                _input.SongPositionPointerObservable.Subscribe(m => Debug.Log(m)),
                _input.SyncObservable.Subscribe(m => Debug.Log(m)),
            };
        }

        protected void OnDisable()
        {
            _subscriptions?.Dispose();
        }
    }
}