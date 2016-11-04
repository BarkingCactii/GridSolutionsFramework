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
    [ASN1Sequence(Name = "Service_and_Parameter_CBBs", IsSet = false)]
    public class Service_and_Parameter_CBBs : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(Service_and_Parameter_CBBs));
        private DataParameters dataParameters_;
        private DomainManagementParameters domainManagement_;
        private ErrorParameters errors_;
        private AdditionalCBBOptions extendedParameters_;
        private AdditionalSupportOptions extendedServices_Client_;
        private AdditionalSupportOptions extendedServices_Server_;
        private FileManagementParameters fileManagement_;
        private GeneralManagementParameters generalManagement_;
        private long nest_;
        private OperatorCommunicationParameters operatorCommunication_;
        private ParameterSupportOptions parameters_;
        private ProgramInvocationManagementParameters programInvocation_;
        private SemaphoreManagementParameters semaphoreManagement_;
        private ServiceSupportOptions services_Client_;


        private ServiceSupportOptions services_Server_;
        private VMDSupportParameters vMDSupport_;
        private VariableAccessParameters variableAccess_;

        [ASN1Element(Name = "services-Client", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ServiceSupportOptions Services_Client
        {
            get
            {
                return services_Client_;
            }
            set
            {
                services_Client_ = value;
            }
        }

        [ASN1Element(Name = "services-Server", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ServiceSupportOptions Services_Server
        {
            get
            {
                return services_Server_;
            }
            set
            {
                services_Server_ = value;
            }
        }


        [ASN1Element(Name = "parameters", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public ParameterSupportOptions Parameters
        {
            get
            {
                return parameters_;
            }
            set
            {
                parameters_ = value;
            }
        }


        [ASN1Integer(Name = "")]
        [ASN1Element(Name = "nest", IsOptional = false, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public long Nest
        {
            get
            {
                return nest_;
            }
            set
            {
                nest_ = value;
            }
        }


        [ASN1Element(Name = "extendedServices-Client", IsOptional = false, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public AdditionalSupportOptions ExtendedServices_Client
        {
            get
            {
                return extendedServices_Client_;
            }
            set
            {
                extendedServices_Client_ = value;
            }
        }


        [ASN1Element(Name = "extendedServices-Server", IsOptional = false, HasTag = true, Tag = 5, HasDefaultValue = false)]
        public AdditionalSupportOptions ExtendedServices_Server
        {
            get
            {
                return extendedServices_Server_;
            }
            set
            {
                extendedServices_Server_ = value;
            }
        }


        [ASN1Element(Name = "extendedParameters", IsOptional = false, HasTag = true, Tag = 6, HasDefaultValue = false)]
        public AdditionalCBBOptions ExtendedParameters
        {
            get
            {
                return extendedParameters_;
            }
            set
            {
                extendedParameters_ = value;
            }
        }


        [ASN1Element(Name = "generalManagement", IsOptional = false, HasTag = true, Tag = 7, HasDefaultValue = false)]
        public GeneralManagementParameters GeneralManagement
        {
            get
            {
                return generalManagement_;
            }
            set
            {
                generalManagement_ = value;
            }
        }


        [ASN1Element(Name = "vMDSupport", IsOptional = false, HasTag = true, Tag = 8, HasDefaultValue = false)]
        public VMDSupportParameters VMDSupport
        {
            get
            {
                return vMDSupport_;
            }
            set
            {
                vMDSupport_ = value;
            }
        }


        [ASN1Element(Name = "domainManagement", IsOptional = false, HasTag = true, Tag = 9, HasDefaultValue = false)]
        public DomainManagementParameters DomainManagement
        {
            get
            {
                return domainManagement_;
            }
            set
            {
                domainManagement_ = value;
            }
        }


        [ASN1Element(Name = "programInvocation", IsOptional = false, HasTag = true, Tag = 10, HasDefaultValue = false)]
        public ProgramInvocationManagementParameters ProgramInvocation
        {
            get
            {
                return programInvocation_;
            }
            set
            {
                programInvocation_ = value;
            }
        }


        [ASN1Element(Name = "variableAccess", IsOptional = false, HasTag = true, Tag = 11, HasDefaultValue = false)]
        public VariableAccessParameters VariableAccess
        {
            get
            {
                return variableAccess_;
            }
            set
            {
                variableAccess_ = value;
            }
        }


        [ASN1Element(Name = "dataParameters", IsOptional = false, HasTag = true, Tag = 12, HasDefaultValue = false)]
        public DataParameters DataParameters
        {
            get
            {
                return dataParameters_;
            }
            set
            {
                dataParameters_ = value;
            }
        }


        [ASN1Element(Name = "semaphoreManagement", IsOptional = false, HasTag = true, Tag = 13, HasDefaultValue = false)]
        public SemaphoreManagementParameters SemaphoreManagement
        {
            get
            {
                return semaphoreManagement_;
            }
            set
            {
                semaphoreManagement_ = value;
            }
        }


        [ASN1Element(Name = "operatorCommunication", IsOptional = false, HasTag = true, Tag = 14, HasDefaultValue = false)]
        public OperatorCommunicationParameters OperatorCommunication
        {
            get
            {
                return operatorCommunication_;
            }
            set
            {
                operatorCommunication_ = value;
            }
        }


        [ASN1Element(Name = "errors", IsOptional = false, HasTag = true, Tag = 15, HasDefaultValue = false)]
        public ErrorParameters Errors
        {
            get
            {
                return errors_;
            }
            set
            {
                errors_ = value;
            }
        }


        [ASN1Element(Name = "fileManagement", IsOptional = false, HasTag = true, Tag = 16, HasDefaultValue = false)]
        public FileManagementParameters FileManagement
        {
            get
            {
                return fileManagement_;
            }
            set
            {
                fileManagement_ = value;
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