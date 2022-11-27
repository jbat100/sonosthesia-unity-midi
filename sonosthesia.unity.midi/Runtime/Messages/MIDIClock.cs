namespace Sonosthesia.MIDI
{
    public readonly struct MIDIClock
    {
        public readonly int Count;

        public MIDIClock(int count)
        {
            Count = count;
        }
        
        public override string ToString()
        {
            return $"{nameof(MIDIClock)} <{nameof(Count)} {Count}>";
        }
    }
}