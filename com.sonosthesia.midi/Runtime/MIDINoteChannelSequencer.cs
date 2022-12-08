using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sonosthesia.AdaptiveMIDI.Messages;
using Sonosthesia.Flow;
using UniRx;
using UnityEngine;

namespace Sonosthesia.MIDI
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(MIDINoteChannelSequencer))]
    public class MIDINoteChannelSequencerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MIDINoteChannelSequencer sequencer = (MIDINoteChannelSequencer)target;
            if(GUILayout.Button("Play"))
            {
                sequencer.Play();
            }
            if(GUILayout.Button("Stop"))
            {
                sequencer.Stop();
            }
        }
    }
#endif
    
    public class MIDINoteChannelSequencer : ChannelSequencer<MIDINote>
    {
        [Serializable]
        private class SequenceEvent
        {
            public int Note;
            public int Velocity;
            public float Delay;
            public float Duration = 1f;
        }

        [SerializeField] private int _channel; 
        
        [SerializeField] private List<SequenceEvent> _midiNotes;

        [SerializeField] private bool _loop;
        
        [SerializeField] private bool _emmitNoteOff;
        
        private CancellationTokenSource _cancellationTokenSource;

        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            PlaySequence(_cancellationTokenSource.Token).Forget();
        }

        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        private async UniTask PlaySequence(CancellationToken cancellationToken)
        {
            int index = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (index >= _midiNotes.Count)
                {
                    if (_loop)
                    {
                        index = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                await PlaySequenceEvent(_midiNotes[index], cancellationToken);
                index++;
            }
        }

        private async UniTask PlaySequenceEvent(SequenceEvent sequenceEvent, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(sequenceEvent.Delay), cancellationToken: cancellationToken);
            MIDINote noteOn = new MIDINote(_channel, sequenceEvent.Note, sequenceEvent.Velocity);
            IObservable<MIDINote> stream = Observable.Return(noteOn);
            if (_emmitNoteOff)
            {
                MIDINote noteOff = new MIDINote(_channel, sequenceEvent.Note, 0);
                stream = stream.Concat(Observable.Timer(TimeSpan.FromSeconds(sequenceEvent.Duration))
                    .Select(_ => noteOff));
            }
            else
            {
                stream = stream.Concat(Observable.Empty<MIDINote>()
                    .Delay(TimeSpan.FromSeconds(sequenceEvent.Duration)));
            }
            Sequence(stream);
            await stream.ToUniTask(cancellationToken: cancellationToken);
        }
    }
}