using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Services
{
    public class DateTimeConvertService
    {
        public double? ToJavascriptDate(DateTimeOffset? source)
        {
            if (!source.HasValue) { return null; }

            var sourceValue = source.Value;
            return sourceValue.Subtract(Basis).TotalMilliseconds;
        }

        public DateTimeOffset? ToDateTimeOffset(long? source)
        {
            if (!source.HasValue)
            {
                return null;
            }

            return Basis.AddMilliseconds(source.Value);
        }

        public readonly DateTimeOffset Basis = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
