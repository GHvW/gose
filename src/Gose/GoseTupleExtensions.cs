using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gose {

    public static class GoseTupleExtensions {

        public static (Err? Error, Result? Data) Select<Err, Data, Result>(
            this (Err? Error, Data? Data) @this, 
            Func<Data, Result> fn) =>
            @this switch {
                (null, Data d) => (default, fn(d)),
                _ => (@this.Error, default)
            };


        public static (Err? Error, Result? Data) SelectMany<Err, Data, Result>(
            this (Err? Error, Data? Data) @this, 
            Func<Data, (Err?, Result?)> fn) =>
            @this switch {
                (null, Data d) => fn(d),
                _ => (@this.Error, default)
            };


        public static (Err? Error, Select? Data) SelectMany<Err, Data, Result, Select>(
            this (Err? Error, Data? Data) @this,
            Func<Data, (Err?, Result?)> fn,
            Func<Data, Result, Select> selector) =>
            @this switch {
                (null, Data d) => fn(d).Select(data => selector(d, data)),
                _ => (@this.Error, default)
            };

        
        public static (Result? Error, Data? Data) SelectErr<Err, Data, Result>(
            this (Err? Error, Data? Data) @this,
            Func<Err, Result> fn) =>
            @this switch {
                (Err e, null) => (fn(e), default),
                _ => (default, @this.Data)
            };
    }
}
