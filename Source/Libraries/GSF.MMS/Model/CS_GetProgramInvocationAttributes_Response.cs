//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System.Collections.Generic;
using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;
using GSF.ASN1.Types;

namespace GSF.MMS.Model
{
    
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "CS_GetProgramInvocationAttributes_Response", IsSet = false)]
    public class CS_GetProgramInvocationAttributes_Response : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(CS_GetProgramInvocationAttributes_Response));
        private ControlChoiceType control_;
        private long errorCode_;

        [ASN1Integer(Name = "")]
        [ASN1Element(Name = "errorCode", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public long ErrorCode
        {
            get
            {
                return errorCode_;
            }
            set
            {
                errorCode_ = value;
            }
        }


        [ASN1Element(Name = "control", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ControlChoiceType Control
        {
            get
            {
                return control_;
            }
            set
            {
                control_ = value;
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

        [ASN1PreparedElement]
        [ASN1Choice(Name = "control")]
        public class ControlChoiceType : IASN1PreparedElement
        {
            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ControlChoiceType));
            private ControlledChoiceType controlled_;
            private bool controlled_selected;
            private ControllingSequenceType controlling_;
            private bool controlling_selected;


            private NullObject normal_;
            private bool normal_selected;

            [ASN1Element(Name = "controlling", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
            public ControllingSequenceType Controlling
            {
                get
                {
                    return controlling_;
                }
                set
                {
                    selectControlling(value);
                }
            }

            [ASN1Element(Name = "controlled", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
            public ControlledChoiceType Controlled
            {
                get
                {
                    return controlled_;
                }
                set
                {
                    selectControlled(value);
                }
            }


            [ASN1Null(Name = "normal")]
            [ASN1Element(Name = "normal", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
            public NullObject Normal
            {
                get
                {
                    return normal_;
                }
                set
                {
                    selectNormal(value);
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


            public bool isControllingSelected()
            {
                return controlling_selected;
            }


            public void selectControlling(ControllingSequenceType val)
            {
                controlling_ = val;
                controlling_selected = true;


                controlled_selected = false;

                normal_selected = false;
            }


            public bool isControlledSelected()
            {
                return controlled_selected;
            }


            public void selectControlled(ControlledChoiceType val)
            {
                controlled_ = val;
                controlled_selected = true;


                controlling_selected = false;

                normal_selected = false;
            }


            public bool isNormalSelected()
            {
                return normal_selected;
            }


            public void selectNormal()
            {
                selectNormal(new NullObject());
            }


            public void selectNormal(NullObject val)
            {
                normal_ = val;
                normal_selected = true;


                controlling_selected = false;

                controlled_selected = false;
            }

            [ASN1PreparedElement]
            [ASN1Choice(Name = "controlled")]
            public class ControlledChoiceType : IASN1PreparedElement
            {
                private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ControlledChoiceType));
                private Identifier controllingPI_;
                private bool controllingPI_selected;


                private NullObject none_;
                private bool none_selected;

                [ASN1Element(Name = "controllingPI", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
                public Identifier ControllingPI
                {
                    get
                    {
                        return controllingPI_;
                    }
                    set
                    {
                        selectControllingPI(value);
                    }
                }


                [ASN1Null(Name = "none")]
                [ASN1Element(Name = "none", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
                public NullObject None
                {
                    get
                    {
                        return none_;
                    }
                    set
                    {
                        selectNone(value);
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


                public bool isControllingPISelected()
                {
                    return controllingPI_selected;
                }


                public void selectControllingPI(Identifier val)
                {
                    controllingPI_ = val;
                    controllingPI_selected = true;


                    none_selected = false;
                }


                public bool isNoneSelected()
                {
                    return none_selected;
                }


                public void selectNone()
                {
                    selectNone(new NullObject());
                }


                public void selectNone(NullObject val)
                {
                    none_ = val;
                    none_selected = true;


                    controllingPI_selected = false;
                }
            }

            [ASN1PreparedElement]
            [ASN1Sequence(Name = "controlling", IsSet = false)]
            public class ControllingSequenceType : IASN1PreparedElement
            {
                private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ControllingSequenceType));
                private ICollection<Identifier> controlledPI_;


                private string programLocation_;

                private bool programLocation_present;


                private RunningModeChoiceType runningMode_;

                [ASN1SequenceOf(Name = "controlledPI", IsSetOf = false)]
                [ASN1Element(Name = "controlledPI", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
                public ICollection<Identifier> ControlledPI
                {
                    get
                    {
                        return controlledPI_;
                    }
                    set
                    {
                        controlledPI_ = value;
                    }
                }

                [ASN1String(Name = "",
                    StringType = UniversalTags.VisibleString, IsUCS = false)]
                [ASN1Element(Name = "programLocation", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
                public string ProgramLocation
                {
                    get
                    {
                        return programLocation_;
                    }
                    set
                    {
                        programLocation_ = value;
                        programLocation_present = true;
                    }
                }


                [ASN1Element(Name = "runningMode", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
                public RunningModeChoiceType RunningMode
                {
                    get
                    {
                        return runningMode_;
                    }
                    set
                    {
                        runningMode_ = value;
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

                public bool isProgramLocationPresent()
                {
                    return programLocation_present;
                }

                [ASN1PreparedElement]
                [ASN1Choice(Name = "runningMode")]
                public class RunningModeChoiceType : IASN1PreparedElement
                {
                    private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(RunningModeChoiceType));
                    private long cycleLimited_;
                    private bool cycleLimited_selected;
                    private NullObject freeRunning_;
                    private bool freeRunning_selected;


                    private long stepLimited_;
                    private bool stepLimited_selected;

                    [ASN1Null(Name = "freeRunning")]
                    [ASN1Element(Name = "freeRunning", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
                    public NullObject FreeRunning
                    {
                        get
                        {
                            return freeRunning_;
                        }
                        set
                        {
                            selectFreeRunning(value);
                        }
                    }

                    [ASN1Integer(Name = "")]
                    [ASN1Element(Name = "cycleLimited", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
                    public long CycleLimited
                    {
                        get
                        {
                            return cycleLimited_;
                        }
                        set
                        {
                            selectCycleLimited(value);
                        }
                    }


                    [ASN1Integer(Name = "")]
                    [ASN1Element(Name = "stepLimited", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
                    public long StepLimited
                    {
                        get
                        {
                            return stepLimited_;
                        }
                        set
                        {
                            selectStepLimited(value);
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


                    public bool isFreeRunningSelected()
                    {
                        return freeRunning_selected;
                    }


                    public void selectFreeRunning()
                    {
                        selectFreeRunning(new NullObject());
                    }


                    public void selectFreeRunning(NullObject val)
                    {
                        freeRunning_ = val;
                        freeRunning_selected = true;


                        cycleLimited_selected = false;

                        stepLimited_selected = false;
                    }


                    public bool isCycleLimitedSelected()
                    {
                        return cycleLimited_selected;
                    }


                    public void selectCycleLimited(long val)
                    {
                        cycleLimited_ = val;
                        cycleLimited_selected = true;


                        freeRunning_selected = false;

                        stepLimited_selected = false;
                    }


                    public bool isStepLimitedSelected()
                    {
                        return stepLimited_selected;
                    }


                    public void selectStepLimited(long val)
                    {
                        stepLimited_ = val;
                        stepLimited_selected = true;


                        freeRunning_selected = false;

                        cycleLimited_selected = false;
                    }
                }
            }
        }
    }
}