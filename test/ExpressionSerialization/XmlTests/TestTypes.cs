namespace vm2.ExpressionSerialization.XmlTests;
using System;

using System.Runtime.Serialization;

enum EnumTest
{
    One,
    Two,
    Three,
};

[Flags]
enum EnumFlagsTest
{
    One = 1,
    Two = 2,
    Three = 4,
}

[DataContract(Namespace = "urn:vm.Test.Diagnostics", IsReference = true)]
class Object1
{
    [DataMember]
    public object? ObjectProperty { get; set; }

    [DataMember]
    public int? NullIntProperty { get; set; } = null;

    [DataMember]
    public long? NullLongProperty { get; set; } = 1L;

    [DataMember]
    public bool BoolProperty { get; set; } = true;

    [DataMember]
    public char CharProperty { get; set; } = 'A';

    [DataMember]
    public byte ByteProperty { get; set; } = 1;

    [DataMember]
    public sbyte SByteProperty { get; set; } = 1;

    [DataMember]
    public short ShortProperty { get; set; } = 1;

    [DataMember]
    public int IntProperty { get; set; } = 1;

    [DataMember]
    public long LongProperty { get; set; } = 1L;

    [DataMember]
    public ushort UShortProperty { get; set; } = 1;

    [DataMember]
    public uint UIntProperty { get; set; } = 1;

    [DataMember]
    public ulong ULongProperty { get; set; } = 1;

    [DataMember]
    public double DoubleProperty { get; set; } = 1.0;

    [DataMember]
    public float FloatProperty { get; set; } = 1F;

    [DataMember]
    public Half HalfProperty { get; set; } = (Half)1.1;

    [DataMember]
    public decimal DecimalProperty { get; set; } = 1M;

    [DataMember]
    public Guid GuidProperty { get; set; } = Guid.Empty;

    [DataMember]
    public Uri UriProperty { get; set; } = new Uri("http://localhost");

    [DataMember]
    public DateTime DateTimeProperty { get; set; } = new DateTime(2024, 4, 14, 22, 48, 34, DateTimeKind.Local);

    [DataMember]
    public TimeSpan TimeSpanProperty { get; set; } = new TimeSpan(123L);

    [DataMember]
    public DateTimeOffset DateTimeOffsetProperty { get; set; } = new DateTimeOffset(2024, 4, 14, 22, 48, 34, new TimeSpan(0, 5, 0));

    public string _stringField = "Hi there!";
}

[DataContract]
class ClassDataContract1 : IEquatable<ClassDataContract1>
{
    public ClassDataContract1() { }

    public ClassDataContract1(int i, string s)
    {
        IntProperty = i;
        StringProperty = s;
    }

    [DataMember]
    public int IntProperty { get; set; } = 7;

    [DataMember]
    public string StringProperty { get; set; } = "vm";

    public override string ToString() => "this.DumpString()";

    public virtual bool Equals(ClassDataContract1? other)
        => other is not null &&
           (ReferenceEquals(this, other) ||
            GetType() == other.GetType() && IntProperty == other.IntProperty && StringProperty == other.StringProperty);

    public override bool Equals(object? obj) => Equals(obj as ClassDataContract1);

    public override int GetHashCode() => HashCode.Combine(IntProperty, StringProperty);

    public static bool operator ==(ClassDataContract1 left, ClassDataContract1 right) => left is null ? right is null : left.Equals(right);

    public static bool operator !=(ClassDataContract1 left, ClassDataContract1 right) => !(left == right);
}

#pragma warning disable IDE0021 // Use expression body for constructor
[DataContract]
class ClassDataContract2 : ClassDataContract1
{
    public ClassDataContract2()
    {
    }

    public ClassDataContract2(int i, string s, decimal d) :
        base(i, s)
    {
        DecimalProperty = d;
    }

    [DataMember]
    public decimal DecimalProperty { get; set; } = 17M;
}
#pragma warning restore IDE0021 // Use expression body for constructor

[Serializable]
class ClassSerializable1
{
    public int IntProperty { get; set; }

    public string StringProperty { get; set; } = "";

    public override string ToString() => "this.DumpString()";
}

class ClassNonSerializable(int intProperty, string strProperty)
{
    public int IntProperty { get; set; } = intProperty;

    public string StringProperty { get; set; } = strProperty;

    public override string ToString() => "this.DumpString()";
}

[Serializable]
struct StructSerializable1
{
    public StructSerializable1() { }

    public int IntProperty { get; set; }

    public string StringProperty { get; set; } = "";
}

[DataContract]
struct StructDataContract1
{
    public StructDataContract1() { }
    public StructDataContract1(int i, string s)
    {
        IntProperty = i;
        StringProperty = s;
    }

    [DataMember]
    public int IntProperty { get; set; }

    [DataMember]
    public string StringProperty { get; set; } = "";

    public override readonly string ToString() => "this.DumpString()";
}

[DataContract]
class A
{
    [DataMember]
    public int _a;

    public static A operator -(A x) => new() { _a = -x._a };

    public static A operator +(A x) => new() { _a = x._a };
}

[DataContract]
class B
{
    [DataMember]
    public bool _b;

    public static B operator !(B x) => new() { _b = !x._b };
}

[DataContract]
class C : A
{
    [DataMember]
    public double _c;
}

#pragma warning disable IDE0025 // Use expression body for property
[DataContract]
class TestMethods
{
    [DataMember]
    public readonly int _a = 3;

    [DataMember]
    public readonly int _b = 11;

    public int A { get => _a; }

    public int B { get => _b; }

    public static int Method1() => 1;

    public static int Method2(int i, string _) => i;

    public int Method3(int i, double _) => i + _a;

    public void Method4(int i, double d) => Console.WriteLine($"Integer: {i}, double: {d}, Integer instance member: {_a}");
}
#pragma warning restore IDE0025 // Use expression body for property

class Inner
{
    public int IntProperty { get; set; }

    public string StringProperty { get; set; } = "";
}

class TestMembersInitialized
{
    public int TheOuterIntProperty { get; set; }

    public DateTime Time { get; set; } = new(2024, 4, 14, 22, 48, 34, DateTimeKind.Local);

    public Inner InnerProperty { get; set; } = new();

    public int[] ArrayProperty { get; set; } = [1, 2, 3];

    public IEnumerable<string> EnumerableProperty { get; set; } = ["1", "2", "3"];
}

class TestMembersInitialized1
{
    public int TheOuterIntProperty { get; set; }

    public DateTime Time { get; set; }

    public Inner InnerProperty { get; set; } = new();

    public int[] ArrayProperty { get; set; } = [];

    public List<Inner> ListProperty { get; set; } = [];

    public int this[int i]
    {
        get => ArrayProperty![i];
        set => ArrayProperty![i] = value;
    }
}
