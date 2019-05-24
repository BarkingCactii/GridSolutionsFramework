using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSF.PhasorProtocols.IEC61850_90_5_Goose
{
    public class TimeLengthValue
    {
        public DataType Type { get; set; }
        public MeasurementType MeasurementType { get; set; }
        public int Length { get; set; }
        public object Value { get; set; }
    }
}
