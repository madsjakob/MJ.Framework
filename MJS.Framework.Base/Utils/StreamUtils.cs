using MJS.Framework.Base.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MJS.Framework.Base.Utils
{
    public class StreamUtils
    {
        private const int _bufferSize = 4096;
        private const byte _object_escape_mask = 0x01;
        private const byte _bool_mask = 0x02;
        private const byte _bool_value_mask = 0x01;
        private const byte _integer_mask = 0x20;
        private const byte _string_mask = 0x08;
        private const byte _string_size_mask = 0x07;
        private const byte _float_mask = 0x10;
        private const byte _number_size_mask = 0x0f;
        private const byte _datetime_mask = 0x31;
        private const byte _blob_mask = 0x33;
        private const byte _guid_mask = 0x34;
        private const byte _emb_string_mask = 0x40;
        private const byte _emb_string_length_mask = 0x3f;
        private const byte _emb_number_mask = 0x80;
        private const byte _emb_number_value_mask = 0x7f;

        public static int ReadIntValue(Stream stream)
        {
            return Convert.ToInt32(ReadValue(stream));
        }

        public static object ReadValue(Stream stream)
        {
            if (stream.Position >= stream.Length)
            {
                throw new IOException("ReadStreamValueError eof. Size=" + stream.Length + ", Position=" + stream.Position);
            }
            BinaryReader br = new BinaryReader(stream);
            object result = null;
            int controlByte = br.ReadByte();
            if ((controlByte & _emb_number_mask) != 0)
            {
                result = controlByte & _emb_number_value_mask;
            }
            else if ((controlByte & _emb_string_mask) != 0)
            {
                result = ReadStringValue(stream, controlByte);
            }
            else if ((controlByte == _datetime_mask))
            {
                long dateTimeBinary = br.ReadInt64();
                result = DateTime.FromBinary(dateTimeBinary);
            }
            else if (controlByte == _blob_mask)
            {
                int blobLength = br.ReadInt32();
                result = br.ReadBytes(blobLength);
                if (((byte[])result).Length != blobLength)
                {
                    throw new Exception("Need more data!");
                }
            }
            else if (controlByte == _guid_mask)
            {
                byte[] tempGuid = br.ReadBytes(16);
                result = new Guid(tempGuid);
            }
            else if ((controlByte & _integer_mask) != 0)
            {
                if ((_number_size_mask & controlByte) == 1)
                {
                    result = br.ReadByte();
                }
                else if ((_number_size_mask & controlByte) == 2)
                {
                    result = br.ReadInt16();
                }
                else if ((_number_size_mask & controlByte) == 4)
                {
                    result = br.ReadInt32();
                }
                else if ((_number_size_mask & controlByte) == 8)
                {
                    result = br.ReadInt64();
                }
            }
            else if ((controlByte & _float_mask) != 0)
            {
                if ((controlByte & _number_size_mask) == 4)
                {
                    result = br.ReadSingle();
                }
                else if ((controlByte & _number_size_mask) == 8)
                {
                    result = br.ReadDouble();
                }
            }
            else if ((controlByte & _string_mask) != 0)
            {
                result = ReadStringValue(stream, controlByte);
            }
            else if ((controlByte & _bool_mask) != 0)
            {
                result = (bool)((controlByte & _bool_value_mask) != 0);
            }
            else if (controlByte == _object_escape_mask)
            {
                result = typeof(StreamNode);
            }
            else if (controlByte == 0)
            {
                result = null;
            }
            else
            {
                throw new Exception("Illegal streaminput at position " + stream.Position);
            }
            return result;
        }

        private static string ReadStringValue(Stream stream, int controlByte)
        {
            BinaryReader br = new BinaryReader(stream);
            int length;
            if ((controlByte & _emb_string_mask) != 0)
            {
                length = (controlByte & _emb_string_length_mask);
            }
            else
            {
                int sizeLength = (controlByte & _string_size_mask);
                if (sizeLength == 1)
                {
                    length = br.ReadByte();
                }
                else if (sizeLength == 2)
                {
                    length = (ushort)br.ReadInt16();
                }
                else if (sizeLength == 4)
                {
                    length = br.ReadInt32();
                }
                else
                {
                    throw new Exception("Invalid stringsizelengthbyte");
                }
            }
            return Encoding.Default.GetString(br.ReadBytes(length));
        }

        public static void WriteValue(Stream stream, object value)
        {
            if (value != null && value != DBNull.Value)
            {
                if (value.GetType().IsEnum)
                {
                    WriteValue(stream, (int)value);
                }
                else if (value is short)
                {
                    WriteInt(stream, (short)value);
                }
                else if(value is byte)
                {
                    WriteInt(stream, (byte)value);
                }
                else if ((value is int))
                {
                    WriteInt(stream, (int)value);
                }
                else if ((value is long))
                {
                    WriteInt(stream, (long)value);
                }
                else if (value is string)
                {
                    WriteString(stream, (string)value);
                }
                else if (value is bool)
                {
                    WriteBool(stream, (bool)value);
                }
                else if (value is DateTime)
                {
                    WriteDateTime(stream, (DateTime)value);
                }
                else if (value is float)
                {
                    WriteFloat(stream, (float)value);
                }
                else if (value is double)
                {
                    WriteDouble(stream, (double)value);
                }
                else if (value.GetType() == typeof(byte[]))
                {
                    WriteBlob(stream, (byte[])value);
                }
                else if (value is StreamNode)
                {
                    WriteStreamMaskByte(stream, 1);
                }
                else if (value is Guid)
                {
                    WriteGuid(stream, (Guid)value);
                }
                else
                {
                    throw new Exception("Can\'t save " + value.GetType().Name + " to stream!");
                }
            }
            else
            {
                WriteStreamMaskByte(stream, 0);
            }
        }

        private static void WriteGuid(Stream stream, Guid guid)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(_guid_mask);
            bw.Write(guid.ToByteArray(), 0, 16);
        }

        private static void WriteBlob(Stream stream, byte[] value)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(_blob_mask);
            bw.Write(value.Length);
            bw.Write(value, 0, value.Length);
        }

        private static void WriteFloat(Stream stream, float value)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write((byte)(_float_mask + sizeof(float)));
            bw.Write(value);
        }

        private static void WriteDouble(Stream stream, double value)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write((byte)(_float_mask + sizeof(double)));
            bw.Write(value);
        }

        private static void WriteDateTime(Stream stream, DateTime value)
        {
            WriteStreamMaskByte(stream, _datetime_mask);
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(value.ToBinary());
        }

        private static void WriteBool(Stream stream, bool value)
        {
            if (value)
            {
                WriteStreamMaskByte(stream, (byte)(_bool_mask | 1));
            }
            else
            {
                WriteStreamMaskByte(stream, _bool_mask);
            }
        }

        private static void WriteString(Stream stream, string value)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            int strlen = value.Length;
            if (strlen <= _emb_string_length_mask)
            {
                WriteStreamMaskByte(stream, (byte)(_emb_string_mask | strlen));
            }
            else if (strlen <= byte.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_string_mask | 1));
                bw.Write((byte)strlen);
            }
            else if (strlen <= short.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_string_mask | 2));
                bw.Write((short)strlen);
            }
            else if (strlen <= int.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_string_mask | 4));
                bw.Write(strlen);
            }
            else
            {
                throw new Exception("String too long");
            }
            bw.Write(Encoding.Default.GetBytes(value));
        }

        
        
        private static void WriteInt(Stream stream, long value)
        {
            BinaryWriter bw = new BinaryWriter(stream);
            if ((value >= 0) && (value <= _emb_number_value_mask))
            {
                WriteStreamMaskByte(stream, (byte)(_emb_number_mask | value));
            }
            else if (value >= Byte.MinValue && value <= Byte.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_integer_mask | 1));
                bw.Write((byte)(value));
            }
            else if (value >= short.MinValue && value <= short.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_integer_mask | 2));
                bw.Write((short)value);
            }
            else if (value >= int.MinValue && value <= int.MaxValue)
            {
                WriteStreamMaskByte(stream, (byte)(_integer_mask | 4));
                bw.Write((int)value);
            }
            else
            {
                WriteStreamMaskByte(stream, (byte)(_integer_mask | 8));
                bw.Write(value);
            }
        }

        private static void WriteStreamMaskByte(Stream stream, byte value)
        {
            stream.WriteByte(value);
        }

        public static string StreamToHex(Stream stream)
        {
            byte b = 0;
            StringBuilder sb = new StringBuilder();
            stream.Position = 0;
            while (stream.Position < stream.Length)
            {
                b = (byte)stream.ReadByte();
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] ReadStreamContent(Stream stream)
        {
            int dataLength;
            byte[] buffer = new byte[_bufferSize];
            List<byte> tempdata = new List<byte>();
            while ((dataLength = stream.Read(buffer, 0, _bufferSize)) > 0)
            {
                for (int index = 0; index < dataLength; index++)
                {
                    tempdata.Add(buffer[index]);
                }
            }
            return tempdata.ToArray();
        }

        public static byte[] ReadStreamContentLength(Stream stream, int length)
        {
            byte[] result = new byte[length];
            int remaining = length;
            int offset = 0;
            while (remaining > 0)
            {
                int read = stream.Read(result, offset, remaining);
                offset += read;
                remaining -= read;
                if (read == 0 && remaining > 0)
                {
                    throw new Exception("Not enough data!");
                }
            }
            return result;
        }


        public static void ResetStream(Stream stream)
        {
            stream.SetLength(0);
            stream.Position = 0;
        }

        public static void ReadStreamFromStream(Stream stream, Stream value)
        {

            int length = ReadIntValue(stream);
            byte[] data = new byte[length];
            stream.Read(data, 0, length);
            value.Write(data, 0, length);
            value.Position = 0;
        }

        public static void WriteStreamToStream(Stream stream, Stream value)
        {
            value.Position = 0;
            WriteValue(stream, (int)value.Length);
            if (value.Length > 0)
            {
                stream.Write(ReadStreamContent(value), 0, (int)value.Length);
            }
        }

        public static void WriteStringSimple(Stream stream, string value)
        {
            stream.Write(Encoding.Default.GetBytes(value), 0, value.Length);
        }

        internal static void DecompressStream(MemoryStream compressedFile, MemoryStream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal static void CompressStream(MemoryStream stream, MemoryStream compressedFile)
        {
            throw new Exception("The method or operation is not implemented.");
        }



        
    }
}
