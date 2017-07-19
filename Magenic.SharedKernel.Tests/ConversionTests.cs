using System;
using System.Collections.Generic;

using FluentAssertions;

using Xunit;

namespace Magenic.SharedKernel.Tests
{
    /// <summary>
    /// Unit tests around data type conversion.
    /// </summary>
    public class ConversionTests 
    {
        #region Public Methods

        /// <summary>
        /// Verifies a string CSV of enum names can be converted to a list of enum values.
        /// </summary>
        [Fact]
        public void Can_Convert_Enum_String_Names_CSV_To_List()
        {
            IList<DayOfWeek> items = Conversion.ToEnumList<DayOfWeek>(
                "Monday,Wednesday", ",");

            items.Should().BeEquivalentTo(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
        }

        /// <summary>
        /// Verifies a string CSV of enum values can be converted to a list of enum values.
        /// </summary>
        [Fact]
        public void Can_Convert_Enum_String_Values_CSV_To_List()
        {
            IList<DayOfWeek> items = Conversion.ToEnumList<DayOfWeek>(
                "0,2", ",");

            items.Should().BeEquivalentTo(new[] { DayOfWeek.Sunday, DayOfWeek.Tuesday });
        }

        /// <summary>
        /// Verifies a string array of enum names can be converted to a list of enum values.
        /// </summary>
        [Fact]
        public void Can_Convert_Enum_String_Names_Array_To_List()
        {
            IList<DayOfWeek> items =  Conversion.ToEnumList<DayOfWeek>(
                new[] { "Monday", "Tuesday" });

            items.Should().BeEquivalentTo(new[] { DayOfWeek.Monday, DayOfWeek.Tuesday });
        }

        /// <summary>
        /// Verifies a string array of enum values can be converted to a list of enum values.
        /// </summary>
        [Fact]
        public void Can_Convert_Enum_String_Values_Array_To_List()
        {
            IList<DayOfWeek> items = Conversion.ToEnumList<DayOfWeek>(new[] { "4", "5" });

            items.Should().BeEquivalentTo(new[] { DayOfWeek.Thursday, DayOfWeek.Friday });
        }

        [Fact]
        public void Can_Convert_Enum_String_Name_To_Enum()
        {
            DayOfWeek result = Conversion.ToEnum<DayOfWeek>("Tuesday");

            result.Should().Be(DayOfWeek.Tuesday);
        }

        [Fact]
        public void Can_Convert_Enum_Numeric_Value_To_Enum()
        {
            Conversion.ToEnum<DayOfWeek>(1).Should().Be(DayOfWeek.Monday);
        }

        [Fact]
        public void Enum_Conversion_With_Invalid_Value_Throws()
        {
            Assert.Throws<KeyNotFoundException>(() => Conversion.ToEnum<DayOfWeek>(166));
            Assert.Throws<KeyNotFoundException>(
                () => Conversion.ToEnum<DayOfWeek>(DateTime.Now.Ticks.ToString()));
        }

        #endregion
    }
}
