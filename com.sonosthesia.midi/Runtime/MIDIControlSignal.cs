using System;
using Sonosthesia.Flow;
using UnityEngine;
using UniRx;

namespace Sonosthesia.MIDI
{
    public class MIDIControlSignal : Signal<float>
    {
        [SerializeField] private MIDIControlReceiver _receiver;

        [SerializeField] private int _channel;

        [SerializeField] private int _number;

        private IDisposable _subscription;
        
        protected void Awake()
        {
            if (!_receiver)
            {
                _receiver = GetComponentInParent<MIDIControlReceiver>();
            }
        }

        protected void OnEnable()
        {
            _subscription?.Dispose();
            _subscription = _receiver.ControlObservable
                .Where(control => control.Channel == _channel && control.Number == _number)
                .Subscribe(control =>
                    {
                        Broadcast(control.Value);
                    });
        }

        protected void OnDisable()
        {
            _subscription?.Dispose();
            _subscription = null;
        }
    }
}