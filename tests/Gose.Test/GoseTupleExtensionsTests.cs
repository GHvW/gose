using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Gose.Test {

    public class GoseTupleExtensionsTests {

        [Fact]
        public void SecondFail() {
            (string? Error, int? Data) first = (null, 10);
            (string? Error, int? Data) second = ("divide by zero", null);

            var (error, data) =
                (from x in first
                 from y in second
                 select x + y);

            Assert.Equal("divide by zero", error);
            Assert.Null(data);
        }

        [Fact]
        public void FirstFail() {
            (string? Error, int? Data) first = ("no data available", null);
            (string? Error, int? Data) second = (null, 20);

            var (error, data) =
                (from x in first
                 from y in second
                 select x + y);

            Assert.Equal("no data available", error);
            Assert.Null(data);
        }

        [Fact]
        public void Success() {
            (string? Error, int? Data) first = (null, 10);
            (string? Error, int? Data) second = (null, 20);

            var (error, data) =
                (from x in first
                 from y in second
                 select x + y);

            Assert.Equal(30, data);
            Assert.Null(error);
        }

        [Fact]
        public void SelectErr() {
            (int? Error, int? Data) result = (5, null);

            var (error, data) = result.SelectErr(err => $"Error #{err} returned");

            Assert.Equal("Error #5 returned", error);
            Assert.Null(data);
        }
    }
}
