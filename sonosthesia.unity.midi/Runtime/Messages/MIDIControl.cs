namespace Sonosthesia.MIDI
{
    public readonly struct MIDIControl
    {
        public readonly int Channel;
        public readonly int Number;
        public readonly int Value;

        public MIDIControl(int channel, int number, int value)
        {
            Channel = channel;
            Number = number;
            Value = value;
        }

        public override string ToString()
        {
            return $"{nameof(MIDIControl)} <{nameof(Channel)} {Channel} {nameof(Number)} {Number} {nameof(Value)} {Value}>";
        }
    }
}