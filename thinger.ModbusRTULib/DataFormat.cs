namespace thinger.DataConvertLib
{
    public enum DataFormat
    {
        ABCD, // 按照原字节顺序
        BADC, // 两字节交换
        CDAB, // 两个字交换
        DCBA  // 完全反转
    }
}