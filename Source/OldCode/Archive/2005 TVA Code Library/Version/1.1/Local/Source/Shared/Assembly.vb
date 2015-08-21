'Author: Pinal Patel
'Created: 04/29/05
'Modified: 06/03/05
'Description: This class reads the assembly attributes from an AssemblyInfo.vb file.



'Namespaces used.
Imports System.Reflection.Assembly
Imports System.Text.RegularExpressions
Imports System.IO

Namespace [Shared]

    Public Class [Assembly]

        Private m_Assembly As System.Reflection.Assembly
        Private Shared m_CallingAssembly As [Assembly]
        Private Shared m_EntryAssembly As [Assembly]
        Private Shared m_ExecutingAssembly As [Assembly]
        ''' <summary>
        '''Initializes a new instance of the Assembly class using the specified System.Reflection.Assembly.  
        ''' </summary>
        '''<param name="Asm"> Required.An instance that represents the currently loaded assembly</param>
        Public Sub New(ByVal Asm As System.Reflection.Assembly)
            'Set the assembly.
            m_Assembly = Asm
        End Sub
        ''' <summary>
        '''  Returns the Assembly of the method that invoked the currently executing method
        ''' </summary>
        ''' <value>
        ''' Calling Assembly
        ''' </value>
       
        Public Shared ReadOnly Property CallingAssembly() As [Assembly]
            Get
                If m_CallingAssembly Is Nothing Then m_CallingAssembly = New [Assembly](GetCallingAssembly())
                Return m_CallingAssembly
            End Get
        End Property
        ''' <summary>
        ''' Gets the process executable in the default application domain
        ''' </summary>
        ''' <value>
        '''  Entry Assembly
        ''' </value>
        Public Shared ReadOnly Property EntryAssembly() As [Assembly]
            Get
                If m_EntryAssembly Is Nothing Then m_EntryAssembly = New [Assembly](GetEntryAssembly())
                Return m_EntryAssembly
            End Get
        End Property
        ''' <summary>
        ''' Gets Assembly that the current code is running from
        ''' </summary>
        ''' <value>
        '''  Executing Assembly
        ''' </value>
        Public Shared ReadOnly Property ExecutingAssembly() As [Assembly]
            Get
                If m_ExecutingAssembly Is Nothing Then m_ExecutingAssembly = New [Assembly](GetExecutingAssembly())
                Return m_ExecutingAssembly
            End Get
        End Property
        ''' <summary>
        ''' Gets assembly title information 
        ''' </summary>
        ''' <value>
        '''  Title attribute
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Title() As String
            Get
                'Returns the Title attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyTitleAttribute)), System.Reflection.AssemblyTitleAttribute).Title.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Description
        ''' </summary>
        ''' <value>
        '''  Description
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Description() As String
            Get
                'Returns the Description attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyDescriptionAttribute)), System.Reflection.AssemblyDescriptionAttribute).Description.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Company Attribute
        ''' </summary>
        ''' <value>
        '''  Company
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Company() As String
            Get
                'Returns the Company attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyCompanyAttribute)), System.Reflection.AssemblyCompanyAttribute).Company.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Product
        ''' </summary>
        ''' <value>
        '''  Product
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Product() As String
            Get
                'Returns the Product attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyProductAttribute)), System.Reflection.AssemblyProductAttribute).Product.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Copyright attribute
        ''' </summary>
        ''' <value>
        '''  Copyright
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Copyright() As String
            Get
                'Returns the Copyright attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyCopyrightAttribute)), System.Reflection.AssemblyCopyrightAttribute).Copyright.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Trademark attribute
        ''' </summary>
        ''' <value>
        '''  Trademark
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Trademark() As String
            Get
                'Returns the Trademark attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyTrademarkAttribute)), System.Reflection.AssemblyTrademarkAttribute).Trademark.ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Configuration attribute
        ''' </summary>
        ''' <value>
        ''' Configuration
        ''' </value>
        '''  <remarks>
        ''' Value must be string
        ''' </remarks>
        Public ReadOnly Property Configuration() As String
            Get
                'Returns the Configuration attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyConfigurationAttribute)), System.Reflection.AssemblyConfigurationAttribute).Configuration()
            End Get
        End Property
        ''' <summary>
        ''' Gets the DelaySign attribute
        ''' </summary>
        ''' <value>
        ''' Delay Sign
        ''' </value>
        '''  <remarks>
        ''' Value must be Boolean
        ''' </remarks>
        Public ReadOnly Property DelaySign() As Boolean
            Get
                'Returns the DelaySign attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyDelaySignAttribute)), System.Reflection.AssemblyDelaySignAttribute).DelaySign()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Informational version attribute
        ''' </summary>
        ''' <value>
        ''' InformationalVersion
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property InformationalVersion() As String
            Get
                'Returns the InformationalVersion attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyInformationalVersionAttribute)), System.Reflection.AssemblyInformationalVersionAttribute).InformationalVersion()
            End Get
        End Property
        ''' <summary>
        ''' Gets the Key File attribute
        ''' </summary>
        ''' <value>
        ''' Key File
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property KeyFile() As String
            Get
                'Returns the KeyFile attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Reflection.AssemblyKeyFileAttribute)), System.Reflection.AssemblyKeyFileAttribute).KeyFile()
            End Get
        End Property
        ''' <summary>
        ''' Gets the NeutralResourcesLanguage attribute
        ''' </summary>
        ''' <value>
        ''' NeutralResourcesLanguage
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property NeutralResourcesLanguage() As String
            Get
                'Returns the NeutralResourcesLanguage attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Resources.NeutralResourcesLanguageAttribute)), System.Resources.NeutralResourcesLanguageAttribute).CultureName()
            End Get
        End Property
        ''' <summary>
        ''' Gets the SatelliteContractVersion Attribute 
        ''' </summary>
        ''' <value>
        ''' SatelliteContract Version
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property SatelliteContractVersion() As String
            Get
                'Returns the SatelliteContractVersionAttribute attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Resources.SatelliteContractVersionAttribute)), System.Resources.SatelliteContractVersionAttribute).Version()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  ComCompatibleVersion attribute
        ''' </summary>
        ''' <value>
        ''' SatelliteContract Version
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property ComCompatibleVersion() As String
            Get
                'Returns the ComCompatibleVersion attribute.
                Dim oComCompatibleVersionAttribute As System.Runtime.InteropServices.ComCompatibleVersionAttribute = DirectCast(GetCustomAttribute(GetType(System.Runtime.InteropServices.ComCompatibleVersionAttribute)), System.Runtime.InteropServices.ComCompatibleVersionAttribute)
                Return oComCompatibleVersionAttribute.MajorVersion().ToString() & "." & oComCompatibleVersionAttribute.MinorVersion().ToString() & "." & oComCompatibleVersionAttribute.RevisionNumber().ToString() & "." & oComCompatibleVersionAttribute.BuildNumber().ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  ComVisible attribute
        ''' </summary>
        ''' <value>
        ''' ComVisible 
        ''' </value>
        '''  <remarks>
        ''' Value must be Boolean
        ''' </remarks>
        Public ReadOnly Property ComVisible() As Boolean
            Get
                'Returns the ComVisible attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Runtime.InteropServices.ComVisibleAttribute)), System.Runtime.InteropServices.ComVisibleAttribute).Value()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  Guid attribute
        ''' </summary>
        ''' <value>
        ''' Guid
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property Guid() As String
            Get
                'Returns the Guid attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Runtime.InteropServices.GuidAttribute)), System.Runtime.InteropServices.GuidAttribute).Value()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  TypeLibVersion attribute
        ''' </summary>
        ''' <value>
        ''' TypeLibVersion
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property TypeLibVersion() As String
            Get
                'Returns the TypeLibVersion attribute.
                Dim oTypeLibVersionAttribute As System.Runtime.InteropServices.TypeLibVersionAttribute = DirectCast(GetCustomAttribute(GetType(System.Runtime.InteropServices.TypeLibVersionAttribute)), System.Runtime.InteropServices.TypeLibVersionAttribute)
                Return oTypeLibVersionAttribute.MajorVersion().ToString() & "." & oTypeLibVersionAttribute.MinorVersion().ToString()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  CLSCompliant attribute
        ''' </summary>
        ''' <value>
        ''' CLSCompliant
        ''' </value>
        '''  <remarks>
        ''' Value must be Boolean
        ''' </remarks>
        Public ReadOnly Property CLSCompliant() As Boolean
            Get
                'Returns the CLSCompliant attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.CLSCompliantAttribute)), System.CLSCompliantAttribute).IsCompliant()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  Debuggable attribute
        ''' </summary>
        ''' <value>
        ''' Debuggable
        ''' </value>
        '''  <remarks>
        ''' Value must be Boolean
        ''' </remarks>
        Public ReadOnly Property Debuggable() As Boolean
            Get
                'Returns the Debuggable attribute.
                Return DirectCast(GetCustomAttribute(GetType(System.Diagnostics.DebuggableAttribute)), System.Diagnostics.DebuggableAttribute).IsJITTrackingEnabled()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  Location attribute
        ''' </summary>
        ''' <value>
        ''' Location
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property Location() As String
            Get
                'Returns the location of the assembly.
                Return m_Assembly.Location().ToLower()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  CodeBase attribute
        ''' </summary>
        ''' <value>
        ''' CodeBase
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property CodeBase() As String
            Get
                'Returns the location of the assembly in codebase format.
                Return m_Assembly.CodeBase().Replace("file:///", "").ToLower()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  full name of the assembly
        ''' </summary>
        ''' <value>
        ''' full name of the assembly
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property FullName() As String
            Get
                'Returns the full name of the assembly.
                Return m_Assembly.FullName()
            End Get
        End Property
        ''' <summary>
        ''' Gets the name of the assembly
        ''' </summary>
        ''' <value>
        ''' Name of the assembly
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property Name() As String
            Get
                'Returns the name of the assembly.
                Return m_Assembly.GetName().Name()
            End Get
        End Property
        ''' <summary>
        ''' Gets the version of the assembly
        ''' </summary>
        ''' <value>
        ''' Assembly Version
        ''' </value>
        '''  <remarks>
        ''' Value must be Version
        ''' </remarks>
        Public ReadOnly Property Version() As Version
            Get
                'Returns the Version attribute.
                Return m_Assembly.GetName().Version()
            End Get
        End Property
        ''' <summary>
        ''' Gets the  version of Common Language Runtime
        ''' </summary>
        ''' <value>
        ''' CLR Version
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property ImageRuntimeVersion() As String
            Get
                'Returns the version of Common Language Runtime.
                Return m_Assembly.ImageRuntimeVersion()
            End Get
        End Property
        ''' <summary>
        ''' Gets whether the assembly was loaded from the GAC
        ''' </summary>
        ''' <value>
        ''' True or False
        ''' </value>
        '''  <remarks>
        ''' Value must be Boolean
        ''' </remarks>
        Public ReadOnly Property GACLoaded() As Boolean
            Get
                'Returns whether the assembly was loaded from the GAC.
                Return m_Assembly.GlobalAssemblyCache()
            End Get
        End Property
        ''' <summary>
        ''' Gets the date and time when the assembly was last built.
        ''' </summary>
        ''' <value>
        ''' Date assembly was last built
        ''' </value>
        '''  <remarks>
        ''' Value must be Date
        ''' </remarks>
        Public ReadOnly Property BuildDate() As Date
            Get
                'Returns the date and time when the assembly was last built.
                Return IO.File.GetLastWriteTime(m_Assembly.Location())
            End Get
        End Property
        ''' <summary>
        ''' Gets the root namespace of the assembly.
        ''' </summary>
        ''' <value>
        ''' RootNameSpace
        ''' </value>
        '''  <remarks>
        ''' Value must be String
        ''' </remarks>
        Public ReadOnly Property RootNamespace() As String
            Get
                'Returns the root namespace of the assembly.
                Dim strRootNamespace As String = m_Assembly.GetExportedTypes(0).AssemblyQualifiedName()
                Return strRootNamespace.Substring(0, strRootNamespace.IndexOf("."c))
            End Get
        End Property
        '''<summary>
        '''<para> Retrieves a collection of the  Assembly Info attributes </para>
        '''</summary>
        '''<returns>
        '''<para>A collection containing the  attributes of AssemblyInfo.
        '''	</para>
        '''</returns>
        Public Function GetAttributes() As Specialized.NameValueCollection

            Dim nvc As New Specialized.NameValueCollection

            With nvc
                'Add some values that are not in AssemblyInfo.
                .Add("Full Name", FullName())
                .Add("Name", Name())
                .Add("Version", Version().ToString())
                .Add("Image Runtime Version", ImageRuntimeVersion())
                .Add("Build Date", BuildDate().ToString())
                .Add("Location", Location())
                .Add("Code Base", CodeBase())
                .Add("GAC Loaded", GACLoaded().ToString())


                'Add all attributes available from AssemblyInfo.
                For Each oAttribute As Object In m_Assembly.GetCustomAttributes(False)
                    'Dim strName As String = Regex.Match(oAttribute.GetType().ToString(), "(\.Assembly|\.)(?<Name>[^.]*)Attribute$", RegexOptions.IgnoreCase).Groups("Name").Value()

                    If TypeOf oAttribute Is System.Reflection.AssemblyTitleAttribute Then
                        .Add("Title", Title())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyDescriptionAttribute Then
                        .Add("Description", Description())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyCompanyAttribute Then
                        .Add("Company", Company())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyProductAttribute Then
                        .Add("Product", Product())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyCopyrightAttribute Then
                        .Add("Copyright", Copyright())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyTrademarkAttribute Then
                        .Add("Trademark", Trademark())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyConfigurationAttribute Then
                        .Add("Configuration", Configuration())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyDelaySignAttribute Then
                        .Add("Delay Sign", DelaySign().ToString())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyInformationalVersionAttribute Then
                        .Add("Informational Version", InformationalVersion())
                    ElseIf TypeOf oAttribute Is System.Reflection.AssemblyKeyFileAttribute Then
                        .Add("Key File", KeyFile())
                    ElseIf TypeOf oAttribute Is System.Resources.NeutralResourcesLanguageAttribute Then
                        nvc.Add("Neutral Resources Language", NeutralResourcesLanguage())
                    ElseIf TypeOf oAttribute Is System.Resources.SatelliteContractVersionAttribute Then
                        .Add("Satellite Contract Version", SatelliteContractVersion())
                    ElseIf TypeOf oAttribute Is System.Runtime.InteropServices.ComCompatibleVersionAttribute Then
                        .Add("Com Compatible Version", ComCompatibleVersion())
                    ElseIf TypeOf oAttribute Is System.Runtime.InteropServices.ComVisibleAttribute Then
                        .Add("Com Visible", ComVisible().ToString())
                    ElseIf TypeOf oAttribute Is System.Runtime.InteropServices.GuidAttribute Then
                        nvc.Add("Guid", Guid())
                    ElseIf TypeOf oAttribute Is System.Runtime.InteropServices.TypeLibVersionAttribute Then
                        .Add("Type Lib Version", TypeLibVersion())
                    ElseIf TypeOf oAttribute Is System.CLSCompliantAttribute Then
                        .Add("CLS Compliant", CLSCompliant().ToString())
                    ElseIf TypeOf oAttribute Is System.Diagnostics.DebuggableAttribute Then
                        .Add("Debuggable", Debuggable().ToString())
                    End If
                Next
            End With


            Return nvc

        End Function
        '''  <summary>
        '''<para> Gets all the custom attributes for this assembly.  .</para>
        '''</summary>
        '''<param name="AttributeType">The <see cref="T:System.Type" /> object to which the custom attributes are applied.</param>
        '''<returns>
        ''' <para> custom attributes of type <paramref name="AttributeType" /> .
        '''</para>
        '''</returns>
        Public Function GetCustomAttribute(ByVal AttributeType As System.Type) As Object

            'Returns the requested assembly attribute.
            Dim objAttribute As Object() = m_Assembly.GetCustomAttributes(AttributeType, False)
            If objAttribute.Length() >= 1 Then
                Return objAttribute(0)
            Else
                Throw New ApplicationException("Assembly does not expose this attribute")
            End If

        End Function
        '''  <summary>
        '''<para> Extract and return the requested embedded resource .</para>
        '''</summary>
        '''<param name="ResourceName">Resource to be extracted.</param>
        '''<returns>
        ''' <para> Requested Resource.
        '''</para>
        '''</returns>
        Public Function GetEmbeddedResource(ByVal ResourceName As System.String) As Stream

            'Extract and return the requested embedded resource.
            Return m_Assembly.GetManifestResourceStream(RootNamespace() & "." & ResourceName)

        End Function
        '''  <summary>
        '''<para> Load the assembly from embedded resource .</para>
        '''</summary>
        '''<param name="assemblyName">The loaded assembly.</param>
        
        Public Shared Sub LoadAssemblyFromResource(ByVal assemblyName As System.String)

            Static addedResolver As Boolean

            ' Hook into assembly resolve event for current domain so we can load assembly from embedded resource
            If Not addedResolver Then
                AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblyFromResource
                addedResolver = True
            End If

            ' Load the assembly (this will invoke event that will resolve assembly from resource)
            AppDomain.CurrentDomain.Load(assemblyName)

        End Sub
        '''  <summary>
        '''<para> Resolve Assembly for the embedded resource.</para>
        '''</summary>
        '''<param name="sender">Required.</param>
        ''' <param name="args">Required.</param>
        '''<returns>
        ''' <para> Resource Assembly
        '''</para>
        '''</returns>
        Private Shared Function ResolveAssemblyFromResource(ByVal sender As Object, ByVal args As ResolveEventArgs) As System.Reflection.Assembly

            Static assemblyCache As New Hashtable
            Static rootNameSpace As System.String
            Dim resourceAssembly As System.Reflection.Assembly
            Dim shortName As String = args.Name.Split(","c)(0)

            resourceAssembly = assemblyCache(shortName)

            If resourceAssembly Is Nothing Then
                ' Get root namespace of executing assembly since all embedded resources will be prefixed with this
                If rootNameSpace Is Nothing Then
                    rootNameSpace = GetEntryAssembly().GetExportedTypes(0).AssemblyQualifiedName()
                    rootNameSpace = rootNameSpace.Substring(0, rootNameSpace.IndexOf("."c))
                End If

                ' Loop through all of the resources in the executing assembly
                For Each name As String In GetEntryAssembly.GetManifestResourceNames()
                    ' See if the embedded resource name matches assembly we are trying to load
                    If String.Compare(Path.GetFileNameWithoutExtension(name), rootNameSpace & "." & shortName, True) = 0 Then
                        ' If so, load embedded resource assembly into a binary buffer
                        With GetEntryAssembly.GetManifestResourceStream(name)
                            Dim buffer As Byte() = Array.CreateInstance(GetType(Byte), .Length)
                            .Read(buffer, 0, .Length)
                            .Close()

                            ' Load assembly from binary buffer
                            resourceAssembly = Load(buffer)
                            assemblyCache.Add(shortName, resourceAssembly)
                            Exit For
                        End With
                    End If
                Next
            End If

            Return resourceAssembly

        End Function

    End Class

End Namespace
