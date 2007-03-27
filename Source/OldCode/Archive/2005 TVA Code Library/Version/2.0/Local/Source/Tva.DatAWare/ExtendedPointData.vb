' 02/24/2007

Imports TVA.Interop.Bit

Public Class ExtendedPointData
    Inherits PointDataBase

    Private Const TimeZoneIndexMask As Int32 = Bit5 Or Bit6 Or Bit7 Or Bit8 Or Bit9 Or Bit10
    Private Const DaylightSavingsTimeMask As Int32 = Bit11

    Public Shadows Const Size As Integer = 16

    Public Sub New()

        MyBase.New()

    End Sub

    Public Sub New(ByVal binaryImage As Byte())

        MyClass.New(binaryImage, 0)

    End Sub

    Public Sub New(ByVal binaryImage As Byte(), ByVal startIndex As Integer)

        MyBase.New()
        If binaryImage IsNot Nothing Then
            If binaryImage.Length - startIndex >= Size Then
                TimeTag = New TimeTag(BitConverter.ToDouble(binaryImage, startIndex))
                Flags = BitConverter.ToInt32(binaryImage, startIndex + 8)
                Value = BitConverter.ToSingle(binaryImage, startIndex + 12)
            Else
                Throw New ArgumentException("Binary image size from startIndex is too small.")
            End If
        Else
            Throw New ArgumentNullException("binaryImage")
        End If

    End Sub

    Public Sub New(ByVal timeTag As TimeTag, ByVal value As Single, ByVal quality As Quality)

        MyBase.New(timeTag, value, quality)

    End Sub

    Public Sub New(ByVal timeTag As TimeTag, ByVal value As Single, ByVal quality As Quality, ByVal definition As PointDefinition)

        MyBase.New(timeTag, value, quality, definition)

    End Sub

    Public Property TimeZoneIndex() As Short
        Get
            Return Convert.ToInt16((Flags And TimeZoneIndexMask) \ 32)
        End Get
        Set(ByVal value As Short)
            Flags = (Flags And Not TimeZoneIndexMask Or (value * 32))
        End Set
    End Property

    Public Property DaylightSavingsTime() As Boolean
        Get
            Return Convert.ToBoolean(Flags And DaylightSavingsTimeMask)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Flags = (Flags Or DaylightSavingsTimeMask)
            Else
                Flags = (Flags And Not DaylightSavingsTimeMask)
            End If
        End Set
    End Property

    Public Overrides ReadOnly Property BinaryImage() As Byte()
        Get
            Dim data As Byte() = CreateArray(Of Byte)(Size)

            Array.Copy(BitConverter.GetBytes(TimeTag.Value), 0, data, 0, 8)
            Array.Copy(BitConverter.GetBytes(Flags), 0, data, 8, 4)
            Array.Copy(BitConverter.GetBytes(Value), 0, data, 12, 4)

            Return data
        End Get
    End Property

    Public Overrides ReadOnly Property BinaryLength() As Integer
        Get
            Return Size
        End Get
    End Property

    Public Function ToStandard() As StandardPointData

        Return New StandardPointData(TimeTag, Value, Quality, Definition)

    End Function

End Class

#Region " Old Code "

''*******************************************************************************************************
''  PointData.vb - Most basic data element in DatAWare
''  Copyright � 2006 - TVA, all rights reserved - Gbtc
''
''  Build Environment: VB.NET, Visual Studio 2005
''  Primary Developer: J. Ritchie Carroll, Operations Data Architecture [TVA]
''      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
''       Phone: 423/751-2827
''       Email: jrcarrol@tva.gov
''
''  Code Modification History:
''  -----------------------------------------------------------------------------------------------------
''  05/03/2006 - J. Ritchie Carroll
''       Initial version of source imported from 1.1 code library
''
''*******************************************************************************************************

'Imports TVA.Interop.Bit

'' This is the most basic form of a point of data in DatAWare (used by ReadEvent and ReadRange)
'<Serializable()> _
'Public Class ExtendedPointData
'    Implements IComparable

'#Region " Member Declaration "

'    ' *******************************************************************************
'    ' *                             Bit usage for *Flags*                           *
'    ' *******************************************************************************
'    ' * Bits    Mask    Description                                                 *
'    ' * ----    ----    -----------                                                 *
'    ' * 0-4     &H1F    Data quality indicator, a number between 0 and 31.          *
'    ' *                 Maps to the same qualities as used by PMS process computer. *
'    ' * 5-10    &H7E0   Index of time-zone used, number between 0 and 51            *
'    ' * 11      &H800   DST indicator. When set, DST else Standard Time.            *
'    ' *******************************************************************************

