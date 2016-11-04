//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System.Collections.Generic;
using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "DeleteNamedType_Request", IsSet = false)]
    public class DeleteNamedType_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DeleteNamedType_Request));
        private Identifier domainName_;

        private bool domainName_present;
        private ICollection<ObjectName> listOfTypeName_;

        private bool listOfTypeName_present;
        private long scopeOfDelete_;

        [ASN1Integer(Name = "")]
        [ASN1Element(Name = "scopeOfDelete", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = true)]
        public long ScopeOfDelete
        {
            get
            {
                return scopeOfDelete_;
            }
            set
            {
                scopeOfDelete_ = value;
            }
        }

        [ASN1SequenceOf(Name = "listOfTypeName", IsSetOf = false)]
        [ASN1Element(Name = "listOfTypeName", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ICollection<ObjectName> ListOfTypeName
        {
            get
            {
                return listOfTypeName_;
            }
            set
            {
                listOfTypeName_ = value;
                listOfTypeName_present = true;
            }
        }


        [ASN1Element(Name = "domainName", IsOptional = true, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public Identifier DomainName
        {
            get
            {
                return domainName_;
            }
            set
            {
                domainName_ = value;
                domainName_present = true;
            }
        }


        public void initWithDefaults()
        {
            long param_ScopeOfDelete =
                0;
            ScopeOfDelete = param_ScopeOfDelete;
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }

        public bool isListOfTypeNamePresent()
        {
            return listOfTypeName_present;
        }

        public bool isDomainNamePresent()
        {
            return domainName_present;
        }
    }
}