namespace vm2.Threading.Buffers;

using System.Collections;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleReaderWriterRingBuffer<T> : IEnumerable<T>
{
    readonly T[] _buffer;
    int _writeIndex;
    int _readIndex;

    /// <summary>
    /// Gets the maximum number of items that can be stored.
    /// </summary>
    public int Capacity { get; init; }

    /// <summary>
    /// Gets a value indicating whether the collection is empty.
    /// </summary>
    public bool IsEmpty => _writeIndex == _readIndex;

    /// <summary>
    /// Gets a value indicating whether the buffer is full.
    /// </summary>
    public bool IsFull => Next(_writeIndex) == _readIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleReaderWriterRingBuffer{T}"/> class with the specified
    /// capacity.
    /// </summary>
    /// <param name="capacity">The maximum number of elements the ring buffer can hold. Must be greater than zero.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="capacity"/> is less than or equal to zero.</exception>
    public SingleReaderWriterRingBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        Capacity = capacity;
        _buffer = new T[capacity];
    }

    int Next(int index) => (index + 1) % Capacity;

    T Current => _readIndex != _writeIndex
                        ? _buffer[_readIndex]
                        : throw new InvalidOperationException("The buffer is empty.");

    /// <summary>
    /// Attempts to write an item to the buffer.
    /// </summary>
    /// <remarks>This method does not throw an exception if the buffer is full. Instead, it  returns <see
    /// langword="false"/> to indicate that the write operation could not be completed.</remarks>
    /// <param name="item">The item to write to the buffer.</param>
    /// <returns><see langword="true"/> if the item was successfully written to the buffer;  otherwise, <see langword="false"/>
    /// if the buffer is full.</returns>
    public bool TryWrite(T item)
    {
        if (IsFull)
            return false;
        _buffer[_writeIndex] = item;
        _writeIndex = Next(_writeIndex);
        return true;
    }

    /// <summary>
    /// Attempts to read the next item from the buffer.
    /// </summary>
    /// <remarks>This method does not throw an exception if the buffer is empty. Instead, it returns <see
    /// langword="false"/> and sets <paramref name="item"/> to the default value of <typeparamref name="T"/>.</remarks>
    /// <param name="item">When this method returns, contains the item read from the buffer if the operation succeeds; otherwise, the
    /// default value of <typeparamref name="T"/>.</param>
    /// <returns><see langword="true"/> if an item was successfully read from the buffer; otherwise, <see langword="false"/>.</returns>
    public bool TryRead(out T item)
    {
        if (IsEmpty)
        {
            item = default!;
            return false;
        }
        item = _buffer[_readIndex];
        _readIndex = Next(_readIndex);
        return true;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <remarks>
    /// The enumerator provides a simple way to access each element in the collection sequentially.
    /// Use a `foreach` loop in C# or a `For Each` loop in Visual Basic to iterate through the collection.
    /// </remarks>
    /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
    public IEnumerator<T> GetEnumerator() => new Enumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    class Enumerator : IEnumerator<T>
    {
        readonly SingleReaderWriterRingBuffer<T> _ringBuffer;
        bool _started;
        bool _finished;

        public Enumerator(SingleReaderWriterRingBuffer<T> ringBuffer)
            => _ringBuffer = ringBuffer;

        public T Current => _started
                                ? _ringBuffer.Current
                                : throw new InvalidOperationException("Start with MoveNext() or continue with Reset().");

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (!_started)
            {
                _started = true;
                return !(_finished = _ringBuffer.IsEmpty);
            }

            if (!_finished)
            {
                _ringBuffer.TryRead(out var _);
                return !(_finished = _ringBuffer.IsEmpty);
            }

            return false;
        }

        public void Reset()
        {
            if (_started)
                _ringBuffer.TryRead(out var _);
            _started = _finished = false;
        }

        public void Dispose() { }
    }
}