'    Private m_tTag As TimeTag
'    Private m_value As Single
'    Private m_flags As Int32
'    Private m_definition As PointDefinition

'    Private Const QualityMask As Int32 = Bit0 Or Bit1 Or Bit2 Or Bit3 Or Bit4
'    Private Const TimeZoneIndexMask As Int32 = Bit5 Or Bit6 Or Bit7 Or Bit8 Or Bit9 Or Bit10
'    Private Const DaylightSavingsTimeMask As Int32 = Bit11

'#End Region

'#Region " Public Code "

'    Public Const BinaryLength As Integer = 16

'#Region " Constructors "

'    Public Sub New(ByVal binaryImage As Byte())

'        MyClass.New(binaryImage, 0)

'    End Sub

'    Public Sub New(ByVal binaryImage As Byte(), ByVal startIndex As Integer)

'        MyBase.New()
'        If binaryImage IsNot Nothing Then
'            If binaryImage.Length - startIndex >= BinaryLength Then
'                m_tTag = New TimeTag(BitConverter.ToDouble(binaryImage, startIndex))
'                m_flags = BitConverter.ToInt32(binaryImage, startIndex + 8)
'                m_value = BitConverter.ToSingle(binaryImage, startIndex + 12)
'            Else
'                Throw New ArgumentException("Binary image size from startIndex is too small.")
'            End If
'        Else
'            Throw New ArgumentNullException("binaryImage")
'        End If

'    End Sub

'    Public Sub New(ByVal seconds As Double, ByVal value As Single, ByVal quality As Quality)

'        MyClass.New(New TimeTag(seconds), value, quality)

'    End Sub

'    Public Sub New(ByVal timestamp As Date, ByVal value As Single, ByVal quality As Quality)

'        MyClass.New(New TimeTag(timestamp), value, quality)

'    End Sub

'    Public Sub New(ByVal tTag As TimeTag, ByVal value As Single, ByVal quality As Quality)

'        MyClass.New(tTag, value, -1)
'        Me.Quality = quality

'    End Sub

'    Public Sub New(ByVal seconds As Double, ByVal value As Single, ByVal flags As Int32)

'        MyClass.New(New TimeTag(seconds), value, flags)

'    End Sub

'    Public Sub New(ByVal timestamp As Date, ByVal value As Single, ByVal flags As Int32)

'        MyClass.New(New TimeTag(timestamp), value, flags)

'    End Sub

'    Public Sub New(ByVal tTag As TimeTag, ByVal value As Single, ByVal flags As Int32)

'        m_tTag = tTag
'        m_value = value
'        m_flags = flags

'    End Sub

'#End Region

'    Public Property TTag() As TimeTag
'        Get
'            Return m_tTag
'        End Get
'        Set(ByVal value As TimeTag)
'            m_tTag = value
'        End Set
'    End Property

'    Public Property Value() As Single
'        Get
'            Return m_value
'        End Get
'        Set(ByVal value As Single)
'            m_value = value
'        End Set
'    End Property

'    Public Property Quality() As Quality
'        Get
'            Return CType((m_flags And QualityMask), Quality)
'        End Get
'        Set(ByVal value As Quality)
'            m_flags = (m_flags Or value)
'        End Set
'    End Property

'    Public Property Definition() As PointDefinition
'        Get
'            Return m_definition
'        End Get
'        Set(ByVal value As PointDefinition)
'            m_definition = value
'        End Set
'    End Property

'    Public ReadOnly Property BinaryImage() As Byte()
'        Get
'            Dim image As Byte() = CreateArray(Of Byte)(BinaryLength)

'            ' Construct the binary IP buffer for this event
'            Array.Copy(BitConverter.GetBytes(m_tTag.Value), 0, image, 0, 8)
'            Array.Copy(BitConverter.GetBytes(m_flags), 0, image, 8, 4)
'            Array.Copy(BitConverter.GetBytes(m_value), 0, image, 12, 4)

'            Return image
'        End Get
'    End Property

'#Region " IComparable Implementation "

'    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo

'        Dim other As ExtendedPointData = TryCast(obj, ExtendedPointData)
'        If other IsNot Nothing Then
'            Return m_tTag.CompareTo(other.TTag)
'        Else
'            Throw New ArgumentException(String.Format("Cannot compare {0} with {1}.", Me.GetType().Name, other.GetType().Name))
'        End If

'    End Function

'#End Region

'#End Region

'End Class

#End Region