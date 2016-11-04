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
    [ASN1Sequence(Name = "EntryContent", IsSet = false)]
    public class EntryContent : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(EntryContent));
        private EntryFormChoiceType entryForm_;
        private TimeOfDay occurrenceTime_;

        [ASN1Element(Name = "occurrenceTime", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public TimeOfDay OccurrenceTime
        {
            get
            {
                return occurrenceTime_;
            }
            set
            {
                occurrenceTime_ = value;
            }
        }


        [ASN1Element(Name = "entryForm", IsOptional = false, HasTag = false, HasDefaultValue = false)]
        public EntryFormChoiceType EntryForm
        {
            get
            {
                return entryForm_;
            }
            set
            {
                entryForm_ = value;
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
        [ASN1Choice(Name = "entryForm")]
        public class EntryFormChoiceType : IASN1PreparedElement
        {
            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(EntryFormChoiceType));
            private MMSString annotation_;
            private bool annotation_selected;
            private DataSequenceType data_;
            private bool data_selected;


            [ASN1Element(Name = "data", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
            public DataSequenceType Data
            {
                get
                {
                    return data_;
                }
                set
                {
                    selectData(value);
                }
            }


            [ASN1Element(Name = "annotation", IsOptional = false, HasTag = true, Tag = 3, HasDefaultValue = false)]
            public MMSString Annotation
            {
                get
                {
                    return annotation_;
                }
                set
                {
                    selectAnnotation(value);
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


            public bool isDataSelected()
            {
                return data_selected;
            }


            public void selectData(DataSequenceType val)
            {
                data_ = val;
                data_selected = true;


                annotation_selected = false;
            }


            public bool isAnnotationSelected()
            {
                return annotation_selected;
            }


            public void selectAnnotation(MMSString val)
            {
                annotation_ = val;
                annotation_selected = true;


                data_selected = false;
            }

            [ASN1PreparedElement]
            [ASN1Sequence(Name = "data", IsSet = false)]
            public class DataSequenceType : IASN1PreparedElement
            {
                private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DataSequenceType));
                private EventSequenceType event_;

                private bool event_present;


                private ICollection<Journal_Variable> listOfVariables_;

                private bool listOfVariables_present;

                [ASN1Element(Name = "event", IsOptional = true, HasTag = true, Tag = 0, HasDefaultValue = false)]
                public EventSequenceType Event
                {
                    get
                    {
                        return event_;
                    }
                    set
                    {
                        event_ = value;
                        event_present = true;
                    }
                }

                [ASN1SequenceOf(Name = "listOfVariables", IsSetOf = false)]
                [ASN1Element(Name = "listOfVariables", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
                public ICollection<Journal_Variable> ListOfVariables
                {
                    get
                    {
                        return listOfVariables_;
                    }
                    set
                    {
                        listOfVariables_ = value;
                        listOfVariables_present = true;
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

                public bool isEventPresent()
                {
                    return event_present;
                }

                public bool isListOfVariablesPresent()
                {
                    return listOfVariables_present;
                }

                [ASN1PreparedElement]
                [ASN1Sequence(Name = "event", IsSet = false)]
                public class EventSequenceType : IASN1PreparedElement
                {
                    private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(EventSequenceType));
                    private EC_State currentState_;
                    private ObjectName eventConditionName_;

                    [ASN1Element(Name = "eventConditionName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
                    public ObjectName EventConditionName
                    {
                        get
                        {
                            return eventConditionName_;
                        }
                        set
                        {
                            eventConditionName_ = value;
                        }
                    }


                    [ASN1Element(Name = "currentState", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
                    public EC_State CurrentState
                    {
                        get
                        {
                            return currentState_;
                        }
                        set
                        {
                            currentState_ = value;
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
        }
    }
}