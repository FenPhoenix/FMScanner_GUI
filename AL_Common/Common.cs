﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using static System.StringComparison;

namespace AL_Common;

[PublicAPI]
public static class Common
{
    #region Fields / classes

    // Class instead of enum so we don't have to keep casting its fields
    public static class ByteSize
    {
        public const int KB = 1024;
        public const int MB = KB * 1024;
        public const int GB = MB * 1024;
    }

    /// <summary>
    /// Stores a filename/index pair for quick lookups into a zip file.
    /// </summary>
    public readonly struct NameAndIndex
    {
        public readonly string Name;
        public readonly int Index;

        public NameAndIndex(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public NameAndIndex(string name)
        {
            Name = name;
            Index = -1;
        }
    }

    #region Custom hash tables

    private static readonly PathComparer _pathComparer = new();
    public sealed class PathComparer : StringComparer
    {
        // Allocations here, but this doesn't ever seem to get hit for Add() or Contains() calls
        public override int Compare(string? x, string? y)
        {
            return x == y ? 0 :
                x == null ? -1 :
                y == null ? 1 :
                string.Compare(x.ToBackSlashes(), y.ToBackSlashes(), StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(string? x, string? y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;

            return x.PathEqualsI(y);
        }

        // @MEM(PathComparer/GetHashCode): We allocate if the string is not already backslash separators
        public override int GetHashCode(string obj) => obj == null
            ? throw new ArgumentNullException(nameof(obj))
            : OrdinalIgnoreCase.GetHashCode(obj.ToBackSlashes());
    }

    /// <summary>
    /// A HashSet&lt;<see cref="string"/>&gt; where lookups are case-insensitive and directory separator-insensitive
    /// </summary>
    public sealed class HashSetPathI : HashSet<string>
    {
        public HashSetPathI() : base(_pathComparer) { }

        public HashSetPathI(int capacity) : base(capacity, _pathComparer) { }

        public HashSetPathI(IEnumerable<string> collection) : base(collection, _pathComparer) { }

        /// <inheritdoc cref="HashSet{T}.Add"/>
        public new bool Add(string value) => base.Add(value.ToBackSlashes());
    }

    /// <summary>
    /// HashSet&lt;<see langword="string"/>&gt; that uses <see cref="StringComparer.OrdinalIgnoreCase"/> for equality comparison.
    /// </summary>
    public sealed class HashSetI : HashSet<string>
    {
        public HashSetI() : base(StringComparer.OrdinalIgnoreCase) { }

        public HashSetI(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase) { }

        public HashSetI(IEnumerable<string> collection) : base(collection, StringComparer.OrdinalIgnoreCase) { }
    }

    /// <summary>
    /// Dictionary&lt;<see langword="string"/>, TValue&gt; that uses <see cref="StringComparer.OrdinalIgnoreCase"/> for equality comparison.
    /// Since the key type will always be <see langword="string"/>, only the value type is specifiable.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public sealed class DictionaryI<TValue> : Dictionary<string, TValue>
    {
        public DictionaryI() : base(StringComparer.OrdinalIgnoreCase) { }

        public DictionaryI(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase) { }

#if false
        public DictionaryI(IDictionary<string, TValue> collection) : base(collection, StringComparer.OrdinalIgnoreCase) { }
#endif
    }

    #endregion

    public static readonly byte[] RTFHeaderBytes =
    {
        (byte)'{',
        (byte)'\\',
        (byte)'r',
        (byte)'t',
        (byte)'f',
        (byte)'1'
    };

    public static readonly byte[] MAPPARAM =
    {
        (byte)'M',
        (byte)'A',
        (byte)'P',
        (byte)'P',
        (byte)'A',
        (byte)'R',
        (byte)'A',
        (byte)'M'
    };

    #region Preset char arrays

    // Perf, for passing to Split(), Trim() etc. so we don't allocate all the time
    public static readonly char[] CA_Comma = { ',' };
    public static readonly char[] CA_Semicolon = { ';' };
    public static readonly char[] CA_CommaSemicolon = { ',', ';' };
    public static readonly char[] CA_CommaSpace = { ',', ' ' };
    public static readonly char[] CA_Backslash = { '\\' };
    public static readonly char[] CA_BS_FS = { '\\', '/' };
    public static readonly char[] CA_BS_FS_Space = { '\\', '/', ' ' };
    public static readonly char[] CA_Plus = { '+' };

    #endregion

    // How many times have you thought, "Gee, I wish I could just reach in and grab that backing array from
    // that List, instead of taking the senseless performance hit of having it copied to a newly allocated
    // array all the time in a ToArray() call"? Hooray!
    /// <summary>
    /// Because this list exposes its internal array and also doesn't clear said array on <see cref="ClearFast"/>,
    /// it must be used with care.
    /// <para>
    /// -Only use this with value types. Reference types will be left hanging around in the array.
    /// </para>
    /// <para>
    /// -The internal array is there so you can get at it without incurring an allocation+copy.
    ///  It can very easily become desynced with the <see cref="ListFast{T}"/> if you modify it.
    /// </para>
    /// <para>
    /// -Only use the internal array in conjunction with the <see cref="Count"/> property.
    ///  Using the <see cref="ItemsArray"/>.Length value will get the array's actual length, when what you
    ///  wanted was the list's "virtual" length. This is the same as a normal List except with a normal List
    ///  the array is private so you can't have that problem.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    public sealed class ListFast<T>
    {
        public T[] ItemsArray;
        private int _itemsArrayLength;

        /// <summary>
        /// Properties are slow. You can set this from outside if you know what you're doing.
        /// </summary>
        public int Count;

        /// <summary>
        /// No bounds checking, so use caution!
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get => ItemsArray[index];
            set => ItemsArray[index] = value;
        }

        public ListFast(int capacity)
        {
            ItemsArray = new T[capacity];
            _itemsArrayLength = capacity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Count == _itemsArrayLength) EnsureCapacity(Count + 1);
            ItemsArray[Count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(ListFast<T> items, int count)
        {
            EnsureCapacity(Count + count);
            // We usually add small enough arrays that a loop is faster
            for (int i = 0; i < count; i++)
            {
                ItemsArray[Count + i] = items[i];
            }
            Count += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddFast(T item) => ItemsArray[Count++] = item;

        public void InsertAtZeroFast(T item)
        {
            Array.Copy(ItemsArray, 0, ItemsArray, 1, Count);
            ItemsArray[0] = item;
            Count++;
        }

        /*
        Honestly, for fixed-size value types, doing an Array.Clear() is completely unnecessary. For reference
        types, you definitely want to clear it to get rid of all the references, but for ints or chars etc.,
        all a clear does is set a bunch of fixed-width values to other fixed-width values. You don't save
        space and you don't get rid of loose references, all you do is waste an alarming amount of time. We
        drop fully 200ms from the Unicode parser just by using the fast clear!
        */
        /// <summary>
        /// Just sets <see cref="Count"/> to 0. Doesn't zero out the array or do anything else whatsoever.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearFast() => Count = 0;

        public int Capacity
        {
            get => _itemsArrayLength;
            set
            {
                if (value == _itemsArrayLength) return;
                if (value > 0)
                {
                    T[] objArray = new T[value];
                    if (Count > 0) Array.Copy(ItemsArray, 0, objArray, 0, Count);
                    ItemsArray = objArray;
                    _itemsArrayLength = value;
                }
                else
                {
                    ItemsArray = Array.Empty<T>();
                    _itemsArrayLength = 0;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int min)
        {
            if (_itemsArrayLength >= min) return;
            int num = _itemsArrayLength == 0 ? 4 : _itemsArrayLength * 2;
            if ((uint)num > 2146435071U) num = 2146435071;
            if (num < min) num = min;
            Capacity = num;
        }
    }

    private static bool? _fieldStreamBufferFieldFound;
    private static FieldInfo? _fieldStreamBufferFieldInfo;

    public sealed class FileStream_LengthCached : FileStream
    {
        private long _length = -1;
        public override long Length
        {
            get
            {
                if (_length == -1)
                {
                    _length = base.Length;
                }
                return _length;
            }
        }

        public FileStream_LengthCached(
            string path,
            FileMode mode,
            FileAccess access,
            FileShare share,
            int bufferSize)
            : base(path, mode, access, share, bufferSize)
        {
        }

        public FileStream_LengthCached(
            string path,
            FileMode mode,
            FileAccess access,
            FileShare share)
            : base(path, mode, access, share)
        {
        }
    }

    public const RegexOptions IgnoreCaseInvariant = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

    #endregion

    #region Methods

    #region Stream reading

    public static FileStream_LengthCached GetReadModeFileStreamWithCachedBuffer(string path, byte[] buffer)
    {
        buffer.Clear();

        if (_fieldStreamBufferFieldFound == null)
        {
            try
            {
                // @NET5(FileStream buffering): Newer .NETs (since the FileStream "strategy" additions) are totally different
                // We'd have to see if they added a way to pass in a buffer, and if not, we'd have to write totally
                // different code to get at the buffer here for newer .NETs.
                // typeof(FileStream) (base type) because that's the type where the buffer field is
                _fieldStreamBufferFieldInfo = typeof(FileStream)
                    .GetField(
                        "_buffer",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                _fieldStreamBufferFieldFound = _fieldStreamBufferFieldInfo != null &&
                                               _fieldStreamBufferFieldInfo.FieldType == typeof(byte[]);
            }
            catch
            {
                _fieldStreamBufferFieldFound = false;
                _fieldStreamBufferFieldInfo = null;
            }
        }

        var fs =
            _fieldStreamBufferFieldFound == true
                ? new FileStream_LengthCached(path, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length)
                : new FileStream_LengthCached(path, FileMode.Open, FileAccess.Read, FileShare.Read);

        if (_fieldStreamBufferFieldFound == true)
        {
            try
            {
                _fieldStreamBufferFieldInfo?.SetValue(fs, buffer);
            }
            catch
            {
                _fieldStreamBufferFieldFound = false;
                _fieldStreamBufferFieldInfo = null;
            }
        }

        return fs;
    }

    public static int ReadAll(this Stream stream, byte[] buffer, int offset, int count)
    {
        int bytesReadRet = 0;
        int startPosThisRound = offset;
        while (true)
        {
            int bytesRead = stream.Read(buffer, startPosThisRound, count);
            if (bytesRead <= 0) break;
            bytesReadRet += bytesRead;
            startPosThisRound += bytesRead;
            count -= bytesRead;
        }

        return bytesReadRet;
    }

    #endregion

    #region String

    /// <summary>
    /// Uses <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool StartsWithO(this string str, string value) => str.StartsWith(value, Ordinal);

    /// <summary>
    /// Uses <see cref="StringComparison.Ordinal"/>.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EndsWithO(this string str, string value) => str.EndsWith(value, Ordinal);

    #region ASCII-specific

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiUpper(this char c) => (uint)(c - 'A') <= 'Z' - 'A';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiLower(this char c) => (uint)(c - 'a') <= 'z' - 'a';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsIAscii(this char char1, char char2) =>
        char1 == char2 ||
        (char1.IsAsciiUpper() && char2.IsAsciiLower() && char1 == char2 - 32) ||
        (char1.IsAsciiLower() && char2.IsAsciiUpper() && char1 == char2 + 32);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiAlpha(this char c) => (((uint)c - 'A') & ~0x20) < 26;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiAlpha(this byte b) => (((uint)b - 'A') & ~0x20) < 26;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiNumeric(this char c) => (uint)(c - '0') <= '9' - '0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiNumeric(this byte c) => (uint)(c - '0') <= '9' - '0';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAsciiAlphanumeric(this char c) => ((((uint)c - 'A') & ~0x20) < 26) || ((uint)(c - '0') <= '9' - '0');

    public static bool IsAsciiAlphaUpper(this string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (!str[i].IsAsciiUpper()) return false;
        }
        return true;
    }

    public static bool IsAsciiLower(this string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (c > 127 || ((uint)(c - 'A') <= 'Z' - 'A')) return false;
        }
        return true;
    }

    public static bool IsAsciiLower(this string str, int start)
    {
        for (int i = start; i < str.Length; i++)
        {
            char c = str[i];
            if (c > 127 || ((uint)(c - 'A') <= 'Z' - 'A')) return false;
        }
        return true;
    }

    #endregion

    #region Empty / whitespace checks

    /// <summary>
    /// Returns true if <paramref name="value"/> is null or empty.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [ContractAnnotation("null => true")]
    public static bool IsEmpty([NotNullWhen(false)] this string? value) => string.IsNullOrEmpty(value);

    /// <summary>
    /// Returns true if <paramref name="value"/> is null, empty, or whitespace.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [ContractAnnotation("null => true")]
    public static bool IsWhiteSpace([NotNullWhen(false)] this string? value) => string.IsNullOrWhiteSpace(value);

    #endregion

    #region Equals

    /// <summary>
    /// Determines whether this string and a specified <see langword="string"/> object have the same value.
    /// Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    public static bool EqualsI(this string first, string second) => first.Equals(second, OrdinalIgnoreCase);

    /// <summary>
    /// Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EqualsTrue(this string value) => string.Equals(value, bool.TrueString, OrdinalIgnoreCase);

    /// <summary>
    /// Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EqualsFalse(this string value) => string.Equals(value, bool.FalseString, OrdinalIgnoreCase);

    /// <summary>
    /// Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexAfterEq"></param>
    /// <returns></returns>
    public static bool EndEqualsTrue(this string value, int indexAfterEq)
    {
        int valueLen = value.Length;
        return valueLen - indexAfterEq == 4 &&
               (value[valueLen - 4] == 'T' || value[valueLen - 4] == 't') &&
               (value[valueLen - 3] == 'R' || value[valueLen - 3] == 'r') &&
               (value[valueLen - 2] == 'U' || value[valueLen - 2] == 'u') &&
               (value[valueLen - 1] == 'E' || value[valueLen - 1] == 'e');
    }

    /// <summary>
    /// Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indexAfterEq"></param>
    /// <returns></returns>
    public static bool EndEqualsFalse(this string value, int indexAfterEq)
    {
        int valueLen = value.Length;
        return valueLen - indexAfterEq == 5 &&
               (value[valueLen - 5] == 'F' || value[valueLen - 5] == 'f') &&
               (value[valueLen - 4] == 'A' || value[valueLen - 4] == 'a') &&
               (value[valueLen - 3] == 'L' || value[valueLen - 3] == 'l') &&
               (value[valueLen - 2] == 'S' || value[valueLen - 2] == 's') &&
               (value[valueLen - 1] == 'E' || value[valueLen - 1] == 'e');
    }

    /// <summary>
    /// Uses ASCII case-insensitivity. Should not be used with non-ASCII values!
    /// </summary>
    /// <param name="str"></param>
    /// <param name="str2"></param>
    /// <param name="indexAfterEq"></param>
    /// <returns></returns>
    public static bool ValueEqualsIAscii(this string str, string str2, int indexAfterEq)
    {
        int strLen = str.Length;
        int str2Len = str2.Length;

        if (strLen - indexAfterEq != str2Len) return false;

        for (int i = indexAfterEq; i < strLen; i++)
        {
            if (!str[i].EqualsIAscii(str2[i - indexAfterEq]))
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Contains

    public static bool Contains(this string value, char character) => value.IndexOf(character) >= 0;

    public static bool Contains(this string value, string substring, StringComparison comparison) =>
        // If substring is empty, IndexOf(string) returns 0, which would be a false "success" return
        !value.IsEmpty() && !substring.IsEmpty() && value.IndexOf(substring, comparison) >= 0;

    /// <summary>
    /// Determines whether a string contains a specified substring. Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static bool ContainsI(this string value, string substring) => value.Contains(substring, OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether a string[] contains a specified element. Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static bool ContainsI(this string[] value, string substring)
    {
        for (int i = 0; i < value.Length; i++) if (value[i].Equals(substring, OrdinalIgnoreCase)) return true;
        return false;
    }

    /// <summary>
    /// Determines whether a List&lt;string&gt; contains a specified element. Uses <see cref="StringComparison.OrdinalIgnoreCase"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="substring"></param>
    /// <returns></returns>
    public static bool ContainsI(this List<string> value, string substring)
    {
        for (int i = 0; i < value.Count; i++) if (value[i].Equals(substring, OrdinalIgnoreCase)) return true;
        return false;
    }

    /// <summary>
    /// Very simple "fuzzy" search, only finds exact strings or strings present with other chars in between.
    /// <br/>
    /// Does no Levenshtein or anything fancy.
    /// </summary>
    /// <param name="hay"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    public static (bool Matched, bool ExactMatch)
    ContainsI_Subsequence(this string hay, string needle)
    {
        var fail = (false, false);

        int hayLength = hay.Length;
        int needleLength = needle.Length;

        if (needleLength == 0) return fail;
        // Don't do a needle > hay length check, because we want to support many duplicate chars (possibly
        // beyond the hay length) in the needle

        /*
        This algo sometimes rejects results that have the actual exact string in them, and if you try to
        tune it so it doesn't, then it gets other problems. It's just too simplistic to really work that
        well, so do a strict check first to cover that case.
        */
        if (hay.ContainsI(needle)) return (true, true);

        // Repetition everywhere so that we make sure only the ascii path runs if it's ascii, because with
        // big if((this and that) or that) statements, the non-ascii path always runs even if we're ascii and
        // blah.

        int needleUsed = 0;
        int skippedInARow = 0;

        int startIndex = -1;
        for (int i = 0; i < hayLength; i++)
        {
            if (hay[i] < 128 && needle[0] < 128)
            {
                if (hay[i].EqualsIAscii(needle[0]))
                {
                    startIndex = i;
                    break;
                }
            }
            else if (hay[i].EqualsIAscii(needle[0]) || hay[i].ToString().EqualsI(needle[0].ToString()))
            {
                startIndex = i;
                break;
            }
        }

        if (startIndex == -1) return fail;

        for (int i = startIndex; i < hayLength; ++i)
        {
            if (needleUsed == needleLength)
            {
                return (true, false);
            }

            if (skippedInARow > 2) return fail;

            char hayChar = hay[i];
            char needleChar = needle[needleUsed];

            // Don't allocate unless we need to...
            if (hayChar < 128 && needleChar < 128)
            {
                if (hayChar.EqualsIAscii(needleChar))
                {
                    skippedInARow = 0;
                    char lastChar;
                    char currentChar;
                    do
                    {
                        ++needleUsed;
                        lastChar = needleChar;
                    } while (needleUsed < needleLength - 1 && ((currentChar = needle[needleUsed]).EqualsIAscii(lastChar) || char.IsWhiteSpace(currentChar)));
                }
                else if (!char.IsWhiteSpace(hayChar))
                {
                    ++skippedInARow;
                }
            }
            else
            {
                if (hayChar.EqualsIAscii(needleChar) || hayChar.ToString().EqualsI(needleChar.ToString()))
                {
                    skippedInARow = 0;
                    char lastChar;
                    do
                    {
                        ++needleUsed;
                        lastChar = needleChar;
                    } while (needleUsed < needleLength - 1 && needle[needleUsed].EqualsIAscii(lastChar));
                }
                else if (!char.IsWhiteSpace(hayChar))
                {
                    ++skippedInARow;
                }
            }
        }

        return (needleUsed == needleLength, false);
    }

    #endregion

    #endregion

    #region Numeric

    #region Clamping

    /// <summary>
    /// Clamps a number to between <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T> =>
        value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;

    /// <summary>
    /// If <paramref name="value"/> is less than zero, returns zero. Otherwise, returns <paramref name="value"/>
    /// unchanged.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ClampToZero(this int value) => Math.Max(value, 0);

    public static float ClampZeroToOne(this float value) => value.Clamp(0, 1.0f);

    /// <summary>
    /// Clamps a number to <paramref name="min"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <returns></returns>
    public static T ClampToMin<T>(this T value, T min) where T : IComparable<T> => value.CompareTo(min) < 0 ? min : value;

    #endregion

    #region Percent

    public static int GetPercentFromValue_Int(int current, int total) => total == 0 ? 0 : (100 * current) / total;
    public static float GetValueFromPercent_Float(float percent, int total) => (percent / 100f) * total;
#if false
    public static double GetPercentFromValue_Double(int current, int total) => total == 0 ? 0 : (double)(100 * current) / total;
    public static long GetValueFromPercent(double percent, long total) => (long)((percent / 100) * total);
    public static int GetValueFromPercent(double percent, int total) => (int)((percent / 100d) * total);
    public static int GetValueFromPercent_Rounded(double percent, int total) => (int)Math.Round((percent / 100d) * total, 1, MidpointRounding.AwayFromZero);
    public static double GetValueFromPercent_Double(double percent, int total) => (percent / 100d) * total;
#endif

    #endregion

    #region TryParse Invariant

    /// <summary>
    /// Calls <see langword="float"/>.TryParse(<paramref name="s"/>, <see cref="NumberStyles.Float"/>, <see cref="NumberFormatInfo.InvariantInfo"/>, out <see langword="float"/> <paramref name="result"/>);
    /// </summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="result"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Float_TryParseInv(string s, out float result)
    {
        return float.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);
    }

    #region Disabled until needed

#if false

    /// <summary>
    /// Calls <see langword="double"/>.TryParse(<paramref name="s"/>, <see cref="NumberStyles.Float"/>, <see cref="NumberFormatInfo.InvariantInfo"/>, out <see langword="double"/> <paramref name="result"/>);
    /// </summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="result"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Double_TryParseInv(string s, out double result)
    {
        return double.TryParse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out result);
    }

#endif

    #endregion

    /// <summary>
    /// Calls <see langword="int"/>.TryParse(<paramref name="s"/>, <see cref="NumberStyles.Integer"/>, <see cref="NumberFormatInfo.InvariantInfo"/>, out <see langword="int"/> <paramref name="result"/>);
    /// </summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="result"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Int_TryParseInv(string s, out int result)
    {
        return int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
    }

    /// <summary>
    /// Calls <see langword="uint"/>.TryParse(<paramref name="s"/>, <see cref="NumberStyles.Integer"/>, <see cref="NumberFormatInfo.InvariantInfo"/>, out <see langword="uint"/> <paramref name="result"/>);
    /// </summary>
    /// <param name="s">A string representing a number to convert.</param>
    /// <param name="result"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool UInt_TryParseInv(string s, out uint result)
    {
        return uint.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result);
    }

    #endregion

    #endregion

    #region File/Path

    #region Forward/backslash conversion

    public static string ToForwardSlashes(this string value) => value.Replace('\\', '/');

    public static string ToForwardSlashes_Net(this string value)
    {
        return value.StartsWithO(@"\\") ? @"\\" + value.Substring(2).ToForwardSlashes() : value.ToForwardSlashes();
    }

    public static string ToBackSlashes(this string value) => value.Replace('/', '\\');

    public static string ToSystemDirSeps(this string value) => value.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

    public static string ToSystemDirSeps_Net(this string value)
    {
        return value.StartsWithO(@"\\") ? @"\\" + value.Substring(2).ToSystemDirSeps() : value.ToSystemDirSeps();
    }

    public static string MakeUNCPath(string path) => path.StartsWithO(@"\\") ? @"\\?\UNC\" + path.Substring(2) : @"\\?\" + path;

    #endregion

    #region ReadAllLines

    // Return the original lists to avoid the wasteful and useless allocation of the array conversion that
    // you get with the built-in methods
    public static List<string> File_ReadAllLines_List(string path)
    {
        var ret = new List<string>();
        using var sr = new StreamReaderCustom.SRC_Wrapper(File_OpenReadFast(path), new StreamReaderCustom());
        while (sr.Reader.ReadLine() is { } str)
        {
            ret.Add(str);
        }
        return ret;
    }

    public static FileStream_LengthCached File_OpenReadFast(string path)
    {
        return new FileStream_LengthCached(path, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    #endregion

    #region Path-specific string queries (separator-agnostic)

    public static bool PathContainsI(this List<string> value, string substring)
    {
        for (int i = 0; i < value.Count; i++) if (value[i].PathEqualsI(substring)) return true;
        return false;
    }

    #region Disabled until needed

#if false
    public static bool PathContainsI(this string[] value, string substring)
    {
        for (int i = 0; i < value.Length; i++) if (value[i].PathEqualsI(substring)) return true;
        return false;
    }
#endif

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDirSep(this char character) => character is '/' or '\\';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool StartsWithDirSep(this string value) => value.Length > 0 && value[0].IsDirSep();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EndsWithDirSep(this string value) => value.Length > 0 && value[value.Length - 1].IsDirSep();

    // Note: We hardcode '/' and '\' for now because we can get paths from archive files too, where the dir
    // sep chars are in no way guaranteed to match those of the OS.
    // Not like any OS is likely to use anything other than '/' or '\' anyway.

    // We hope not to have to call this too often, but it's here as a fallback.
    public static string CanonicalizePath(string value) => value.ToBackSlashes();

    /// <summary>
    /// Returns true if <paramref name="value"/> contains either directory separator character.
    /// <para>Do NOT use for full (non-relative) paths as it will count the "\\" at the start of UNC paths! </para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool Rel_ContainsDirSep(this string value)
    {
        for (int i = 0; i < value.Length; i++) if (value[i].IsDirSep()) return true;
        return false;
    }

    /// <summary>
    /// Counts the total occurrences of both directory separator characters in <paramref name="value"/>.
    /// <para>Do NOT use for full (non-relative) paths as it will count the "\\" at the start of UNC paths! </para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static int Rel_CountDirSeps(this string value, int start = 0)
    {
        int count = 0;
        for (int i = start; i < value.Length; i++) if (value[i].IsDirSep()) count++;
        return count;
    }

    /// <summary>
    /// <para>Do NOT use for full (non-relative) paths as it will count the "\\" at the start of UNC paths! </para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static bool Rel_DirSepCountIsAtLeast(this string value, int count, int start = 0)
    {
        int foundCount = 0;
        for (int i = start; i < value.Length; i++)
        {
            if (value[i].IsDirSep()) foundCount++;
            if (foundCount == count) return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the number of directory separators in a string, earlying-out once it's counted <paramref name="maxToCount"/>
    /// occurrences.
    /// <para>Do NOT use for full (non-relative) paths as it will count the "\\" at the start of UNC paths! </para>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="maxToCount">The maximum number of occurrences to count before earlying-out.</param>
    /// <returns></returns>
    public static int Rel_CountDirSepsUpToAmount(this string value, int maxToCount)
    {
        int foundCount = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i].IsDirSep())
            {
                foundCount++;
                if (foundCount == maxToCount) break;
            }
        }

        return foundCount;
    }

    /// <summary>
    /// Returns the last index of either directory separator character in <paramref name="value"/>.
    /// <para>Do NOT use for full (non-relative) paths as it will count the "\\" at the start of UNC paths! </para>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Rel_LastIndexOfDirSep(this string value)
    {
        int i1 = value.LastIndexOf('/');
        int i2 = value.LastIndexOf('\\');

        return i1 == -1 && i2 == -1 ? -1 : Math.Max(i1, i2);
    }

    /// <summary>
    /// Path equality check ignoring case and directory separator differences.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool PathEqualsI(this string first, string second)
    {
        if (first == second) return true;

        int firstLen = first.Length;
        if (firstLen != second.Length) return false;

        for (int i = 0; i < firstLen; i++)
        {
            char fc = first[i];
            char sc = second[i];

            if (fc > 127 || sc > 127)
            {
                return first.Equals(second, OrdinalIgnoreCase) ||
                       CanonicalizePath(first).Equals(CanonicalizePath(second), OrdinalIgnoreCase);
            }

            if (!AsciiPathCharsConsideredEqual_Win(fc, sc)) return false;
        }

        return true;
    }

    /// <summary>
    /// Path starts-with check ignoring case and directory separator differences.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool PathStartsWithI(this string first, string second)
    {
        if (first.Length < second.Length) return false;

        for (int i = 0; i < second.Length; i++)
        {
            char fc = first[i];
            char sc = second[i];

            if (fc > 127 || sc > 127)
            {
                return first.StartsWith(second, OrdinalIgnoreCase) ||
                       CanonicalizePath(first).StartsWith(CanonicalizePath(second), OrdinalIgnoreCase);
            }

            if (!AsciiPathCharsConsideredEqual_Win(fc, sc)) return false;
        }

        return true;
    }

    /// <summary>
    /// Path ends-with check ignoring case and directory separator differences.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool PathEndsWithI(this string first, string second)
    {
        if (first.Length < second.Length) return false;

        for (int fi = first.Length - second.Length, si = 0; fi < first.Length; fi++, si++)
        {
            char fc = first[fi];
            char sc = second[si];

            if (fc > 127 || sc > 127)
            {
                return first.EndsWith(second, OrdinalIgnoreCase) ||
                       CanonicalizePath(first).EndsWith(CanonicalizePath(second), OrdinalIgnoreCase);
            }

            if (!AsciiPathCharsConsideredEqual_Win(fc, sc)) return false;
        }

        return true;
    }

    #region Disabled until needed

#if false
    public static bool PathContainsI_Dir(this List<string> value, string substring)
    {
        for (int i = 0; i < value.Count; i++) if (value[i].PathEqualsI_Dir(substring)) return true;
        return false;
    }

    public static bool PathContainsI_Dir(this string[] value, string substring)
    {
        for (int i = 0; i < value.Length; i++) if (value[i].PathEqualsI_Dir(substring)) return true;
        return false;
    }

    /// <summary>
    /// Counts the total occurrences of both directory separator characters in <paramref name="value"/>.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static int CountDirSeps(this string value, int start = 0)
    {
        int count = 0;
        for (int i = start; i < value.Length; i++) if (value[i].IsDirSep()) count++;
        return count;
    }

    /// <summary>
    /// Counts dir seps up to <paramref name="count"/> occurrences and then returns, skipping further counting.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="count"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    internal static bool DirSepCountIsAtLeast(this string value, int count, int start = 0)
    {
        int foundCount = 0;
        for (int i = start; i < value.Length; i++)
        {
            if (value[i].IsDirSep()) foundCount++;
            if (foundCount == count) return true;
        }

        return false;
    }
#endif

    #endregion

    #region Equality / StartsWith / EndsWith

    public static bool PathSequenceEqualI_Dir(this IList<string> first, IList<string> second)
    {
        int firstCount;
        if ((firstCount = first.Count) != second.Count) return false;

        for (int i = 0; i < firstCount; i++) if (!first[i].PathEqualsI_Dir(second[i])) return false;
        return true;
    }

    #region Disabled until needed

#if false
    public static bool PathSequenceEqualI(this IList<string> first, IList<string> second)
    {
        int firstCount;
        if ((firstCount = first.Count) != second.Count) return false;

        for (int i = 0; i < firstCount; i++) if (!first[i].PathEqualsI(second[i])) return false;
        return true;
    }
#endif

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AsciiPathCharsConsideredEqual_Win(char char1, char char2) =>
        char1.EqualsIAscii(char2) ||
        (char1.IsDirSep() && char2.IsDirSep());

    /// <summary>
    /// Path equality check ignoring case and directory separator differences. Directory version: Ignores
    /// trailing path separators.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool PathEqualsI_Dir(this string first, string second) => first.TrimEnd(CA_BS_FS).PathEqualsI(second.TrimEnd(CA_BS_FS));

    #endregion

    #endregion

    /// <summary>
    /// Just removes the extension from a filename, without the rather large overhead of
    /// Path.GetFileNameWithoutExtension().
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string RemoveExtension(this string fileName)
    {
        int i;
        return (i = fileName.LastIndexOf('.')) == -1 ? fileName : fileName.Substring(0, i);
    }

    /// <summary>
    /// Determines whether this string ends with a file extension. Obviously only makes sense for strings
    /// that are supposed to be file names.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool HasFileExtension(this string value)
    {
        int lastDotIndex = value.LastIndexOf('.');
        return lastDotIndex > value.LastIndexOf('/') ||
               lastDotIndex > value.LastIndexOf('\\');
    }

    #endregion

    #region Count chars

    #region Disabled until needed

#if false

    /// <summary>
    /// Returns the number of times a character appears in a string.
    /// Avoids whatever silly overhead junk Count(predicate) is doing.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="character"></param>
    /// <returns></returns>
    public static int CountChars(this string value, char character)
    {
        int count = 0;
        for (int i = 0; i < value.Length; i++) if (value[i] == character) count++;

        return count;
    }

#endif

    #endregion

    /// <summary>
    /// Returns the number of times a character appears in a string, earlying-out once it's counted <paramref name="maxToCount"/>
    /// occurrences.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="character"></param>
    /// <param name="maxToCount">The maximum number of occurrences to count before earlying-out.</param>
    /// <returns></returns>
    public static int CountCharsUpToAmount(this string value, char character, int maxToCount)
    {
        int foundCount = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == character)
            {
                foundCount++;
                if (foundCount == maxToCount) break;
            }
        }

        return foundCount;
    }

    public static bool CharCountIsAtLeast(this string value, char character, int count)
    {
        int foundCount = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == character)
            {
                foundCount++;
                if (foundCount == count) return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CharAppearsExactlyOnce(this string value, char character) => value.CountCharsUpToAmount(character, 2) == 1;

    #endregion

    #region Enumerable

    #region Array initialization

    /// <summary>
    /// Returns an array of type <typeparamref name="T"/> with all elements initialized to non-null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="length"></param>
    public static T[] InitializedArray<T>(int length) where T : new()
    {
        T[] ret = new T[length];
        for (int i = 0; i < length; i++) ret[i] = new T();
        return ret;
    }

    /// <summary>
    /// Returns an array of type <typeparamref name="T"/> with all elements initialized to <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="length"></param>
    /// <param name="value">The value to initialize all elements with.</param>
    public static T[] InitializedArray<T>(int length, T value) where T : new()
    {
        T[] ret = new T[length];
        for (int i = 0; i < length; i++) ret[i] = value;
        return ret;
    }

    /// <summary>
    /// Returns two arrays of type <typeparamref name="T1"/> and <typeparamref name="T2"/> respectively,
    /// with all elements initialized to non-null. Uses a single assignment loop for performance.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="length"></param>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    public static void InitializeArrays<T1, T2>(int length,
        out T1[] array1,
        out T2[] array2)
        where T1 : new()
        where T2 : new()
    {
        array1 = new T1[length];
        array2 = new T2[length];
        for (int i = 0; i < length; i++)
        {
            array1[i] = new T1();
            array2[i] = new T2();
        }
    }

    #endregion

    public static T[] CombineArrays<T>(params T[][] arrays)
    {
        int totalLen = 0;
        for (int i = 0; i < arrays.Length; i++)
        {
            totalLen += arrays[i].Length;
        }

        T[] ret = new T[totalLen];

        int pos = 0;
        for (int i = 0; i < arrays.Length; i++)
        {
            T[] array = arrays[i];
            int arrayLen = array.Length;

            Array.Copy(array, 0, ret, pos, arrayLen);

            pos += arrayLen;
        }

        return ret;
    }

    /// <summary>
    /// Clears <paramref name="array"/> and returns it back.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array to clear.</param>
    /// <returns>A cleared version of <paramref name="array"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] Cleared<T>(this T[] array)
    {
        Array.Clear(array, 0, array.Length);
        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear<T>(this T[] array) => Array.Clear(array, 0, array.Length);

    public static bool StartsWith(this byte[] first, byte[] second)
    {
        if (first.Length < second.Length) return false;

        for (int i = 0; i < second.Length; i++)
        {
            if (first[i] != second[i]) return false;
        }
        return true;
    }

    #region Clear and add

    public static void ClearAndAdd<T>(this List<T> list, T item)
    {
        list.Clear();
        list.Add(item);
    }

    public static void ClearAndAdd<T>(this List<T> list, IEnumerable<T> items)
    {
        list.Clear();
        list.AddRange(items);
    }

    public static void ClearAndAdd<T>(this List<T> list, List<T> items)
    {
        list.Clear();
        list.AddRange(items);
    }

    public static void ClearAndAdd<T>(this List<T> list, T[] items)
    {
        list.Clear();
        list.AddRange(items);
    }

    #endregion

    #region Dispose and clear

    public static void DisposeAll<T>(this T[] array) where T : IDisposable?
    {
        for (int i = 0; i < array.Length; i++) array[i]?.Dispose();
    }

    public static void DisposeRange<T>(this T[] array, int start, int end) where T : IDisposable?
    {
        for (int i = start; i < end; i++) array[i]?.Dispose();
    }

    #endregion

    public static HashSetI ToHashSetI(this IEnumerable<string> source) => new(source);

    public static HashSetPathI ToHashSetPathI(this IEnumerable<string> source) => new(source);

    #endregion

    #region Find / replace

    public static int FindIndexOfByteSequence(byte[] input, byte[] pattern, int start = 0)
    {
        byte firstByte = pattern[0];
        int index = Array.IndexOf(input, firstByte, start);

        while (index > -1)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (index + i >= input.Length) return -1;
                if (pattern[i] != input[index + i])
                {
                    if ((index = Array.IndexOf(input, firstByte, index + i)) == -1) return -1;
                    break;
                }

                if (i == pattern.Length - 1) return index;
            }
        }

        return -1;
    }

    public static int FindIndexOfCharSequence(string input, string pattern, int start = 0)
    {
        char firstChar = pattern[0];
        int index = input.IndexOf(firstChar, start);

        while (index > -1)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (index + i >= input.Length) return -1;
                if (pattern[i] != input[index + i])
                {
                    if ((index = input.IndexOf(firstChar, index + i)) == -1) return -1;
                    break;
                }

                if (i == pattern.Length - 1) return index;
            }
        }

        return -1;
    }

#if false
    public static int FindIndexOfByteSequence(List<byte> input, byte[] pattern, int start = 0)
    {
        byte firstByte = pattern[0];
        int index = input.IndexOf(firstByte, start);

        while (index > -1)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (index + i >= input.Count) return -1;
                if (pattern[i] != input[index + i])
                {
                    if ((index = input.IndexOf(firstByte, index + i)) == -1) return -1;
                    break;
                }

                if (i == pattern.Length - 1) return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// What it says
    /// </summary>
    /// <param name="input"></param>
    /// <param name="groupControlWord">Must start with '{', for example "{\fonttbl"</param>
    /// <param name="start"></param>
    /// <returns></returns>
    public static (bool Found, int StartIndex, int EndIndex)
    FindStartAndEndIndicesOfRtfGroup(byte[] input, byte[] groupControlWord, int start = 0)
    {
        int index = FindIndexOfByteSequence(input, groupControlWord, start: start);
        if (index == -1) return (false, -1, -1);

        int braceLevel = 1;

        for (int i = index + 1; i < input.Length; i++)
        {
            byte b = input[i];
            if (b == (byte)'{')
            {
                braceLevel++;
            }
            else if (b == (byte)'}')
            {
                braceLevel--;
            }

            if (braceLevel < 1) return (true, index, i + 1);
        }

        return (false, -1, -1);
    }

#endif

    public static void ReplaceByteSequence(byte[] input, byte[] pattern, byte[] replacePattern)
    {
        byte firstByte = pattern[0];
        int index = Array.IndexOf(input, firstByte);
        int pLen = pattern.Length;

        while (index > -1)
        {
            for (int i = 0; i < pLen; i++)
            {
                if (index + i >= input.Length) return;
                if (pattern[i] != input[index + i])
                {
                    if ((index = Array.IndexOf(input, firstByte, index + i)) == -1) return;
                    break;
                }

                if (i == pLen - 1)
                {
                    for (int j = index, ri = 0; j < index + pLen; j++, ri++)
                    {
                        input[j] = replacePattern[ri];
                    }
                }
            }
        }
    }

#if false

    public static void ReplaceByteSequence(List<byte> input, byte[] pattern, byte[] replacePattern)
    {
        byte firstByte = pattern[0];
        int index = input.IndexOf(firstByte);
        int pLen = pattern.Length;

        while (index > -1)
        {
            for (int i = 0; i < pLen; i++)
            {
                if (index + i >= input.Count) return;
                if (pattern[i] != input[index + i])
                {
                    if ((index = input.IndexOf(firstByte, index + i)) == -1) return;
                    break;
                }

                if (i == pLen - 1)
                {
                    for (int j = index, ri = 0; j < index + pLen; j++, ri++)
                    {
                        input[j] = replacePattern[ri];
                    }
                }
            }
        }
    }

#endif

    // I don't know if this is "supposed" to be the fastest way, but every other algorithm I've tried is at
    // least 2-8x slower. IndexOf() calls an internal method TrySZIndexOf() which is obviously some voodoo
    // speed demon stuff because none of this Moyer-Bohr-Kensington-Smythe-Wappcapplet fancy stuff beats it.
    // Or maybe I just don't know what I'm doing. Either way.
    public static bool Contains(this byte[] input, byte[] pattern, int length = -1)
    {
        if (length == -1) length = input.Length;

        byte firstByte = pattern[0];
        int index = Array.IndexOf(input, firstByte, 0, length);

        while (index > -1)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (index + i >= length) return false;
                if (pattern[i] != input[index + i])
                {
                    if ((index = Array.IndexOf(input, firstByte, index + i, length - (index + i))) == -1) return false;
                    break;
                }

                if (i == pattern.Length - 1) return true;
            }
        }

