namespace BRM.Blackboards.Utilties
{
    internal class ValueContainer<T>// where T : struct
    {
        public ValueContainer(T value)
        {
            Value = value;
        }

        public T Value;
    }
}