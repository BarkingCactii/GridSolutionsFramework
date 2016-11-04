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
    [ASN1Sequence(Name = "GetEventActionAttributes_Response", IsSet = false)]
    public class GetEventActionAttributes_Response : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(GetEventActionAttributes_Response));
        private Identifier accessControlList_;

        private bool accessControlList_present;
        private ConfirmedServiceRequest confirmedServiceRequest_;
        private Request_Detail cs_extension_;

        private bool cs_extension_present;
        private ICollection<Modifier> listOfModifier_;
        private bool mmsDeletable_;

        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "mmsDeletable", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = true)]
        public bool MmsDeletable
        {
            get
            {
                return mmsDeletable_;
            }
            set
            {
                mmsDeletable_ = value;
            }
        }


        [ASN1SequenceOf(Name = "listOfModifier", IsSetOf = false)]
        [ASN1Element(Name = "listOfModifier", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ICollection<Modifier> ListOfModifier
        {
            get
            {
                return listOfModifier_;
            }
            set
            {
                listOfModifier_ = value;
            }
        }


        [ASN1Element(Name = "confirmedServiceRequest", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public ConfirmedServiceRequest ConfirmedServiceRequest
        {
            get
            {
                return confirmedServiceRequest_;
            }
            set
            {
                confirmedServiceRequest_ = value;
            }
        }


        [ASN1Element(Name = "cs-extension", IsOptional = true, HasTag = true, Tag = 79, HasDefaultValue = false)]
        public Request_Detail Cs_extension
        {
            get
            {
                return cs_extension_;
            }
            set
            {
                cs_extension_ = value;
                cs_extension_present = true;
            }
        }


        [ASN1Element(Name = "accessControlList", IsOptional = true, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public Identifier AccessControlList
        {
            get
            {
                return accessControlList_;
            }
            set
            {
                accessControlList_ = value;
                accessControlList_present = true;
            }
        }


        public void initWithDefaults()
        {
            bool param_MmsDeletable =
                false;
            MmsDeletable = param_MmsDeletable;
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }

        public bool isCs_extensionPresent()
        {
            return cs_extension_present;
        }

        public bool isAccessControlListPresent()
        {
            return accessControlList_present;
        }
    }
}