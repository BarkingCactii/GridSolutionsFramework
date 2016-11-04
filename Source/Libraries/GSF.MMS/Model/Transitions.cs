//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Attributes.Constraints;
using GSF.ASN1.Coders;
using GSF.ASN1.Types;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1BoxedType(Name = "Transitions")]
    public class Transitions : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(Transitions));
        private BitString val;

        public Transitions()
        {
        }

        public Transitions(BitString value)
        {
            Value = value;
        }

        [ASN1BitString(Name = "Transitions")]
        [ASN1SizeConstraint(Max = 7L)]
        public BitString Value
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