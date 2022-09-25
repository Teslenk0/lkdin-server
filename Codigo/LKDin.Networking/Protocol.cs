namespace LKDin.Networking
{
    public static class Protocol
    {
        public const int SIZE_LENGTH_HEADER = 10;

        public const int SIZE_CMD_HEADER = 4;

        // HEADER => "CMD=1111|LENGTH=1234567890"
        public const int HEADER_SIZE = 26;

        public const string LENGTH_HEADER_NAME = "LENGTH";

        public const string CMD_HEADER_NAME = "CMD";

        public const string MSG_NAME = "MESSAGE";
    }
}
