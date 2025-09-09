using vm2.UlidType;

var ulidFactory = new UlidFactory();

var vmUlid = ulidFactory.NewUlid();

DateTimeOffset timestamp = vmUlid.Timestamp();
byte[] random = vmUlid.Random();

Console.WriteLine("Ulid");
Console.WriteLine($"As bytes:    {string.Join(":", vmUlid.ToByteArray().Select(b => b.ToString("X2")))}");
Console.WriteLine($"As a string: {vmUlid}");
Console.WriteLine($"{timestamp:o} : {string.Join(":", random.Select(b => b.ToString("X2")))}");

Console.WriteLine();

Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Task.Delay(2).Wait();
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Task.Delay(2).Wait();
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine(ulidFactory.NewUlid().ToString());
Console.WriteLine();

/*
using SyUlid = System.Ulid;

Console.WriteLine("System.Ulid");
var syUlid = SyUlid.NewUlid();
timestamp = syUlid.Time;
random = syUlid.Random;

Console.WriteLine($"As bytes:    {string.Join(":", syUlid.ToByteArray().Select(b => b.ToString("X2")))}");
Console.WriteLine($"As a string: {syUlid}");
Console.WriteLine($"{timestamp:o} : {string.Join(":", random.Select(b => b.ToString("X2")))}");

Console.WriteLine();


Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());
Console.WriteLine(SyUlid.NewUlid().ToString());

01K4P8T8PW:2NRPGWK4VC9H38PS
01K4P8T8PX:SFM8VGRCFTSZ1STZ
01K4P8T8PX:HWSQB413GQ18FQM8
01K4P8T8PX:43FGEJ4WX30YSCGG
01K4P8T8PX:M8BW2DH4BW3BS2XS
01K4P8T8PX:XB4Q98K7V1PVX1A9
01K4P8T8PX:711NFBQ7GBSR33R4
01K4P8T8PX:JMEQXX10P5V1GYZM
01K4P8T8PX:3DK2XNSG46KVGZR0
01K4P8T8PX:HVPHQ4P1H1Z8QMF1
01K4P8T8PX:TJDPST8MCT6GJ3W1
*/