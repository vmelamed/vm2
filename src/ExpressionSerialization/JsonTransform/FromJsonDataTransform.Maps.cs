﻿namespace vm2.ExpressionSerialization.JsonTransform;
using System.Globalization;
using System.Text.RegularExpressions;

static partial class FromJsonDataTransform
{
    static readonly Dictionary<string, Transformation> _constantTransformations_ = new()
    {
        { Vocabulary.Boolean,           (JElement x, ref Type t) => x.GetValue<bool>()          },
        { Vocabulary.Boolean,           (JElement x, ref Type t) => x.GetValue<byte>()          },
        { Vocabulary.Byte,              (JElement x, ref Type t) => x.GetValue<byte>()          },
        { Vocabulary.Char,              (JElement x, ref Type t) => JsonToChar(x)               },
        { Vocabulary.Double,            (JElement x, ref Type t) => JsonToDouble(x)             },
        { Vocabulary.Float,             (JElement x, ref Type t) => x.GetValue<float>()         },
        { Vocabulary.Int,               (JElement x, ref Type t) => x.GetValue<int>()           },
        { Vocabulary.IntPtr,            (JElement x, ref Type t) => JsonToIntPtr(x)             },
        { Vocabulary.Long,              (JElement x, ref Type t) => x.GetValue<long>()          },
        { Vocabulary.SignedByte,        (JElement x, ref Type t) => x.GetValue<sbyte>()         },
        { Vocabulary.Short,             (JElement x, ref Type t) => x.GetValue<short>()         },
        { Vocabulary.UnsignedInt,       (JElement x, ref Type t) => x.GetValue<uint>()          },
        { Vocabulary.UnsignedIntPtr,    (JElement x, ref Type t) => JsonToUIntPtr(x)            },
        { Vocabulary.UnsignedLong,      (JElement x, ref Type t) => x.GetValue<ulong>()         },
        { Vocabulary.UnsignedShort,     (JElement x, ref Type t) => x.GetValue<ushort>()        },

        { Vocabulary.DateTime,          (JElement x, ref Type t) => JsonToDateTime(x)           },
        { Vocabulary.DateTimeOffset,    (JElement x, ref Type t) => JsonToDateTimeOffset(x)     },
        { Vocabulary.Duration,          (JElement x, ref Type t) => JsonToTimeSpan(x)           },
        { Vocabulary.DBNull,            (JElement x, ref Type t) => DBNull.Value                },
        { Vocabulary.Decimal,           (JElement x, ref Type t) => JsonToDecimal(x)            },
        { Vocabulary.Guid,              (JElement x, ref Type t) => JsonToGuid(x)               },
        { Vocabulary.Half,              (JElement x, ref Type t) => JsonToHalf(x)               },
        { Vocabulary.String,            (JElement x, ref Type t) => x.GetValue<string>()        },
        { Vocabulary.Uri,               (JElement x, ref Type t) => JsonToUri(x)                },

        //{ Vocabulary.Anonymous,         TransformAnonymous                                      },
        //{ Vocabulary.ByteSequence,      TransformByteSequence                                   },
        //{ Vocabulary.Sequence,          TransformCollection                                     },
        //{ Vocabulary.Dictionary,        TransformDictionary                                     },
        //{ Vocabulary.Enum,              TransformEnum                                           },
        //{ Vocabulary.Nullable,          TransformNullable                                       },
        //{ Vocabulary.Object,            TransformObject                                         },
        //{ Vocabulary.Tuple,             TransformTuple                                          },
        //{ Vocabulary.TupleItem,         TransformTuple                                          },
    };
    static readonly FrozenDictionary<string, Transformation> _constantTransformations = _constantTransformations_.ToFrozenDictionary();

    static char JsonToChar(JElement x)
    {
        var s = x.GetValue<string>();

        if (string.IsNullOrEmpty(s))
            throw new SerializationException($"Could not convert the value of property `{x.Name}` to `char` - the string is `null` or empty.");

        return s[0];
    }