        return false;
    }

    #endregion

    public static bool EqualsIfNotNull(this object? obj1, object? obj2) => obj1 != null && obj2 != null && obj1 == obj2;

    #region Set file attributes

    public static void File_UnSetReadOnly(string fileOnDiskFullPath, bool throwException = false)
    {
        try
        {
            new FileInfo(fileOnDiskFullPath).IsReadOnly = false;
        }
        catch (Exception ex)
        {
            Logger.Log("Unable to set file attributes for " + fileOnDiskFullPath, ex);
            if (throwException) throw;
        }
    }

    public static void Dir_UnSetReadOnly(string dirOnDiskFullPath, bool throwException = false)
    {
        try
        {
            // TODO: Apparently ReadOnly is ignored for directories
            // https://support.microsoft.com/en-us/topic/you-cannot-view-or-change-the-read-only-or-the-system-attributes-of-folders-in-windows-server-2003-in-windows-xp-in-windows-vista-or-in-windows-7-55bd5ec5-d19e-6173-0df1-8f5b49247165
            // Says up to Win7, doesn't say anything about later versions, but have to assume it still holds...?
            _ = new DirectoryInfo(dirOnDiskFullPath).Attributes &= ~FileAttributes.ReadOnly;
        }
        catch (Exception ex)
        {
            Logger.Log("Unable to set directory attributes for " + dirOnDiskFullPath, ex);
            if (throwException) throw;
        }
    }

    public static void DirAndFileTree_UnSetReadOnly(string path, bool throwException = false)
    {
        Dir_UnSetReadOnly(path, throwException);

        foreach (string f in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
        {
            File_UnSetReadOnly(f, throwException);
        }

        foreach (string d in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
        {
            Dir_UnSetReadOnly(d, throwException);
        }
    }

    #endregion

    #endregion
}
