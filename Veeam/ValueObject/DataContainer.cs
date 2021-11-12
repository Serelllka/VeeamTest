namespace Test.ValueObject
{
    public class DataContainer
    {
        public DataContainer(byte[] data, int dataSize)
        {
            Data = data;
            DataSize = dataSize;
        }
        
        public byte[] Data { get; }
        public int DataSize { get; }
    }
}