    static double JsonToDouble(JElement x)
    {
        if (x.Value?.AsValue()?.GetValueKind() is JsonValueKind.String)
        {
            var fpStr = x.Value.AsValue().GetValue<string>();

            return fpStr switch {
                Vocabulary.NaN => double.NaN,
                Vocabulary.NegInfinity => double.NegativeInfinity,
                Vocabulary.PosInfinity => double.PositiveInfinity,
                _ => double.Parse(fpStr)
            };
        }

        return x.GetValue<double>();
    }

    static float JsonToFloat(JElement x)
    {
        if (x.Value?.AsValue()?.GetValueKind() is JsonValueKind.String)
        {
            var fpStr = x.Value.AsValue().GetValue<string>();

            return fpStr switch {
                Vocabulary.NaN => float.NaN,
                Vocabulary.NegInfinity => float.NegativeInfinity,
                Vocabulary.PosInfinity => float.PositiveInfinity,
                _ => float.Parse(fpStr)
            };
        }

        return x.GetValue<float>();
    }

    static Half JsonToHalf(JElement x)
    {
        if (x.Value?.AsValue()?.GetValueKind() is JsonValueKind.String)
        {
            var fpStr = x.Value.AsValue().GetValue<string>();

            return fpStr switch {
                Vocabulary.NaN => Half.NaN,
                Vocabulary.NegInfinity => Half.NegativeInfinity,
                Vocabulary.PosInfinity => Half.PositiveInfinity,
                _ => Half.Parse(fpStr)
            };
        }

        return (Half)x.GetValue<float>();
    }

    static IntPtr JsonToIntPtr(JElement x)
    {
        if (x.Value?.AsValue()?.GetValueKind() is JsonValueKind.String)
        {
            var ptrStr = x.Value.AsValue().GetValue<string>();

            if (string.IsNullOrWhiteSpace(ptrStr))
                throw new SerializationException($"Could not convert the value of property `{x.Name}` to `IntPtr` - the string is `null`, or empty, or consists of whitespaces only.");

            return checked(
                Environment.Is64BitProcess
                    ? (IntPtr)Int64.Parse(ptrStr)
                    : (IntPtr)Int32.Parse(ptrStr));
        }

        return checked(
            Environment.Is64BitProcess
                ? (IntPtr)x.GetValue<Int64>()
                : (IntPtr)x.GetValue<Int32>());
    }

    static UIntPtr JsonToUIntPtr(JElement x)
    {
        if (x.Value?.AsValue()?.GetValueKind() is JsonValueKind.String)
        {
            var ptrStr = x.Value?.AsValue()?.GetValue<string>();

            if (string.IsNullOrWhiteSpace(ptrStr))
                throw new SerializationException($"Could not convert the value of property `{x.Name}` to `IntPtr` - the string is `null`, or empty, or consists of whitespaces only.");

            return checked(
                Environment.Is64BitProcess
                    ? (UIntPtr)UInt64.Parse(ptrStr)
                    : (UIntPtr)UInt32.Parse(ptrStr));
        }

        return checked(
            Environment.Is64BitProcess
                ? (UIntPtr)x.GetValue<UInt64>()
                : (UIntPtr)x.GetValue<UInt32>());
    }

    static string GetJsonStringToParse(JElement x, string typeName)
    {
        if (x.Value?.AsValue()?.GetValueKind() is not JsonValueKind.String)
            throw new SerializationException($"Could not convert the value of property `{x.Name}` to `{typeName}` - the JSON value is not string.");

        var str = x.Value?.AsValue()?.GetValue<string>();

        if (string.IsNullOrWhiteSpace(str))
            throw new SerializationException($"Could not convert the value of property `{x.Name}` to `{typeName}` - the JSON string is a null, empty, or consists of whitespace characters only.");

        return str;
    }

