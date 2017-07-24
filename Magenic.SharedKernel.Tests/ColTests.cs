using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// This class implements unit tests for ColEx utility class.
    /// </summary>    
    public class ColTest
    {
        #region Inner Classes

        /// <summary>
        /// Implement EqualityComparer for testing.
        /// </summary>  
        private class TestEqualityComparerObj : EqualityComparer<string>
        {
            public override bool Equals(string x, string y)
            {
                return (x == y);
            }

            public override int GetHashCode(string obj)
            {
                return obj.GetHashCode();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// A test for CreateSet method.
        ///</summary>
        [Fact]
        public void Col_CreateSet_1()
        {
            IEnumerable<string> seqList = new List<string>()
            {
                { "First" },
                { "Second" }
            };

            Assert.NotNull(seqList);
            Assert.NotEmpty(seqList);

            TestEqualityComparerObj comparerObj = new TestEqualityComparerObj();

            Assert.NotNull(comparerObj);
            Assert.NotNull(new HashSet<string>(seqList, comparerObj));

            ISet<string> col_SeqAndComparerAreNull = ColEx.CreateSet<string>(
                seq: null, comparer: null);

            Assert.NotNull(col_SeqAndComparerAreNull);
            Assert.IsType<HashSet<string>>(col_SeqAndComparerAreNull);
            Assert.Equal(new HashSet<string>(), col_SeqAndComparerAreNull);
            col_SeqAndComparerAreNull.Should().Equal(new HashSet<string>());

            ISet<string> col_SeqAndComparerAreNotNull = ColEx.CreateSet(
                seq: seqList, comparer: comparerObj);

            Assert.NotNull(col_SeqAndComparerAreNotNull);
            Assert.IsType<HashSet<string>>(col_SeqAndComparerAreNotNull);
            Assert.NotEmpty(col_SeqAndComparerAreNotNull);
            Assert.Equal(
                new HashSet<string>(seqList, comparerObj),
                col_SeqAndComparerAreNotNull);

            ISet<string> col_SeqIsNotNull = ColEx.CreateSet(
                seq: seqList, comparer: null);

            Assert.NotNull(col_SeqIsNotNull);
            Assert.IsType<HashSet<string>>(col_SeqIsNotNull);
            Assert.NotEmpty(col_SeqIsNotNull);
            Assert.NotNull(new HashSet<string>(seqList));            
            Assert.Equal(new HashSet<string>(seqList), col_SeqIsNotNull);

            ISet<string> col_ComprarerIsNotNull = ColEx.CreateSet(
                seq: null, comparer: comparerObj);

            Assert.NotNull(col_ComprarerIsNotNull);
            Assert.IsType<HashSet<string>>(col_ComprarerIsNotNull);
            Assert.NotNull(new HashSet<string>(comparerObj));
            Assert.Equal(new HashSet<string>(comparerObj), col_ComprarerIsNotNull);
        }

        /// <summary>
        /// A test for CreateSet.
        ///</summary>
        [Fact]
        public void Col_CreateSet_2()
        {
            IEnumerable<int> seq = Enumerable.Range(0, 7);
            ISet<int> set = ColEx.CreateSet<int>();

            Seq.List<object>(seq, set).Apply(o => Assert.NotNull(o));

            Assert.Empty(set);
            Assert.NotEmpty(seq);
            seq.Should().HaveCount(7);

            Assert.IsType<HashSet<int>>(set);

            Util.Repeat(i => Assert.True(set.Add(i)), 7);
            Assert.NotEmpty(set);
            set.Should().HaveCount(7);
            set.Should().Equal(seq);
            set.Should().Equal(ColEx.CreateVector(seq));
        }
        
        /// <summary>
        /// A test for CreateMap.
        ///</summary>
        [Fact]
        public void Col_CreateMap_1()
        {
            Dictionary<string, string> mapDictionary = new Dictionary<string, string>()
            {
                {"FirstId", "FirstValue"},
                {"SecondId", "SecondValue"}
            };

            Assert.NotNull(mapDictionary);
            Assert.NotEmpty(mapDictionary);

            TestEqualityComparerObj comparerObj = new TestEqualityComparerObj();

            Assert.NotNull(comparerObj);

            IDictionary<string, string> col_HasCapacity = ColEx
                .CreateMap<string, string>(capacity: 2);

            Assert.NotNull(col_HasCapacity);
            Assert.IsType<Dictionary<string, string>>(col_HasCapacity);
            Assert.NotNull(new Dictionary<string, string>(mapDictionary, comparerObj));
            Assert.Equal(new Dictionary<string, string>(), col_HasCapacity);

            IDictionary<string, string> col_MapAndComparerAreNull =
                ColEx.CreateMap<string, string>(map: null, comparer: null);

            Assert.NotNull(col_MapAndComparerAreNull);
            Assert.IsType<Dictionary<string, string>>(col_MapAndComparerAreNull);
            Assert.NotNull(new Dictionary<string, string>(mapDictionary, comparerObj));
            Assert.Equal(new Dictionary<string, string>(), col_MapAndComparerAreNull);
            Assert.Equal(0, col_MapAndComparerAreNull.Count());

            IDictionary<string, string> col_MapAndComparerAreNotNull = ColEx
                .CreateMap(map: mapDictionary, comparer: comparerObj);

            Assert.NotNull(col_MapAndComparerAreNotNull);
            Assert.IsType<Dictionary<string, string>>(col_MapAndComparerAreNotNull);
            Assert.NotNull(new Dictionary<string, string>(mapDictionary, comparerObj));
            Assert.Equal(
                new Dictionary<string, string>(mapDictionary, comparerObj),
                col_MapAndComparerAreNotNull);
            Assert.Equal(2, col_MapAndComparerAreNotNull.Count());

            IDictionary<string, string> col_MapIsNotNull = ColEx
                .CreateMap(map: mapDictionary, comparer: null);

            Assert.NotNull(col_MapIsNotNull);
            Assert.IsType<Dictionary<string, string>>(col_MapIsNotNull);
            Assert.NotNull(new Dictionary<string, string>(mapDictionary));
            Assert.Equal(
                col_MapIsNotNull,
                new Dictionary<string, string>(mapDictionary));
            Assert.Equal(2, col_MapIsNotNull.Count());

            IDictionary<string, string> col_ComparerIsNotNull = ColEx
                .CreateMap<string, string>(map: null, comparer: comparerObj);

            Assert.NotNull(col_ComparerIsNotNull);
            Assert.IsType<Dictionary<string, string>>(col_ComparerIsNotNull);
            Assert.NotNull(new Dictionary<string, string>(comparerObj));
            Assert.Equal(
                new Dictionary<string, string>(comparerObj),
                col_ComparerIsNotNull);
            Assert.Empty(col_ComparerIsNotNull);

            IEnumerable<KeyValuePair<string, string>> sourceList =
                new List<KeyValuePair<string, string>>()
                {
                    { new KeyValuePair<string, string>("FirstId", "FirstValue") }
                };

            Assert.NotNull(sourceList);
            Assert.NotEmpty(sourceList);

            IDictionary<string, string> col_HasSource = ColEx
                .CreateMap(source: sourceList);

            Assert.NotNull(col_HasSource);
            Assert.IsType<Dictionary<string, string>>(col_HasSource);
            Assert.NotEmpty(col_HasSource);
            Assert.NotNull(new Dictionary<string, string>(comparerObj));
            Assert.Equal(1, col_HasSource.Count());
            Assert.Equal(
                sourceList.First().Key,
                col_HasSource.First().Key);

            Assert.Throws<ArgumentNullException>(() => ColEx
                .CreateMap<int, object>(source: null));
        }

        /// <summary>
        /// A test for CreateMap.
        ///</summary>
        [Fact]
        public void Col_CreateMap_2()
        {
            Random random = RandomEx.Create(1739494172);
            IEnumerable<int> keys = Enumerable.Range(0, 13);

            Seq.List<object>(random, keys).Apply(o => Assert.NotNull(o));
            Assert.IsType<Random>(random);
            Assert.NotEmpty(keys);

            List<string> values = keys
                .Map(i => random.NextString(i, i + 4))
                .ToList();
            IDictionary<int, string> map = ColEx.CreateMap<int, string>();

            Seq.List<object>(values, map).Apply(o => Assert.NotNull(o));
            Assert.IsType<List<string>>(values);
            Assert.IsType<Dictionary<int, string>>(map);
            Assert.NotEmpty(values);
            Assert.Empty(map);

            Seq.Apply(
                keys,
                values,
                (k, v) => Assert.False(map.UpdateOrAdd(k, v)));

            map.Should().HaveCount(13);
            map.Keys.Should().Equal(keys);
            map.Values.Should().Equal(values);
            map.Should().Equal(ColEx.CreateMap(map));

            map = ColEx.CreateMap<int, string>(98);

            Assert.NotNull(map);
            Assert.Empty(map);
            map.Should().Equal(new Dictionary<int, string>());
        }

        /// <summary>
        /// A test for CreateVector.
        ///</summary>
        [Fact]
        public void Col_CreateVector_1()
        {
            IList<string> seqList = new List<string>()
            {
                {"First"},
                {"Second"}
            };

            Assert.NotNull(seqList);
            Assert.NotEmpty(seqList);

            IList<string> col_HasCapacity = ColEx.CreateVector<string>(capacity: 2);

            Assert.NotNull(col_HasCapacity);
            Assert.IsType<List<string>>(col_HasCapacity);
            Assert.Empty(col_HasCapacity);
            Assert.Equal(new List<string>(), col_HasCapacity);

            IList<string> col_CreateNew = ColEx.CreateVector<string>();

            Assert.NotNull(col_CreateNew);
            Assert.IsType<List<string>>(col_CreateNew);
            Assert.Empty(col_CreateNew);
            Assert.Equal(new List<string>(), col_CreateNew);

            IList<string> col_HasSeq = ColEx.CreateVector(seq: seqList);

            Assert.NotNull(col_HasSeq);
            Assert.IsType<List<string>>(col_HasSeq);
            Assert.NotEmpty(col_HasSeq);
            Assert.Equal(new List<string>(seqList), col_HasSeq);
            Assert.Equal(2, col_HasSeq.Count());
        }

        /// <summary>
        /// A test for CreateVector.
        ///</summary>
        [Fact]
        public void Col_CreateVector_2()
        {
            IEnumerable<int> seq = Enumerable.Range(0, 12);
            IList<int> vector = ColEx.CreateVector<int>();

            Seq.List<object>(seq, vector).Apply(o => Assert.NotNull(o));

            Assert.NotEmpty(seq);
            Assert.Empty(vector);

            Assert.IsType<List<int>>(vector);

            Util.Repeat(i => vector.Add(i), 12);

            Assert.NotEmpty(vector);
            vector.Should().HaveCount(12);
            vector.Should().Equal(seq);
            vector.Should().Equal(ColEx.CreateVector(seq));
        }

        /// <summary>
        /// A test for CreateKVP.
        ///</summary>
        [Fact]
        public void Col_CreateKVP_1()
        {
            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(
                "firstId", "perls");
            KeyValuePair<string, string> col_HasKeyAndValue = ColEx
                .CreateKVP(key: kvp.Key, value: kvp.Value);

            Assert.NotNull(col_HasKeyAndValue);
            Assert.IsType<KeyValuePair<string, string>>(col_HasKeyAndValue);
            Assert.Equal(kvp.Key, col_HasKeyAndValue.Key);
            Assert.Equal(kvp.Value, col_HasKeyAndValue.Value);

            KeyValuePair<string, string> col_HasNoKeyAndValue = ColEx
                .CreateKVP<string, string>(key: null, value: null);

            Assert.NotNull(col_HasNoKeyAndValue);
            Assert.IsType<KeyValuePair<string, string>>(col_HasNoKeyAndValue);
            Assert.Null(col_HasNoKeyAndValue.Key);
            Assert.Null(col_HasNoKeyAndValue.Value);
        }

        /// <summary>
        /// A test for CreateKVP.
        ///</summary>
        [Fact]
        public void Col_CreateKVP_2()
        {
            string key = "Random";
            int value = 516;
            KeyValuePair<string, int> kvp = ColEx.CreateKVP(key, value);

            Assert.NotNull(kvp);
            Assert.IsType<KeyValuePair<string, int>>(kvp);
            Assert.Equal(key, kvp.Key);
            Assert.Equal(value, kvp.Value);

            int sevenInt = 7;
            string sevenString = "Seven";

            Assert.Equal(
                new KeyValuePair<int, string>(sevenInt, sevenString),
                ColEx.CreateKVP(sevenInt, sevenString));
        }

        /// <summary>
        /// A test for Repeat.
        ///</summary>
        [Fact]
        public void Col_Repeat_1()
        {
            int repeatCount = 5;
            string initialValue = "Initial Value";
            IDictionary<int, string> mapDictionary = new Dictionary<int, string>()
            {
                { 1, initialValue }
            };
            Func<int, KeyValuePair<int, string>> fn = (x) => mapDictionary
                .First(y => y.Key == 1);

            Assert.NotNull(mapDictionary);
            Assert.NotEmpty(mapDictionary);
            Assert.NotNull(fn);

            IList<KeyValuePair<int, string>> col_Repeat = ColEx
                .Repeat(fn: fn, count: repeatCount);

            Assert.NotNull(col_Repeat);
            Assert.IsType<List<KeyValuePair<int, string>>>(col_Repeat);
            Assert.NotEmpty(col_Repeat);
            Assert.Equal(repeatCount, col_Repeat.Count);
            Assert.Equal(
                initialValue,
                col_Repeat.Where(x => x.Key == 1).Map(y => y.Value).First());
            Assert.Equal(
                1,
                col_Repeat.Where(x => x.Key == 1).Map(y => y.Key).First());
            Assert.Equal(
                initialValue,
                col_Repeat.Skip(repeatCount - 1).Map(x => x.Value).First());
            Assert.Equal(
                1,
                col_Repeat.Skip(repeatCount - 1).Map(x => x.Key).First());

            Func<KeyValuePair<int, string>> fn_single =
                () => mapDictionary.First(m => m.Key == 1);

            IList<KeyValuePair<int, string>> col_Repeat_Single = ColEx.Repeat(
                fn: fn_single, count: repeatCount);

            Assert.NotNull(col_Repeat_Single);
            Assert.IsType<List<KeyValuePair<int, string>>>(col_Repeat_Single);
            Assert.NotEmpty(col_Repeat_Single);
            Assert.Equal(repeatCount, col_Repeat_Single.Count);
            Assert.Equal("Initial Value", col_Repeat_Single.Skip(repeatCount - 1)
                .Select(m => m.Value).First().ToString());
            Assert.Equal("1", col_Repeat_Single.Skip(repeatCount - 1)
                .Select(m => m.Key).First().ToString());
        }

        /// <summary>
        /// A test for Repeat.
        ///</summary>
        [Fact]
        public void Col_Repeat_2()
        {
            Random random0 = RandomEx.Create(6533587);

            Assert.NotNull(random0);
            Assert.IsType<Random>(random0);

            IList<string> vector0 = ColEx.Repeat(
                () => random0.NextString(random0.Next(4, 32)), random0.Next(128, 256));

            Assert.NotNull(vector0);

            Random random1 = RandomEx.Create(6533587);

            Assert.NotNull(random1);
            Assert.IsType<Random>(random1);

            IList<string> vector1 = ColEx.CreateVector<string>();

            Assert.NotNull(vector1);

            int count = random1.Next(128, 256);

            Assert.True(count >= 128);
            Assert.True(count < 256);

            for (int i = 0; i < count; i++)
            {
                vector1.Add(random1.NextString(random1.Next(4, 32)));
            }

            vector0.Should().Equal(vector1);

            vector1.Clear();

            for (int i = 0; i < count; i++)
            {
                vector1.Add(random1.NextString(random1.Next(i, i + 4)));
            }

            vector0 = ColEx.Repeat(
                i => random0.NextString(random0.Next(i, i + 4)), count);

            vector0.Should().Equal(vector1);
        }

        /// <summary>
        /// A test for TryGetValue.
        ///</summary>
        [Fact]
        public void Col_TryGetValue()
        {
            IDictionary<int, string> mapDictionary = new Dictionary<int, string>()
            {
                {1, "FirstValue"},
            };

            Assert.NotNull(mapDictionary);
            Assert.NotEmpty(mapDictionary);

            int mapKey = mapDictionary.Keys.First();
            string mapExpectedValue = mapDictionary.Values.First();
            int mapKeyNotFound = 0;
            string mapKeyNotFoundExpectedValue = "Not found";
            string col_tryGetValue = ColEx.TryGetValue(
                map: mapDictionary, 
                key: mapKey, 
                value: mapExpectedValue);

            Assert.NotNull(col_tryGetValue);            
            Assert.IsType<string>(col_tryGetValue);
            Assert.NotEmpty(col_tryGetValue);
            Assert.Equal(mapExpectedValue, col_tryGetValue);

            string col_tryGetValue_EmptyValue = ColEx.TryGetValue(
                map: mapDictionary, 
                key: mapKey, 
                value: string.Empty);

            Assert.NotNull(col_tryGetValue_EmptyValue);            
            Assert.IsType<string>(col_tryGetValue_EmptyValue);
            Assert.NotEmpty(col_tryGetValue_EmptyValue);
            Assert.Equal(mapExpectedValue, col_tryGetValue_EmptyValue);

            string col_tryGetValue_NoKey = ColEx.TryGetValue(
                map: mapDictionary, 
                key: mapKeyNotFound,
                value: mapKeyNotFoundExpectedValue);

            Assert.NotNull(col_tryGetValue_NoKey);
            Assert.IsType<string>(col_tryGetValue_NoKey);
            Assert.NotEmpty(col_tryGetValue_NoKey);            
            Assert.Equal(mapKeyNotFoundExpectedValue, col_tryGetValue_NoKey);

            string col_tryGetValue_EmptyKey = ColEx.TryGetValue(
                map: mapDictionary, 
                key: 0, 
                value: mapKeyNotFoundExpectedValue);

            Assert.NotNull(col_tryGetValue_EmptyKey);
            Assert.IsType<string>(col_tryGetValue_EmptyKey);
            Assert.NotEmpty(col_tryGetValue_EmptyKey);            
            Assert.Equal(mapKeyNotFoundExpectedValue, col_tryGetValue_EmptyKey);
        }

        /// <summary>
        /// A test for UpdateOrAdd.
        ///</summary>
        [Fact]
        public void Col_UpdateOrAdd_1()
        {
            IDictionary<int, string> mapDictionary = new Dictionary<int, string>()
            {
                {1, "Initial Value"},
            };

            Assert.NotNull(mapDictionary);
            Assert.NotEmpty(mapDictionary);

            int initialKey = mapDictionary.Keys.First();
            string initialValue = mapDictionary.Values.First();
            Assert.Equal(initialValue, mapDictionary[1]);

            int addNewKey = 2;
            string addNewValue = "New Value";

            bool col_UpdateOrAdd_Update = ColEx.UpdateOrAdd(
                map: mapDictionary, 
                key: initialKey, 
                value: addNewValue);

            Assert.NotNull(col_UpdateOrAdd_Update);
            Assert.IsType<bool>(col_UpdateOrAdd_Update);
            Assert.Equal(true, col_UpdateOrAdd_Update);
            Assert.Equal(1, mapDictionary.Count());
            Assert.NotEqual(mapDictionary[1], initialValue);
            Assert.Equal(addNewValue, mapDictionary[1]);

            bool col_UpdateOrAdd_Add = ColEx.UpdateOrAdd(
                map: mapDictionary, 
                key: addNewKey, 
                value: addNewValue);

            Assert.NotNull(col_UpdateOrAdd_Add);
            Assert.IsType<bool>(col_UpdateOrAdd_Add);
            Assert.Equal(false, col_UpdateOrAdd_Add);
            Assert.Equal(2, mapDictionary.Count());
            Assert.Equal(addNewValue, mapDictionary[addNewKey]);

            // Reset the variables its initial state.
            mapDictionary = new Dictionary<int, string>() { { 1, "Initial Value" } };

            initialKey = mapDictionary.Keys.First();
            initialValue = mapDictionary.Values.First();
            addNewKey = 2;
            addNewValue = "New Value";
            string outOldValue = string.Empty;

            bool col_UpdateOrAdd_Update_Out = ColEx.UpdateOrAdd(
                map: mapDictionary, key: initialKey,
                value: addNewValue, 
                oldValue: out outOldValue);

            Assert.NotNull(col_UpdateOrAdd_Update_Out);
            Assert.IsType<bool>(col_UpdateOrAdd_Update_Out);
            Assert.Equal(true, col_UpdateOrAdd_Update_Out);
            Assert.Equal(1, mapDictionary.Count());
            Assert.NotEqual(mapDictionary[1], initialValue);
            Assert.Equal(addNewValue, mapDictionary[1]);
            Assert.Equal(initialValue, outOldValue);

            bool col_UpdateOrAdd_Add_Out = ColEx.UpdateOrAdd(
                map: mapDictionary, key: addNewKey,
                value: addNewValue, 
                oldValue: out outOldValue);

            Assert.NotNull(col_UpdateOrAdd_Add);
            Assert.IsType<bool>(col_UpdateOrAdd_Add);
            Assert.Equal(false, col_UpdateOrAdd_Add);
            Assert.Equal(2, mapDictionary.Count());
            Assert.Equal(addNewValue, mapDictionary[addNewKey]);
            Assert.Equal(default(string), outOldValue);

            //Reset the variables its initial state.
            mapDictionary = new Dictionary<int, string>() { { 1, "Initial Value" } };
            initialKey = mapDictionary.Keys.First();
            initialValue = mapDictionary.Values.First();
            addNewKey = 2;
            addNewValue = "New Value";

            bool col_UpdateOrAdd_Update_KVP = ColEx.UpdateOrAdd(
                map: mapDictionary, 
                kvp: ColEx.CreateKVP(initialKey, addNewValue));

            Assert.NotNull(col_UpdateOrAdd_Update_KVP);
            Assert.IsType<bool>(col_UpdateOrAdd_Update_KVP);
            Assert.Equal(true, col_UpdateOrAdd_Update_KVP);
            Assert.Equal(1, mapDictionary.Count());
            Assert.NotEqual(initialValue, mapDictionary[1]);
            Assert.Equal(addNewValue, mapDictionary[1]);

            bool col_UpdateOrAdd_Add_KVP = ColEx.UpdateOrAdd(
                map: mapDictionary, 
                kvp: ColEx.CreateKVP(addNewKey, addNewValue));

            Assert.NotNull(col_UpdateOrAdd_Add);
            Assert.IsType<bool>(col_UpdateOrAdd_Add);
            Assert.Equal(false, col_UpdateOrAdd_Add);
            Assert.Equal(2, mapDictionary.Count());
            Assert.Equal(addNewValue, mapDictionary[addNewKey]);
        }

        /// <summary>
        /// A test for UpdateOrAdd.
        ///</summary>
        [Fact]
        public void Col_UpdateOrAdd_2()
        {
            IDictionary<int, string> map = new Dictionary<int, string>();
            string s;
            IEnumerable<KeyValuePair<int, string>> seq = Seq.List(
                new KeyValuePair<int, string>(1, "Un"),
                new KeyValuePair<int, string>(4, "Four"),
                new KeyValuePair<int, string>(5, "Five"),
                new KeyValuePair<int, string>(3, "Trois"));

            Seq.List<object>(map, seq).Apply(o => Assert.NotNull(o));
            Assert.Empty(map);
            Assert.IsType<Dictionary<int, string>>(map);
            Assert.NotEmpty(seq);

            Assert.False(map.UpdateOrAdd(1, "One"));
            Assert.NotEmpty(map);
            Assert.True(map.UpdateOrAdd(1, "Uno"));

            Assert.False(map.UpdateOrAdd(2, "Two", out s));
            Assert.Null(s);

            Assert.True(map.UpdateOrAdd(2, "Dos", out s));
            Assert.Equal("Two", s);

            Assert.False(map.UpdateOrAdd(new KeyValuePair<int, string>(3, "Three")));
            Assert.True(map.UpdateOrAdd(new KeyValuePair<int, string>(3, "Tres")));

            map.UpdateOrAdd(seq).Should().Equal(Seq.List(true, false, false, true));
            map.Should().HaveCount(5);
            map.Keys.Should().Equal(Seq.List(1, 2, 3, 4, 5));
            map.Values.Should().Equal(Seq.List("Un", "Dos", "Trois", "Four", "Five"));
        }

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void Col_IsEmpty_1()
        {
            ICollection<string> colList = new List<string>()
            {
                {"First"}
            };

            Assert.NotNull(colList);
            Assert.NotEmpty(colList);
            Assert.Equal(1, colList.Count());

            bool col_IsNotEmpty = ColEx.IsEmpty(colList);

            Assert.NotNull(col_IsNotEmpty);
            Assert.IsType<bool>(col_IsNotEmpty);
            Assert.Equal(false, col_IsNotEmpty);

            ICollection<string> colEmptyList = new List<string>();

            Assert.NotNull(colEmptyList);
            Assert.Empty(colEmptyList);

            bool colIsEmpty = ColEx.IsEmpty(colEmptyList);

            Assert.NotNull(colIsEmpty);
            Assert.IsType<bool>(colIsEmpty);            
            Assert.Equal(true, colIsEmpty);
        }

        /// <summary>
        /// A test for IsEmpty.
        ///</summary>
        [Fact]
        public void Col_IsEmpty_2()
        {
            Assert.True(ColEx.IsEmpty(new Dictionary<int, string>()));
            Assert.True(ColEx.IsEmpty(new List<char>()));
            Assert.True(ColEx.IsEmpty(Array.Empty<short>()));
        }

        /// <summary>
        /// A test for IsNullOrEmpty.
        ///</summary>
        [Fact]
        public void Col_IsNullOrEmpty_1()
        {
            ICollection<string> colList = new List<string>()
            {
                {"First"}
            };

            Assert.NotNull(colList);
            Assert.NotEmpty(colList);

            bool col_IsNotNullOrEmpty = ColEx.IsNullOrEmpty(colList);

            Assert.NotNull(col_IsNotNullOrEmpty);
            Assert.IsType<bool>(col_IsNotNullOrEmpty);
            Assert.Equal(1, colList.Count());
            Assert.Equal(false, col_IsNotNullOrEmpty);

            ICollection<string> colEmpty = ColEx.CreateVector<string>();

            bool col_IsEmpty = ColEx.IsNullOrEmpty(colEmpty);

            Assert.NotNull(col_IsEmpty);
            Assert.IsType<bool>(col_IsEmpty);
            Assert.Equal(0, colEmpty.Count());
            Assert.Equal(true, col_IsEmpty);

            ICollection<string> colNull = null;
            bool col_IsNull = ColEx.IsNullOrEmpty(colNull);

            Assert.NotNull(col_IsNull);
            Assert.IsType<bool>(col_IsNull);
            Assert.Equal(null, colNull);
            Assert.Equal(true, col_IsNull);
        }

        /// <summary>
        /// A test for IsNullOrEmpty.
        ///</summary>
        [Fact]
        public void Col_IsNullOrEmpty_2()
        {
            ICollection<short> nullCol = null;
            IDictionary<int, string> nullMap = null;
            IList<char> nullVector = null;

            Assert.True(ColEx.IsNullOrEmpty(Array.Empty<short>()));
            Assert.True(ColEx.IsNullOrEmpty(nullCol));
            Assert.True(ColEx.IsNullOrEmpty(nullMap));
            Assert.True(ColEx.IsNullOrEmpty(new Dictionary<int, string>()));
            Assert.True(ColEx.IsNullOrEmpty(nullVector));
            Assert.True(ColEx.IsNullOrEmpty(new List<char>()));
        }

        /// <summary>
        /// A test for ToC5String.
        ///</summary>
        [Fact]
        public void Col_ToC5String_1()
        {
            IDictionary<string, string> mapDictionary = new Dictionary<string, string>()
            {
                {"FirstKey", "FirstValue"},
            };

            Assert.NotNull(mapDictionary);
            Assert.NotEmpty(mapDictionary);

            string col_ToC5String = ColEx.ToC5String(map: mapDictionary);

            Assert.NotNull(col_ToC5String);
            Assert.IsType<string>(col_ToC5String);
            Assert.NotEmpty(col_ToC5String);
            Assert.Equal(
                col_ToC5String,
                $"[{mapDictionary.Keys.First()} => {mapDictionary.Values.First()}]");

            mapDictionary.Clear();
            string col_ToC5StringIsEmpty = ColEx.ToC5String(map: mapDictionary);

            Assert.NotNull(col_ToC5StringIsEmpty);
            Assert.IsType<string>(col_ToC5StringIsEmpty);
            Assert.NotEmpty(col_ToC5StringIsEmpty);
            Assert.Equal(StringEx.EMPTY, col_ToC5StringIsEmpty);

            mapDictionary = null;
            string col_ToC5StringIsNull = ColEx.ToC5String(map: mapDictionary);

            Assert.NotNull(col_ToC5StringIsNull);
            Assert.IsType<string>(col_ToC5StringIsNull);
            Assert.NotEmpty(col_ToC5StringIsNull);
            Assert.Equal(StringEx.NULL, col_ToC5StringIsNull);
        }        

        /// <summary>
        /// A test for ToC5String.
        ///</summary>
        [Fact]
        public void Col_ToC5String_2()
        {
            IDictionary<byte, short> nullMap = null;
            IDictionary<string, int> map = ColEx.CreateMap<string, int>();

            Assert.Null(nullMap);
            Assert.NotNull(map);

            Assert.Equal(StringEx.NULL, nullMap.ToC5String());
            Assert.Equal(StringEx.EMPTY, map.ToC5String());

            map.Add("Shaquille", 34);

            Assert.Equal("[Shaquille => 34]", map.ToC5String());

            map.Add("McNair", 9);

            Assert.Equal("[Shaquille => 34|McNair => 9]", map.ToC5String());

            map.UpdateOrAdd("Shaquille", 32);

            Assert.Equal("[Shaquille => 32|McNair => 9]", map.ToC5String());

            map.UpdateOrAdd("Unitas", 19);

            Assert.Equal(
                "[Shaquille => 32|McNair => 9|Unitas => 19]",
                map.ToC5String());
        }

        #endregion
    }
}
