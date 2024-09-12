namespace SliceAndDicePrototype.DiceSystem
{
    public struct SideWithData<T>
    {
        public Side Side;
        public T Data;

        public SideWithData(Side side, T data)
        {
            Side = side;
            Data = data;
        }
    }
}
