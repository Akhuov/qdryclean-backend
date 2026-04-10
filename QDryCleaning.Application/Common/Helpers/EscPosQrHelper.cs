using System.Text;

namespace QDryClean.Application.Common.Helpers
{
    public static class EscPosQrHelper
    {
        public static byte[] GenerateQrCommands(string content, byte moduleSize = 6, byte errorCorrection = 48)
        {
            var data = Encoding.UTF8.GetBytes(content);
            var result = new List<byte>();

            // [1] Select model: 2
            result.AddRange(new byte[]
            {
                0x1D, 0x28, 0x6B, 0x04, 0x00, 0x31, 0x41, 0x32, 0x00
            });

            // [2] Set module size (1..16)
            result.AddRange(new byte[]
            {
                0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, moduleSize
            });

            // [3] Set error correction
            // 48=L, 49=M, 50=Q, 51=H
            result.AddRange(new byte[]
            {
                0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, errorCorrection
            });

            // [4] Store data in symbol storage area
            int length = data.Length + 3;
            byte pL = (byte)(length & 0xFF);
            byte pH = (byte)((length >> 8) & 0xFF);

            result.AddRange(new byte[]
            {
                0x1D, 0x28, 0x6B, pL, pH, 0x31, 0x50, 0x30
            });
            result.AddRange(data);

            // [5] Print QR
            result.AddRange(new byte[]
            {
                0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30
            });

            return result.ToArray();
        }
    }
}