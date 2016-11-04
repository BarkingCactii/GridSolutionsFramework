//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1BoxedType(Name = "LogicalStatus")]
    public class LogicalStatus : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(LogicalStatus));
        private long val;

        [ASN1Integer(Name = "")]
        [ASN1Element(Name = "LogicalStatus", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public long Value
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }


        public void initWithDefaults()
        {
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }
    }
}