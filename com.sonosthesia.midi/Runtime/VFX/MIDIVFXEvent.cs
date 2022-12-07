using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.VFX;

namespace Sonosthesia.MIDI
{
    [Obsolete("Use SpawnChannelVFXEvent")]
    public class MIDIVFXEvent : MIDINoteCallback
    {
        [SerializeField] private List<VisualEffect> _visualEffects;

        [SerializeField] private string _eventName;

        [Header("Sources")] 
        
        [SerializeField] private Mapper<MIDINote, Color> _colorSource;

        [SerializeField] private Mapper<MIDINote, Vector3> _positionSource;

        [SerializeField] private Mapper<MIDINote, Vector3> _scaleSource;

        [SerializeField] private Mapper<MIDINote, float> _sizeSource;
        
        [SerializeField] private Mapper<MIDINote, float> _lifetimeSource;

        protected override void MIDINoteOn(MIDINote midiNote)
        {
            base.MIDINoteOn(midiNote);

            Debug.Log($"{nameof(MIDINoteOn)} {midiNote}");

            foreach (VisualEffect visualEffect in _visualEffects)
            {
                VFXEventAttribute eventAttribute = visualEffect.CreateVFXEventAttribute();
                ConfigureAttribute(eventAttribute, midiNote);
                visualEffect.SendEvent(_eventName, eventAttribute);
            }
        }

        protected virtual void ConfigureAttribute(VFXEventAttribute eventAttribute, MIDINote midiNote)
        {
            StringBuilder descriptionBuilder = new StringBuilder($"{this} event ({midiNote}) attributes:");

            if (_colorSource)
            {
                Color color = _colorSource.Map(midiNote);
                eventAttribute.SetVector3("color", new Vector3(color.r, color.g, color.b));
                descriptionBuilder.Append($" color {color}");
            }

            if (_positionSource)
            {
                Vector3 position = _positionSource.Map(midiNote);
                eventAttribute.SetVector3("position", position);
                descriptionBuilder.Append($" position {position}");
            }

            if (_scaleSource)
            {
                Vector3 scale = _scaleSource.Map(midiNote);
                eventAttribute.SetVector3("scale", scale);
                descriptionBuilder.Append($" scale {scale}");
            }

            if (_sizeSource)
            {
                float size = _sizeSource.Map(midiNote);
                eventAttribute.SetFloat("size", size);
                descriptionBuilder.Append($" size {size}");
            }
            
            if (_lifetimeSource)
            {
                float lifetime = _lifetimeSource.Map(midiNote);
                eventAttribute.SetFloat("lifetime", lifetime);
                descriptionBuilder.Append($" lifetime {lifetime}");
            }
            
            Debug.Log(descriptionBuilder.ToString());
        }
    }
}
