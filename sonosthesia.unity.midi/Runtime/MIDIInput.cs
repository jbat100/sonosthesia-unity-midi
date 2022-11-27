using System;
using System.Text;
using Sonosthesia.MIDI.Messages;
using UniRx;
using UnityEngine;

namespace Sonosthesia.MIDI
{
    public class MIDIInput : MonoBehaviour
    {
        [SerializeField] private string _portName = "IAC Driver Unity";

        [SerializeField] private float _retryInterval = 1;
        
        #region Observables
        
        private readonly Subject<MIDINote> _noteSubject = new ();
        public IObservable<MIDINote> NoteObservable => _noteSubject.AsObservable();
        
        private readonly Subject<MIDIControl> _controlSubject = new ();
        public IObservable<MIDIControl> ControlObservable => _controlSubject.AsObservable();
        
        private readonly Subject<MIDIPolyphonicAftertouch> _polyphonicAftertouchSubject = new ();
        public IObservable<MIDIPolyphonicAftertouch> PolyphonicAftertouchObservable => _polyphonicAftertouchSubject.AsObservable();
        
        private readonly Subject<MIDIClock> _clockSubject = new ();
        public IObservable<MIDIClock> ClockObservable => _clockSubject.AsObservable();
        
        private readonly Subject<MIDISongPositionPointer> _songPositionPointerSubject = new ();
        public IObservable<MIDISongPositionPointer> SongPositionPointerObservable => _songPositionPointerSubject.AsObservable();

        private readonly Subject<MIDISync> _syncSubject = new ();
        public IObservable<MIDISync> SyncObservable => _syncSubject.AsObservable();

        #endregion
        
        private MIDIProbe _probe;
        private MIDIPort _port;
        private IDisposable _subscription;

        private string Description()
        {
            StringBuilder builder = new StringBuilder("MIDIInput scan : ");
            int count = _probe.PortCount;
            for (int i = 0; i < count; i++)
            {
                builder.Append(_probe.GetPortName(i) + ", ");
            }
            return builder.ToString();
        }
        
        private void Scan()
        {
            _subscription?.Dispose();
            _subscription = Observable.Interval(TimeSpan.FromSeconds(Mathf.Max(1f, _retryInterval))).Subscribe(_ =>
            {
                Debug.Log(Description());
                int count = _probe.PortCount;
                bool found = false;
                for (int i = 0; i < count; i++)
                {
                    string portName = _probe.GetPortName(i);
                    if (portName == _portName)
                    {
                        found = true;
                        _subscription?.Dispose();
                        _port?.Dispose();
                        _port = new MIDIPort(i, portName);
                        _port.NoteObservable.Subscribe(_noteSubject);
                        _port.ControlObservable.Subscribe(_controlSubject);
                        _port.PolyphonicAftertouchObservable.Subscribe(_polyphonicAftertouchSubject);
                        _port.SongPositionPointerObservable.Subscribe(_songPositionPointerSubject);
                        _port.SyncObservable.Subscribe(_syncSubject);
                        _port.ClockObservable.Subscribe(_clockSubject);
                        break;
                    }
                }
                if (!found)
                {
                    Debug.LogWarning($"Could not find MIDI port {_portName} from {count} available {_port}");
                }
                else
                {
                    Debug.Log($"Found MIDI port {_portName}");
                }
            });
        }
        
        protected void Awake()
        {
            _probe = new MIDIProbe();
            Scan();
        }

        protected void Update()
        {
            //Debug.Log($"{nameof(MIDIInput)} port count {_probe.PortCount}");
            _port?.ProcessMessageQueue();
        }

        protected void OnDestroy()
        {
            _probe?.Dispose();
            _port?.Dispose();
            _subscription?.Dispose();
        }
    }
}