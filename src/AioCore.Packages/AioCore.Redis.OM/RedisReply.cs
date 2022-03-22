using StackExchange.Redis;

namespace AioCore.Redis.OM
{
    public class RedisReply : IConvertible
    {
        private readonly RedisReply[]? _values;
        private readonly double? _internalDouble;
        private readonly int? _internalInt;
        private readonly string? _internalString;
        private readonly long? _internalLong;

        internal RedisReply(double val)
        {
            _internalDouble = val;
        }

        internal RedisReply(string val)
        {
            _internalString = val;
        }

        internal RedisReply(long val)
        {
            _internalLong = val;
        }

        internal RedisReply(int i)
        {
            _internalInt = i;
        }

        internal RedisReply(RedisReply[] values)
        {
            _values = values;
        }

        internal RedisReply(RedisResult result)
        {
            switch (result.Type)
            {
                case ResultType.None:
                    break;
                case ResultType.SimpleString:
                case ResultType.BulkString:
                    _internalString = (string) result;
                    break;
                case ResultType.Error:
                    break;
                case ResultType.Integer:
                    _internalLong = (long) result;
                    break;
                case ResultType.MultiBulk:
                    _values = ((RedisResult[]) result).Select(x => new RedisReply(x)).ToArray();
                    break;
            }
        }

        public static implicit operator double(RedisReply v)
        {
            if (v._internalDouble != null)
            {
                return (double) v._internalDouble;
            }

            if (v._internalString != null && double.TryParse(v._internalString, out var ret))
            {
                return ret;
            }

            if (v._internalInt != null)
            {
                return (double) v._internalInt;
            }

            if (v._internalLong != null)
            {
                return (double) v._internalLong;
            }

            throw new InvalidCastException("Could not cast to double");
        }

        public static implicit operator double?(RedisReply v) => v._internalDouble;

        public static implicit operator RedisReply(double d) => new(d);

        public static implicit operator RedisReply[](RedisReply v) => v._values ?? new[] {v};

        public static implicit operator RedisReply(RedisReply[] vals) => new(vals);

        public static implicit operator string(RedisReply v) => v._internalString ?? string.Empty;

        public static implicit operator RedisReply(string s) => new(s);

        public static implicit operator int(RedisReply v)
        {
            if (v._internalInt != null)
            {
                return (int) v._internalInt;
            }

            if (v._internalString != null && int.TryParse(v._internalString, out var ret))
            {
                return ret;
            }

            if (v._internalDouble != null)
            {
                return (int) v._internalDouble;
            }

            if (v._internalLong != null)
            {
                return (int) v._internalLong;
            }

            throw new InvalidCastException("Could not cast to int");
        }


        public static implicit operator int?(RedisReply v) => v._internalInt;


        public static implicit operator RedisReply(int i) => new(i);


        public static implicit operator long(RedisReply v)
        {
            if (v._internalLong != null)
            {
                return (long) v._internalLong;
            }

            if (v._internalString != null && long.TryParse(v._internalString, out var ret))
            {
                return ret;
            }

            if (v._internalDouble != null)
            {
                return (long) v._internalDouble;
            }

            if (v._internalInt != null)
            {
                return (long) v._internalInt;
            }

            throw new InvalidCastException("Could not cast to long");
        }


        public static implicit operator long?(RedisReply v) => v._internalLong;


        public static implicit operator RedisReply(long l) => new(l);


        public static implicit operator string[](RedisReply v) =>
            v.ToArray().Select(s => (string) s).ToArray();


        public static implicit operator double[](RedisReply v) =>
            v.ToArray().Select(d => (double) d).ToArray();


        public override string ToString()
        {
            if (_internalDouble != null)
            {
                return _internalDouble.ToString()!;
            }

            if (_internalLong != null)
            {
                return _internalLong.ToString()!;
            }

            if (_internalInt != null)
            {
                return _internalInt.ToString()!;
            }

            return _internalString ?? base.ToString()!;
        }


        public RedisReply[] ToArray() => _values ?? new[] {this};


        public TypeCode GetTypeCode()
        {
            if (_internalDouble != null)
            {
                return TypeCode.Double;
            }

            if (_internalInt != null)
            {
                return TypeCode.Int32;
            }

            if (_internalLong != null)
            {
                return TypeCode.Int64;
            }

            if (_internalString != null)
            {
                return TypeCode.String;
            }

            throw new NotImplementedException();
        }


        public bool ToBoolean(IFormatProvider? provider)
        {
            return this == 1;
        }


        public byte ToByte(IFormatProvider? provider)
        {
            var asDouble = (double) this;
            if (asDouble is >= byte.MinValue and <= byte.MaxValue)
            {
                return (byte) asDouble;
            }

            throw new InvalidCastException();
        }


        public char ToChar(IFormatProvider? provider)
        {
            return ToString().Length == 1
                ? ToString().First()
                : throw new InvalidCastException();
        }


        public DateTime ToDateTime(IFormatProvider? provider)
        {
            throw new InvalidCastException();
        }


        public decimal ToDecimal(IFormatProvider? provider)
        {
            if (_internalDouble != null)
            {
                return (decimal) _internalDouble;
            }

            if (_internalInt != null)
            {
                return (decimal) _internalInt;
            }

            if (_internalLong != null)
            {
                return (decimal) _internalLong;
            }

            throw new InvalidCastException();
        }


        public double ToDouble(IFormatProvider? provider)
        {
            return this;
        }


        public short ToInt16(IFormatProvider? provider)
        {
            var asDouble = (double) this;
            if (asDouble is <= short.MaxValue and >= short.MinValue)
            {
                return (short) asDouble;
            }

            throw new InvalidCastException();
        }


        public int ToInt32(IFormatProvider? provider) => this;


        public long ToInt64(IFormatProvider? provider) => this;


        public sbyte ToSByte(IFormatProvider? provider)
        {
            var asDouble = (double) this;
            if (asDouble is <= sbyte.MaxValue and >= sbyte.MinValue)
            {
                return (sbyte) asDouble;
            }

            throw new InvalidCastException();
        }


        public float ToSingle(IFormatProvider? provider)
        {
            return (float) ((double) this);
        }


        public string ToString(IFormatProvider? provider)
        {
            return ToString();
        }


        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            var underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                switch (underlyingType.Name)
                {
                    case "Int32":
                        return (int) this;
                    case "Int64":
                        return (long) this;
                    case "Single":
                        return (float) this;
                    case "Double":
                        return (double) this;
                }
            }

            throw new NotImplementedException();
        }


        public ushort ToUInt16(IFormatProvider? provider)
        {
            var asLong = (long) this;
            if (asLong is >= ushort.MinValue and <= ushort.MaxValue)
            {
                return (ushort) asLong;
            }

            throw new InvalidCastException();
        }


        public uint ToUInt32(IFormatProvider? provider)
        {
            var asLong = (long) this;
            if (asLong is >= uint.MinValue and <= uint.MaxValue)
            {
                return (uint) asLong;
            }

            throw new InvalidCastException();
        }


        public ulong ToUInt64(IFormatProvider? provider)
        {
            var asLong = (long) this;
            if (asLong is >= 0)
            {
                return (ulong) asLong;
            }

            throw new InvalidCastException();
        }
    }
}