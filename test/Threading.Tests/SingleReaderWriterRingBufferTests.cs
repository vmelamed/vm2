using System.Collections;

using vm2.Threading.Buffers;

namespace vm2.Threading.Tests
{
    public class SingleReaderWriterRingBufferTests
    {
        [Fact]
        public void Constructor_InvalidCapacity_ThrowsArgumentOutOfRangeException()
        {
            // Act
            var act = () => new SingleReaderWriterRingBuffer<int>(0);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Capacity_IsSet()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(5);
            buf.Capacity.Should().Be(5);
        }

        [Fact]
        public void IsEmpty_InitiallyTrue_TryReadReturnsFalseAndDefault()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);

            buf.IsEmpty.Should().BeTrue();

            var read = buf.TryRead(out var item);

            read.Should().BeFalse();
            item.Should().Be(default);
        }

        [Fact]
        public void TryWrite_TryRead_FIFOAndFullBehavior()
        {
            // capacity 3 -> usable slots = 2 for this implementation
            var buf = new SingleReaderWriterRingBuffer<int>(3);

            buf.TryWrite(1).Should().BeTrue();
            buf.TryWrite(2).Should().BeTrue();

            // now buffer should be full
            buf.IsFull.Should().BeTrue();
            buf.TryWrite(3).Should().BeFalse();

            // read back in FIFO order
            buf.TryRead(out var a).Should().BeTrue();
            a.Should().Be(1);

            buf.TryRead(out a).Should().BeTrue();
            a.Should().Be(2);

            // now empty
            buf.IsEmpty.Should().BeTrue();
            buf.TryRead(out a).Should().BeFalse();
            a.Should().Be(default);
        }

        [Fact]
        public void WrapAround_WriteReadAcrossBoundary_WorksCorrectly()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);

            buf.TryWrite(10).Should().BeTrue();
            buf.TryWrite(20).Should().BeTrue();

            // consume one to advance read index
            buf.TryRead(out var first).Should().BeTrue();
            first.Should().Be(10);

            // now there is space to write again (wrap)
            buf.TryWrite(30).Should().BeTrue();

            // remaining items should be 20 then 30
            buf.TryRead(out var second).Should().BeTrue();
            second.Should().Be(20);

            buf.TryRead(out var third).Should().BeTrue();
            third.Should().Be(30);

            buf.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void Enumerator_CurrentBeforeMoveNext_Throws()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);
            buf.TryWrite(1).Should().BeTrue();

            using var enumerator = buf.GetEnumerator();

            var act = () => { var _ = enumerator.Current; };

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Enumerator_IteratesAndConsumesBuffer_And_CurrentAfterEnumerationThrows()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(4);

            buf.TryWrite(1).Should().BeTrue();
            buf.TryWrite(2).Should().BeTrue();
            buf.TryWrite(3).Should().BeTrue(); // capacity 4 -> usable 3

            var collected = new List<int>();

            foreach (var v in buf)
                collected.Add(v);

            collected.Should().ContainInOrder([1, 2, 3]);

            // enumeration consumes items -> buffer should be empty
            buf.IsEmpty.Should().BeTrue();

            // enumerator.Current after exhausting should throw
            using var enumerator = buf.GetEnumerator();

            while (enumerator.MoveNext())
            {
            } // exhaust

            var act = () => { var _ = enumerator.Current; };

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Enumerator_Reset_AllowsReadingRemainingItemWithoutRestoringConsumed()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);

            buf.TryWrite(1).Should().BeTrue();
            buf.TryWrite(2).Should().BeTrue();

            using var enumerator = buf.GetEnumerator();

            // Read first item
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(1);

            // Reset enumerator state (does not restore buffer contents)
            enumerator.Reset();

            // After reset MoveNext should read the next available item (2)
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(2);
        }

        [Fact]
        public void Enumerator_ResetBeforeEnumerating_AllowsReadingAllRemainingItem()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);

            buf.TryWrite(1).Should().BeTrue();
            buf.TryWrite(2).Should().BeTrue();

            using var enumerator = buf.GetEnumerator();

            // Reset enumerator state (does not restore buffer contents)
            enumerator.Reset();

            // Read first item
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(1);

            // After reset MoveNext should read the next available item (2)
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(2);
        }

        [Fact]
        public void IEnumerable_GetEnumerator_ReturnsNonGenericEnumeratorThatWorks()
        {
            var buf = new SingleReaderWriterRingBuffer<int>(3);
            buf.TryWrite(5).Should().BeTrue();

            IEnumerable nonGeneric = buf;
            var enumerator = nonGeneric.GetEnumerator();

            // MoveNext and Current should work using non-generic IEnumerator
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Should().Be(5);

            enumerator.MoveNext().Should().BeFalse(); // exhausted
        }
    }
}