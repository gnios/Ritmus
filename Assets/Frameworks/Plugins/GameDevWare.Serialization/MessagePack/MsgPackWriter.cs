﻿/* 
	Copyright (c) 2016 Denis Zykov, GameDevWare.com

	This a part of "Json & MessagePack Serialization" Unity Asset - https://www.assetstore.unity3d.com/#!/content/59918

	THIS SOFTWARE IS DISTRIBUTED "AS-IS" WITHOUT ANY WARRANTIES, CONDITIONS AND 
	REPRESENTATIONS WHETHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION THE 
	IMPLIED WARRANTIES AND CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY, 
	FITNESS FOR A PARTICULAR PURPOSE, DURABILITY, NON-INFRINGEMENT, PERFORMANCE 
	AND THOSE ARISING BY STATUTE OR FROM CUSTOM OR USAGE OF TRADE OR COURSE OF DEALING.
	
	This source code is distributed via Unity Asset Store, 
	to use it in your project you should accept Terms of Service and EULA 
	https://unity3d.com/ru/legal/as_terms
*/
using System;
using System.IO;
using GameDevWare.Serialization.Exceptions;

// ReSharper disable once CheckNamespace
namespace GameDevWare.Serialization.MessagePack
{
	public class MsgPackWriter : IJsonWriter
	{
		private readonly ISerializationContext context;
		private readonly Stream outputStream;
		private readonly byte[] buffer;
		private readonly EndianBitConverter bitConverter;
		private long bytesWritten;

		public MsgPackWriter(Stream stream, ISerializationContext context)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			if (context == null) throw new ArgumentNullException("context");
			if (!stream.CanWrite) throw new UnreadableStream("stream");

			this.context = context;
			this.outputStream = stream;
			this.buffer = new byte[16];
			this.bitConverter = EndianBitConverter.Little;
			this.bytesWritten = 0;
		}

		public ISerializationContext Context
		{
			get { return this.context; }
		}

		public long CharactersWritten
		{
			get { return this.bytesWritten; }
		}

		public void Flush()
		{
			this.outputStream.Flush();
		}

