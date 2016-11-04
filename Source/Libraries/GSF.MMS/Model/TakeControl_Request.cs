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
    [ASN1Sequence(Name = "TakeControl_Request", IsSet = false)]
    public class TakeControl_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TakeControl_Request));
        private bool abortOnTimeOut_;

        private bool abortOnTimeOut_present;
        private Unsigned32 acceptableDelay_;

        private bool acceptableDelay_present;
        private ApplicationReference applicationToPreempt_;

        private bool applicationToPreempt_present;
        private Unsigned32 controlTimeOut_;

        private bool controlTimeOut_present;
        private Identifier namedToken_;

        private bool namedToken_present;


        private Priority priority_;
        private bool relinquishIfConnectionLost_;
        private ObjectName semaphoreName_;

        [ASN1Element(Name = "semaphoreName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ObjectName SemaphoreName
        {
            get
            {
                return semaphoreName_;
            }
            set
            {
                semaphoreName_ = value;
            }
        }

        [ASN1Element(Name = "namedToken", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public Identifier NamedToken
        {
            get
            {
                return namedToken_;
            }
            set
            {
                namedToken_ = value;
                namedToken_present = true;
            }
        }

        [ASN1Element(Name = "priority", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = true)]
        public Priority Priority
        {
            get
            {
                return priority_;
            }
            set
            {
                priority_ = value;
            }
        }


        [ASN1Element(Name = "acceptableDelay", IsOptional = true, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public Unsigned32 AcceptableDelay
        {
            get
            {
                return acceptableDelay_;
            }
            set
            {
                acceptableDelay_ = value;
                acceptableDelay_present = true;
            }
        }


        [ASN1Element(Name = "controlTimeOut", IsOptional = true, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public Unsigned32 ControlTimeOut
        {
            get
            {
                return controlTimeOut_;
            }
            set
            {
                controlTimeOut_ = value;
                controlTimeOut_present = true;
            }
        }


        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "abortOnTimeOut", IsOptional = true, HasTag = true, Tag = 5, HasDefaultValue = false)]
        public bool AbortOnTimeOut
        {
            get
            {
                return abortOnTimeOut_;
            }
            set
            {
                abortOnTimeOut_ = value;
                abortOnTimeOut_present = true;
            }
        }


        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "relinquishIfConnectionLost", IsOptional = false, HasTag = true, Tag = 6, HasDefaultValue = true)]
        public bool RelinquishIfConnectionLost
        {
            get
            {
                return relinquishIfConnectionLost_;
            }
            set
            {
                relinquishIfConnectionLost_ = value;
            }
        }


        [ASN1Element(Name = "applicationToPreempt", IsOptional = true, HasTag = true, Tag = 7, HasDefaultValue = false)]
        public ApplicationReference ApplicationToPreempt
        {
            get
            {
                return applicationToPreempt_;
            }
            set
            {
                applicationToPreempt_ = value;
                applicationToPreempt_present = true;
            }
        }

        public void initWithDefaults()
        {
            Priority param_Priority =
                new Priority(64);
            Priority = param_Priority;
            bool param_RelinquishIfConnectionLost =
                false;
            RelinquishIfConnectionLost = param_RelinquishIfConnectionLost;
        }

        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }


        public bool isNamedTokenPresent()
        {
            return namedToken_present;
        }

        public bool isAcceptableDelayPresent()
        {
            return acceptableDelay_present;
        }

        public bool isControlTimeOutPresent()
        {
            return controlTimeOut_present;
        }

        public bool isAbortOnTimeOutPresent()
        {
            return abortOnTimeOut_present;
        }

        public bool isApplicationToPreemptPresent()
        {
            return applicationToPreempt_present;
        }
    }
}