    static DateTime JsonToDateTime(JElement x)
        => DateTime.Parse(GetJsonStringToParse(x, nameof(DateTime)), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

    static DateTimeOffset JsonToDateTimeOffset(JElement x)
        => DateTimeOffset.Parse(GetJsonStringToParse(x, nameof(DateTimeOffset)), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

    const string durationRegex = @"^-?P(((?<days3>[0-9]+D)|(?<months2>[0-9]+M)((?<days2>[0-9]+D))?|(?<years1>[0-9]+Y)((?<months1>[0-9]+M)((?<days1>[0-9]+D))?)?)"+
                                 @"(T(((?<hours2>[0-9]+H)((?<minutes4>[0-9]+M)((?<seconds6>[0-9]+S))?)?|(?<minutes3>[0-9]+M)((?<seconds5>[0-9]+S))?|(?<seconds4>[0-9]+S))))?|"+
                                 @"T(((?<hours1>[0-9]+H)((?<minutes2>[0-9]+M)((?<seconds3>[0-9]+S))?)?|(?<minutes1>[0-9]+M)((?<seconds2>[0-9]+S))?|(?<seconds1>[0-9]+S)))|(?<weeks>[0-9]+W))$";

    [GeneratedRegex(durationRegex)]
    private static partial Regex DurationRegex();


    static TimeSpan JsonToTimeSpan(JElement x)
    {
        var str = GetJsonStringToParse(x, nameof(TimeSpan));
        var match = DurationRegex().Match(str);

        if (!match.Success)
            throw new SerializationException($"Could not convert the value of property `{x.Name}` to `{nameof(TimeSpan)}` - the JSON string does not represent a valid ISO8601 duration of time.");

        var seconds =   match.Groups["seconds1"].Success ? match.Groups["seconds1"].Value :
                        match.Groups["seconds2"].Success ? match.Groups["seconds2"].Value :
                        match.Groups["seconds3"].Success ? match.Groups["seconds3"].Value :
                        match.Groups["seconds4"].Success ? match.Groups["seconds4"].Value :
                        match.Groups["seconds5"].Success ? match.Groups["seconds5"].Value :
                        match.Groups["seconds6"].Success ? match.Groups["seconds6"].Value : "0";

        var minutes =   match.Groups["minutes1"].Success ? match.Groups["minutes1"].Value :
                        match.Groups["minutes2"].Success ? match.Groups["minutes2"].Value :
                        match.Groups["minutes3"].Success ? match.Groups["minutes3"].Value :
                        match.Groups["minutes4"].Success ? match.Groups["minutes4"].Value : "0";

        var hours =     match.Groups["hours1"].Success ? match.Groups["hours1"].Value :
                        match.Groups["hours2"].Success ? match.Groups["hours2"].Value : "0";

        var days =      match.Groups["days1"].Success ? match.Groups["days1"].Value :
                        match.Groups["days2"].Success ? match.Groups["days2"].Value :
                        match.Groups["days3"].Success ? match.Groups["days3"].Value : "0";

        var months =    match.Groups["days1"].Success ? match.Groups["days1"].Value :
                        match.Groups["days2"].Success ? match.Groups["days2"].Value : "0";

        var years =     match.Groups["years1"].Success ? match.Groups["years1"].Value : "0";

        var weeks =     match.Groups["weeks"].Success ? match.Groups["weeks"].Value : "0";

        // having years and months in the duration is clearly gray area for ISO8601. Some suggest to use the banking loan depreciation type of cycle of 360/30.
        // Our serialization counterpart never puts years and months - just days, etc.
        return new TimeSpan(int.Parse(years) * 360 + int.Parse(months) * 30 + int.Parse(weeks) + int.Parse(days), int.Parse(hours), int.Parse(minutes), int.Parse(seconds));
    }

    static decimal JsonToDecimal(JElement x)
        => decimal.Parse(GetJsonStringToParse(x, nameof(Decimal)), CultureInfo.InvariantCulture);

    static Guid JsonToGuid(JElement x)
        => Guid.Parse(GetJsonStringToParse(x, nameof(Guid)));

    static Uri JsonToUri(JElement x)
        => new UriBuilder(GetJsonStringToParse(x, nameof(Uri))).Uri;
}