		public void Write(string value)
		{
			if (value == null) throw new ArgumentNullException("value");

			var bytes = context.Encoding.GetBytes(value);

			if (value.Length <= byte.MaxValue)
			{
				this.Write(MsgPackType.Str8);
				buffer[0] = (byte) bytes.Length;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (value.Length <= ushort.MaxValue)
			{
				this.Write(MsgPackType.Str16);
				this.bitConverter.CopyBytes((ushort) bytes.Length, buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.bytesWritten += 2;
			}
			else
			{
				this.Write(MsgPackType.Str32);
				this.bitConverter.CopyBytes((uint) bytes.Length, buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.bytesWritten += 4;
			}

			this.outputStream.Write(bytes, 0, bytes.Length);
			this.bytesWritten += bytes.Length;
		}

		public void Write(JsonMember value)
		{
			var name = value.NameString;
			if (value.IsEscapedAndQuoted)
				name = JsonUtils.UnescapeAndUnquote(name);

			this.WriteString(name);
		}

		public void Write(int number)
		{
			if (number > -32 && number < 0)
			{
				var formatByte = (byte) ((byte) Math.Abs(number) | (byte) MsgPackType.NegativeFixIntStart);
				this.buffer[0] = formatByte;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (number >= 0 && number < 128)
			{
				var formatByte = (byte) ((byte) number | (byte) MsgPackType.PositiveFixIntStart);
				this.buffer[0] = formatByte;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (number < sbyte.MaxValue && number > sbyte.MinValue)
			{
				this.Write(MsgPackType.Int8);
				buffer[0] = (byte) (sbyte) number;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (number < short.MaxValue && number > short.MinValue)
			{
				this.Write(MsgPackType.Int16);
				this.bitConverter.CopyBytes((short) number, this.buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.bytesWritten += 2;
			}
			else
			{
				this.Write(MsgPackType.Int32);
				this.bitConverter.CopyBytes((int) number, this.buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.bytesWritten += 4;
			}
		}

		public void Write(uint number)
		{
			if (number < 128)
			{
				var formatByte = (byte) ((byte) number | (byte) MsgPackType.PositiveFixIntStart);
				this.buffer[0] = formatByte;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (number < byte.MaxValue)
			{
				this.Write(MsgPackType.UInt8);
				buffer[0] = (byte) number;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (number < ushort.MaxValue)
			{
				this.Write(MsgPackType.UInt16);
				this.bitConverter.CopyBytes((ushort) number, this.buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.bytesWritten += 2;
			}
			else
			{
				this.Write(MsgPackType.UInt32);
				this.bitConverter.CopyBytes((uint) number, this.buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.bytesWritten += 4;
			}
		}

		public void Write(long number)
		{
			if (number <= int.MaxValue && number >= int.MinValue)
			{
				this.Write((int) number);
				return;
			}

			this.Write(MsgPackType.Int64);
			this.bitConverter.CopyBytes(number, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 8);
			this.bytesWritten += 8;
		}

		public void Write(ulong number)
		{
			if (number <= uint.MaxValue)
			{
				this.Write((uint) number);
				return;
			}

			this.Write(MsgPackType.UInt64);
			this.bitConverter.CopyBytes(number, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 8);
			this.bytesWritten += 8;
		}

		public void Write(float number)
		{
			this.Write(MsgPackType.Float32);
			this.bitConverter.CopyBytes(number, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 4);
			this.bytesWritten += 4;
		}

		public void Write(double number)
		{
			this.Write(MsgPackType.Float64);
			this.bitConverter.CopyBytes(number, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 8);
			this.bytesWritten += 8;
		}

		public void Write(decimal number)
		{
			this.Write(MsgPackType.FixExt16);
			this.buffer[0] = (byte) MsgPackExtType.Decimal;
			this.outputStream.Write(buffer, 0, 1);
			this.bitConverter.CopyBytes(number, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 16);
			this.bytesWritten += 17;
		}

		public void Write(bool value)
		{
			if (value)
				this.Write(MsgPackType.True);
			else
				this.Write(MsgPackType.False);
		}

		public void Write(DateTime datetime)
		{
			if (datetime.Kind == DateTimeKind.Local)
				datetime = datetime.ToUniversalTime();

			this.Write(MsgPackType.FixExt8);
			this.buffer[0] = (byte) MsgPackExtType.DateTime;
			this.outputStream.Write(buffer, 0, 1);
			this.bitConverter.CopyBytes(datetime.Ticks, this.buffer, 0);
			this.outputStream.Write(buffer, 0, 8);
			this.bytesWritten += 9;
		}

		public void WriteObjectBegin(int numberOfMembers)
		{
			if (numberOfMembers < 0) throw new ArgumentOutOfRangeException("numberOfMembers");

			if (numberOfMembers < 16)
			{
				var formatByte = (byte) (numberOfMembers | (byte) MsgPackType.FixMapStart);
				this.buffer[0] = formatByte;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 1;
			}
			else if (numberOfMembers <= ushort.MaxValue)
			{
				this.Write(MsgPackType.Map16);
				this.bitConverter.CopyBytes((ushort) numberOfMembers, buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.bytesWritten += 2;
			}
			else
			{
				this.Write(MsgPackType.Map32);
				this.bitConverter.CopyBytes((int) numberOfMembers, buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.bytesWritten += 4;
			}
		}

		public void WriteObjectEnd()
		{
		}

		public void WriteArrayBegin(int numberOfMembers)
		{
			if (numberOfMembers < 0) throw new ArgumentOutOfRangeException("numberOfMembers");

			if (numberOfMembers < 16)
			{
				var formatByte = (byte) (numberOfMembers | (byte) MsgPackType.FixArrayStart);
				this.buffer[0] = formatByte;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten++;
			}
			else if (numberOfMembers <= ushort.MaxValue)
			{
				this.Write(MsgPackType.Array16);
				this.bitConverter.CopyBytes((ushort) numberOfMembers, buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.bytesWritten += 2;
			}
			else
			{
				this.Write(MsgPackType.Array32);
				this.bitConverter.CopyBytes((int) numberOfMembers, buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.bytesWritten += 4;
			}
		}

		public void WriteArrayEnd()
		{
		}

		public void WriteNull()
		{
			this.Write(MsgPackType.Nil);
		}

		private void Write(MsgPackType token)
		{
			this.buffer[0] = (byte) token;
			this.outputStream.Write(buffer, 0, 1);
			this.bytesWritten++;
		}

		public void WriteJson(string jsonString)
		{
			this.WriteJson(jsonString.ToCharArray(), 0, jsonString.Length);
		}

		public void WriteJson(char[] jsonString, int index, int charCount)
		{
			var bytes = context.Encoding.GetBytes(jsonString, index, charCount);
			if (bytes.Length < ushort.MaxValue)
			{
				this.Write(MsgPackType.Ext16);
				this.bitConverter.CopyBytes((ushort) bytes.Length, buffer, 0);
				this.outputStream.Write(buffer, 0, 2);
				this.buffer[0] = (byte) MsgPackExtType.JsonString;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 3;
			}
			else
			{
				this.Write(MsgPackType.Ext32);
				this.bitConverter.CopyBytes((uint) bytes.Length, buffer, 0);
				this.outputStream.Write(buffer, 0, 4);
				this.buffer[0] = (byte) MsgPackExtType.JsonString;
				this.outputStream.Write(buffer, 0, 1);
				this.bytesWritten += 5;
			}

			this.outputStream.Write(bytes, 0, bytes.Length);
			this.bytesWritten += bytes.Length;
		}

		public void Reset()
		{
			this.bytesWritten = 0;
			Array.Clear(this.buffer, 0, this.buffer.Length);
		}
	}
}
