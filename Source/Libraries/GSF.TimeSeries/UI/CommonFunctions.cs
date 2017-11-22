﻿//******************************************************************************************************
//  CommonFunctions.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/31/2011 - Mehulbhai P Thakkar
//       Generated original version of source code.
//  12/20/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using GSF.Communication;
using GSF.Data;
using GSF.Identity;
using GSF.IO;
using GSF.Security;
using GSF.TimeSeries.UI.DataModels;

namespace GSF.TimeSeries.UI
{
    /// <summary>
    /// Represents a static class containing common methods.
    /// </summary>
    public static class CommonFunctions
    {
        #region [ Members ]

        /// <summary>
        /// Defines the default settings category for TimeSeriesFramework data connections.
        /// </summary>
        public const string DefaultSettingsCategory = "systemSettings";

        /// <summary>
        /// Defines the maximum number of pages to be stored in page history.
        /// </summary>
        public const int MaxPageHistory = 10;

        #endregion

        #region [ Static ]

        // Static Fields
        private static SecurityPrincipal s_currentPrincipal;
        private static Guid s_currentNodeID;
        private static string s_serviceConnectionString;
        private static string s_dataPublisherConnectionString;
        private static string s_realTimeStatisticServiceUrl;
        private static string s_timeSeriesDataServiceUrl;
        private static WindowsServiceClient s_windowsServiceClient;
        private static bool s_retryServiceConnection;
        private static LinkedList<Tuple<string, Type, object[]>> s_pageHistory;
        private static LinkedListNode<Tuple<string, Type, object[]>> s_currentPage;

        // Static Properties

        /// <summary>
        /// Defines the current principal for the thread owning the common functions.
        /// </summary>
        public static SecurityPrincipal CurrentPrincipal
        {
            get
            {
                return s_currentPrincipal;
            }
            set
            {
                s_currentPrincipal = value;
            }
        }

        /// <summary>
        /// Defines the current user name as defined in the CurrentPrincipal.Identity.
        /// </summary>
        public static string CurrentUser => CurrentPrincipal.Identity.Name;

        /// <summary>
        /// Gets the flag that indicates whether we can go forward to the next user control in history.
        /// </summary>
        public static bool CanGoForward
        {
            get
            {
                SecurityPrincipal currentPrincipal = CurrentPrincipal;
                ISecurityProvider securityProvider = currentPrincipal.Identity.Provider;

                return
                    ((object)s_currentPage != null) &&
                    ((object)s_currentPage.Next != null) &&
                    currentPrincipal.Identity.IsAuthenticated &&
                    securityProvider.UserData.Roles.Any();
            }
        }

        /// <summary>
        /// Gets the flag that indicates whether we can go back to the previous user control in history.
        /// </summary>
        public static bool CanGoBack
        {
            get
            {
                SecurityPrincipal currentPrincipal = CurrentPrincipal;
                ISecurityProvider securityProvider = currentPrincipal.Identity.Provider;

                return
                    ((object)s_currentPage != null) &&
                    ((object)s_currentPage.Previous != null) &&
                    currentPrincipal.Identity.IsAuthenticated &&
                    securityProvider.UserData.Roles.Any();
            }
        }

        // Events

        /// <summary>
        /// Used to notify main window that connection to service has changed.
        /// </summary>
        public static event EventHandler ServiceConnectionRefreshed = delegate
        {
        };

        /// <summary>
        /// Event raised when the <see cref="CurrentPrincipal"/> property changes as
        /// a result of its security provider expiring from the security provider cache.
        /// </summary>
        public static event EventHandler CurrentPrincipalRefreshed;

        /// <summary>
        /// Triggered when the flag that indicates whether we can move forward in page history changes.
        /// </summary>
        public static event EventHandler CanGoForwardChanged;

        /// <summary>
        /// Triggered when the flag that indicates whether we can move backward in page history changes.
        /// </summary>
        public static event EventHandler CanGoBackChanged;

        // Static Methods

        #region [ AdoDataConnection Extension Methods ]

        /// <summary>
        /// Sets the current user context for the database.
        /// </summary>
        /// <remarks>
        /// Purpose of this method is to supply current user information from the UI to DELETE trigger for change logging.
        /// This method must be called before any delete operation on the database in order to log who deleted this record.
        /// For SQL server it sets user name into CONTEXT_INFO().
        /// For MySQL server it sets user name into session variable @context.
        /// For Oracle server it sets user name into context package.
        /// MS Access is not supported for change logging.
        /// For any other database in the future, such as Oracle, this logic must be extended to support change log in the database.
        /// </remarks>
        /// <param name="database"><see cref="AdoDataConnection"/> used to set user context before any delete operation.</param>
        public static void SetCurrentUserContext(AdoDataConnection database)
        {
            bool createdConnection = false;

            try
            {
                if (!string.IsNullOrEmpty(CurrentUser))
                {
                    if ((object)database == null)
                    {
                        database = new AdoDataConnection(DefaultSettingsCategory);
                        createdConnection = true;
                    }

                    IDbCommand command;
                    string connectionType = database.Connection.GetType().Name.ToLower();

                    // Set Current User for the database session for this connection.

                    switch (connectionType)
                    {
                        case "sqlconnection":
                            const string contextSql = "DECLARE @context VARBINARY(128)\n SELECT @context = CONVERT(VARBINARY(128), CONVERT(VARCHAR(128), @userName))\n SET CONTEXT_INFO @context";
                            command = database.Connection.CreateCommand();
                            command.CommandText = contextSql;
                            command.AddParameterWithValue("@userName", CurrentUser);
                            command.ExecuteNonQuery();
                            break;
                        case "mysqlconnection":
                            command = database.Connection.CreateCommand();
                            command.CommandText = "SET @context = '" + CurrentUser + "';";
                            command.ExecuteNonQuery();
                            break;
                        case "oracleconnection":
                            command = database.Connection.CreateCommand();
                            command.CommandText = "BEGIN context.set_current_user('" + CurrentUser + "'); END;";
                            command.ExecuteNonQuery();
                            break;
                    }
                }
            }
            finally
            {
                if (createdConnection)
                    database.Dispose();
            }
        }

        ///// <summary>
        ///// Retrieves connection string to connect to backend windows service.
        ///// </summary>
        ///// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        ///// <returns>IP address and port on which backend windows service is running.</returns>
        //public static string RemoteStatusServerConnectionString(this AdoDataConnection database)
        //{
        //    if (string.IsNullOrEmpty(s_remoteStatusServerConnectionString))
        //        database.GetNodeSettings();

        //    return s_remoteStatusServerConnectionString;
        //}

        /// <summary>
        /// Retrieves web service url to query real time statistics values.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <returns>string, url to web service.</returns>
        public static string RealTimeStatisticServiceUrl(this AdoDataConnection database)
        {
            if (string.IsNullOrEmpty(s_realTimeStatisticServiceUrl))
                database.GetNodeSettings();

            return s_realTimeStatisticServiceUrl;
        }

        ///// <summary>
        ///// Retrieves a port number on which back end service is publishing data.
        ///// </summary>
        ///// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        ///// <returns>port number on which data is being published.</returns>
        //public static string DataPublisherPort(this AdoDataConnection database)
        //{
        //    if (string.IsNullOrEmpty(s_dataPublisherPort))
        //        database.GetNodeSettings();

        //    return s_dataPublisherPort;
        //}

        /// <summary>
        /// Retrieves web service url to query real time data.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <returns>string, url to web service.</returns>
        public static string TimeSeriesDataServiceUrl(this AdoDataConnection database)
        {
            if (string.IsNullOrEmpty(s_timeSeriesDataServiceUrl))
                database.GetNodeSettings();

            return s_timeSeriesDataServiceUrl;
        }

        /// <summary>
        /// Retrieves connection string information to connect to backed windows service from UI.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <param name="overwrite">Flag that determines if cached connection string should be overwritten.</param>
        /// <returns>Connection string to connect to backend windows service.</returns>
        public static string ServiceConnectionString(this AdoDataConnection database, bool overwrite = false)
        {
            if (string.IsNullOrEmpty(s_serviceConnectionString) || overwrite)
                database.GetNodeSettings();

            return s_serviceConnectionString;
        }

        /// <summary>
        /// Retrieves connection string to subscribe data from data published hosted by the backend windows service.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        /// <returns>Connection string to subscribe from data publisher.</returns>
        public static string DataPublisherConnectionString(this AdoDataConnection database)
        {
            if (string.IsNullOrEmpty(s_dataPublisherConnectionString))
                database.GetNodeSettings();

            return s_dataPublisherConnectionString;
        }

        /// <summary>
        /// Method to parse Settings field value for current node defined in the database and extract various parameters to communicate with backend windows service.
        /// </summary>
        /// <param name="database"><see cref="AdoDataConnection"/> to database.</param>
        private static void GetNodeSettings(this AdoDataConnection database)
        {
            Node node = Node.GetCurrentNode(database);
            if (node != null)
            {
                string interfaceValue = string.Empty;
                Dictionary<string, string> settings = node.Settings.ToLower().ParseKeyValuePairs();

                if (settings.ContainsKey("realtimestatisticserviceurl"))
                    s_realTimeStatisticServiceUrl = settings["realtimestatisticserviceurl"];

                if (settings.ContainsKey("timeseriesdataserviceurl"))
                    s_timeSeriesDataServiceUrl = settings["timeseriesdataserviceurl"];

                if (settings.ContainsKey("interface"))
                    interfaceValue = settings["interface"];

                if (settings.ContainsKey("remotestatusserverconnectionstring"))
                {
                    Dictionary<string, string> serviceSettings;

                    s_serviceConnectionString = settings["remotestatusserverconnectionstring"];
                    serviceSettings = s_serviceConnectionString.ParseKeyValuePairs();

                    if (serviceSettings.ContainsKey("interface"))
                        interfaceValue = serviceSettings["interface"];

                    if (serviceSettings.ContainsKey("server"))
                    {
                        string server = serviceSettings["server"];

                        if (serviceSettings.ContainsKey("datapublisherport"))
                        {
                            s_dataPublisherConnectionString = "server=" + server.Substring(0, server.LastIndexOf(":", StringComparison.OrdinalIgnoreCase) + 1) + serviceSettings["datapublisherport"];

                            if (!string.IsNullOrEmpty(interfaceValue))
                                s_dataPublisherConnectionString += ";interface=" + interfaceValue;
                        }
                        else if (settings.ContainsKey("datapublisherport"))
                        {
                            s_dataPublisherConnectionString = "server=" + server.Substring(0, server.LastIndexOf(":", StringComparison.OrdinalIgnoreCase) + 1) + settings["datapublisherport"];

                            if (!string.IsNullOrEmpty(interfaceValue))
                                s_dataPublisherConnectionString += ";interface=" + interfaceValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns current node id <see cref="System.Guid"/> UI is connected to.
        /// </summary>
        /// <returns>Current Node ID.</returns>
        public static Guid CurrentNodeID()
        {
            return s_currentNodeID;
        }

        /// <summary>
        /// Returns current node id <see cref="System.Guid"/> UI is connected to.
        /// </summary>
        /// <param name="database">Connected <see cref="AdoDataConnection"/></param>
        /// <returns>Proper <see cref="System.Guid"/> implementation for current node id.</returns>
        public static object CurrentNodeID(this AdoDataConnection database)
        {
            return database.Guid(s_currentNodeID);
        }

        #endregion

        /// <summary>
        /// Assigns <see cref="CurrentNodeID()"/> based ID of currently active node.
        /// </summary>
        /// <param name="nodeID">Current node ID <see cref="CurrentNodeID()"/> to assign.</param>
        public static void SetAsCurrentNodeID(this Guid nodeID)
        {
            if (s_currentNodeID != nodeID)
            {
                s_currentNodeID = nodeID;

                // When node selection changes, reset other static members related to node.
                s_serviceConnectionString = string.Empty;
                s_dataPublisherConnectionString = string.Empty;
                s_realTimeStatisticServiceUrl = string.Empty;
                s_timeSeriesDataServiceUrl = string.Empty;
                SetRetryServiceConnection(true);
                DisconnectWindowsServiceClient();
                ConnectWindowsServiceClient();
            }
        }

        /// <summary>
        /// Returns <see cref="DBNull"/> if given <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="value">Value to test for null.</param>
        /// <returns><see cref="DBNull"/> if <paramref name="value"/> is <c>null</c>; otherwise <paramref name="value"/>.</returns>
        public static object ToNotNull(this object value)
        {
            if ((object)value == null)
                return (object)DBNull.Value;

            if (value is int && (int)value == 0)
                return (object)DBNull.Value;

            return value;
        }

        /// <summary>
        /// Returns a collection of down sampling methods.
        /// </summary>
        /// <returns><see cref="Dictionary{T1,T2}"/> type collection of down sampling methods.</returns>
        public static Dictionary<string, string> GetDownsamplingMethodLookupList()
        {
            Dictionary<string, string> downsamplingLookupList = new Dictionary<string, string>();

            downsamplingLookupList.Add("LastReceived", "LastReceived");
            downsamplingLookupList.Add("Closest", "Closest");
            downsamplingLookupList.Add("Filtered", "Filtered");
            downsamplingLookupList.Add("BestQuality", "BestQuality");

            return downsamplingLookupList;
        }

        /// <summary>
        /// Returns a collection of system time zones.
        /// </summary>
        /// <param name="isOptional">Indicates if selection on UI is optional for this collection.</param>
        /// <returns><see cref="Dictionary{T1,T2}"/> type collection of system time zones.</returns>
        public static Dictionary<string, string> GetTimeZones(bool isOptional)
        {
            Dictionary<string, string> timeZonesList = new Dictionary<string, string>();

            if (isOptional)
                timeZonesList.Add("", "Select Time Zone");

            foreach (TimeZoneInfo timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                if (!timeZonesList.ContainsKey(timeZoneInfo.Id))
                    timeZonesList.Add(timeZoneInfo.Id, timeZoneInfo.DisplayName);
            }

            return timeZonesList;
        }

        /// <summary>
        /// Retrieves children of an UIElement based on type.
        /// </summary>
        /// <param name="parent">Parent UIElement.</param>
        /// <param name="targetType">Type of child UIElement looking for within parent UIElement.</param>
        /// <param name="children">Reference parameter to return child collection.</param>
        public static void GetChildren(UIElement parent, Type targetType, ref List<UIElement> children)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                    if (child.GetType() == targetType)
                    {
                        children.Add(child);
                    }
                    GetChildren(child, targetType, ref children);
                }
            }
        }

        /// <summary>
        /// Retrieves first child of an UIElement based on type.
        /// </summary>
        /// <param name="parent">Parent UIElement</param>
        /// <param name="targetType">Type of the child UIElement.</param>
        /// <param name="element">Reference parameter to return UIElement.</param>
        public static void GetFirstChild(UIElement parent, Type targetType, ref UIElement element)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                    if (child.GetType() == targetType)
                    {
                        element = child;
                        break;
                    }

                    GetFirstChild(child, targetType, ref element);
                }
            }
        }

        /// <summary>
        /// Retrieves runtime id for an object.
        /// </summary>
        /// <param name="sourceTable">Table where object has been defined.</param>
        /// <param name="sourceID">ID of an object in source table.</param>
        /// <param name="database">Existing database connection, if available.</param>
        /// <returns>string, id of an object in Runtime table.</returns>
        public static string GetRuntimeID(string sourceTable, int sourceID, AdoDataConnection database = null)
        {
            string runtimeID = string.Empty;
            bool createdConnection = false;

            try
            {
                if ((object)database == null)
                {
                    database = new AdoDataConnection(DefaultSettingsCategory);
                    createdConnection = true;
                }

                string query = database.ParameterizedQueryString("SELECT ID FROM Runtime WHERE SourceTable = {0} AND SourceID = {1}", "sourceTable", "sourceID");
                object id = database.Connection.ExecuteScalar(query, sourceTable, sourceID);

                if (id != null)
                    runtimeID = id.ToString();

                return runtimeID;
            }
            finally
            {
                if (createdConnection)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Sets a boolean flag indicating if connection cycle should be continued.
        /// </summary>
        /// <param name="retry"></param>
        public static void SetRetryServiceConnection(bool retry)
        {
            s_retryServiceConnection = retry;
            if (!retry)
                DisconnectWindowsServiceClient();
        }

        /// <summary>
        /// Retrieves <see cref="WindowsServiceClient"/> object.
        /// </summary>
        /// <returns><see cref="WindowsServiceClient"/> object.</returns>
        public static WindowsServiceClient GetWindowsServiceClient()
        {
            ConnectWindowsServiceClient();
            return s_windowsServiceClient;
        }

        /// <summary>
        /// Connects to backend windows service.
        /// </summary>
        public static void ConnectWindowsServiceClient(bool overwrite = false)
        {
            TlsClient remotingClient;
            ISecurityProvider securityProvider;

            if (overwrite)
            {
                DisconnectWindowsServiceClient();
                ServiceConnectionRefreshed(null, EventArgs.Empty);
            }
            else if (s_windowsServiceClient == null || s_windowsServiceClient.Helper.RemotingClient.CurrentState != ClientState.Connected)
            {
                if (s_windowsServiceClient != null)
                    DisconnectWindowsServiceClient();

                AdoDataConnection database = new AdoDataConnection(DefaultSettingsCategory);

                try
                {
                    string connectionString = database.ServiceConnectionString(true);

                    if (!string.IsNullOrWhiteSpace(connectionString))
                    {
                        s_windowsServiceClient = new WindowsServiceClient(connectionString);

                        securityProvider = s_currentPrincipal.Identity.Provider;
                        s_windowsServiceClient.Helper.Username = securityProvider.UserData.LoginID;
                        s_windowsServiceClient.Helper.Password = SecurityProviderUtility.EncryptPassword(securityProvider.Password);

                        remotingClient = s_windowsServiceClient.Helper.RemotingClient as TlsClient;

                        if ((object)remotingClient != null && (object)securityProvider.SecurePassword != null && securityProvider.SecurePassword.Length > 0)
                            remotingClient.NetworkCredential = new NetworkCredential(securityProvider.UserData.LoginID, securityProvider.SecurePassword);

                        s_windowsServiceClient.Helper.RemotingClient.MaxConnectionAttempts = -1;
                        s_windowsServiceClient.Helper.RemotingClient.ConnectionEstablished += RemotingClient_ConnectionEstablished;
                        s_windowsServiceClient.Helper.RemotingClient.ConnectionException += RemotingClient_ConnectionException;

                        ConnectAsync();
                    }
                }
                finally
                {
                    database.Dispose();
                }
            }
        }

        private static void RemotingClient_ConnectionEstablished(object sender, EventArgs e)
        {
            ServiceConnectionRefreshed(null, EventArgs.Empty);
        }

        private static void RemotingClient_ConnectionException(object sender, EventArgs<Exception> e)
        {
            bool logException = true;
            SocketException ex = e.Argument as SocketException;

            if ((object)ex == null && (object)e.Argument.InnerException != null)
                ex = e.Argument.InnerException as SocketException;

            if ((object)ex != null)
                logException = (ex.SocketErrorCode != SocketError.ConnectionRefused);

            if (logException)
                LogException(null, "Remoting Client Connect", e.Argument);
        }

        /// <summary>
        /// Displays a status message in the unobtrusive status message popup.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public static void DisplayStatusMessage(string message)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            TextBlock statusTextBlock;
            Popup statusPopup;

            if (dispatcher.Thread != Thread.CurrentThread)
            {
                dispatcher.BeginInvoke(new Action<string>(DisplayStatusMessage), message);
                return;
            }

            statusTextBlock = Application.Current.MainWindow.FindName("TextBlockResult") as TextBlock;
            statusPopup = Application.Current.MainWindow.FindName("PopupStatus") as Popup;

            if ((object)statusTextBlock != null && (object)statusPopup != null)
            {
                statusTextBlock.Text = message;
                statusPopup.IsOpen = true;
                statusPopup.InvalidateVisual();
            }
        }

        /// <summary>
        /// Hides the unobtrusive status message popup.
        /// </summary>
        public static void HideStatusMessage()
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            Popup statusPopup;

            if (dispatcher.Thread != Thread.CurrentThread)
            {
                dispatcher.BeginInvoke(new Action(HideStatusMessage));
                return;
            }

            statusPopup = Application.Current.MainWindow.FindName("PopupStatus") as Popup;

            if ((object)statusPopup != null)
                statusPopup.IsOpen = false;
        }

        /// <summary>
        /// Gets a message box to display message to users.
        /// </summary>
        public static Action<string, string, MessageBoxImage> Popup
        {
            get
            {
                return (Action<string, string, MessageBoxImage>)((message, caption, messageBoxImage) =>
                     MessageBox.Show(Application.Current.MainWindow, message, caption, MessageBoxButton.OK, messageBoxImage));
            }
        }

        /// <summary>
        /// Connects asynchronously to backend windows service.
        /// </summary>
        private static void ConnectAsync()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                try
                {
                    if ((object)s_windowsServiceClient != null && s_retryServiceConnection)
                    {
                        s_windowsServiceClient.Helper.Connect();

                        // If connection attempt failed with provided credentials, try once more with direct authentication
                        // but only when transport is secured
                        if (!s_windowsServiceClient.Authenticated && s_windowsServiceClient.Helper.RemotingClient is TlsClient)
                        {
                            ISecurityProvider provider = s_currentPrincipal.Identity.Provider;

                            if (provider.SecurePassword.Length > 0)
                            {
                                s_windowsServiceClient.Helper.Disconnect();
                                s_windowsServiceClient.Helper.SecurePassword = provider.SecurePassword;
                                s_windowsServiceClient.Helper.Connect();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(null, "RemoteConsoleConnection", ex);
                }
            });
        }

        /// <summary>
        /// Disconnects from backend windows service.
        /// </summary>
        public static void DisconnectWindowsServiceClient()
        {
            try
            {
                if (s_windowsServiceClient != null)
                {
                    s_windowsServiceClient.Dispose();
                    s_windowsServiceClient = null;
                }
            }
            catch (Exception ex)
            {
                LogException(null, "RemoteConsoleConnection", ex);
            }
        }

        /// <summary>
        /// Sends command to backend windows service via <see cref="WindowsServiceClient"/> object.
        /// </summary>
        /// <param name="command">command to be sent.</param>
        /// <returns>string, indicating success.</returns>
        public static string SendCommandToService(string command)
        {
            if (s_windowsServiceClient != null)
            {
                // Make sure requests are serialized
                lock (s_windowsServiceClient)
                {
                    if (s_windowsServiceClient.Helper.RemotingClient.CurrentState == ClientState.Connected)
                        s_windowsServiceClient.Helper.SendRequest(command);
                    else
                        throw new ApplicationException("Application is currently disconnected from service.");
                }

                return "Successfully sent " + command + " command.";
            }

            throw new ApplicationException("Application is currently disconnected from service.");
        }

        /// <summary>
        /// Retrieves a list of <see cref="StopBits"/>.
        /// </summary>
        /// <returns>Collection of <see cref="StopBits"/> as a <see cref="List{T}"/>.</returns>
        public static List<string> GetStopBits()
        {
            List<string> stopBitsList = new List<string>();

            foreach (string stopBit in Enum.GetNames(typeof(StopBits)))
                stopBitsList.Add(stopBit);

            return stopBitsList;
        }

        /// <summary>
        /// Retrieves a list of <see cref="Parity"/>.
        /// </summary>
        /// <returns>Collection of <see cref="Parity"/> as a <see cref="List{T}"/>.</returns>
        public static List<string> GetParities()
        {
            List<string> parityList = new List<string>();

            foreach (string parity in Enum.GetNames(typeof(Parity)))
                parityList.Add(parity);

            return parityList;
        }

        /// <summary>
        /// Converts xml element to datatype
        /// </summary>
        /// <param name="xmlValue"></param>
        /// <param name="xmlDataType"></param>
        /// <returns></returns>
        public static object ConvertValueToType(string xmlValue, string xmlDataType)
        {
            Type dataType = Type.GetType(xmlDataType);
            float value;

            if (float.TryParse(xmlValue, out value))
            {
                switch (xmlDataType)
                {
                    case "System.DateTime":
                        return new DateTime((long)value);
                    default:
                        return Convert.ChangeType(value, dataType);
                }
            }

            return "".ConvertToType(dataType);
        }

        /// <summary>
        /// Returns min and max point id values for a given node.
        /// </summary>
        /// <param name="connection"><see cref="AdoDataConnection"/> to connection to database.</param>
        /// <param name="nodeID">ID of the node to filter data.</param>
        /// <returns>KeyValuePair containing min and max point id.</returns>
        public static KeyValuePair<int?, int?> GetMinMaxPointIDs(AdoDataConnection connection, Guid nodeID)
        {
            KeyValuePair<int?, int?> minMaxPointIDs = new KeyValuePair<int?, int?>(1, 5000);
            bool createdConnection = false;

            try
            {
                createdConnection = DataModelBase.CreateConnection(ref connection);

                string query = connection.ParameterizedQueryString("SELECT MIN(PointID) AS MinPointID, MAX(PointID) AS MaxPointID FROM MeasurementDetail WHERE NodeID = {0}", "nodeID");
                DataTable results = connection.Connection.RetrieveData(connection.AdapterType, query, connection.Guid(nodeID));

                foreach (DataRow row in results.Rows)
                {
                    minMaxPointIDs = new KeyValuePair<int?, int?>(row.ConvertNullableField<int>("MinPointID"), row.ConvertNullableField<int>("MaxPointID"));
                }
            }
            finally
            {
                if (createdConnection && (object)connection != null)
                    connection.Dispose();
            }

            return minMaxPointIDs;
        }

        /// <summary>
        /// Attempts to retrieve current header text from user control group box host.
        /// </summary>
        /// <param name="defaultText">Default text to display.</param>
        /// <returns>Current header text from user control group box host, if possible; otherwise, <paramref name="defaultText"/>.</returns>
        public static string GetHeaderText(string defaultText)
        {
            if ((object)s_currentPage != null && (object)s_currentPage.Value != null)
                defaultText = s_currentPage.Value.Item1.ToNonNullNorWhiteSpace(defaultText);

            return defaultText;
        }

        /// <summary>
        /// Logs an event to the Windows event log.
        /// </summary>
        /// <param name="message">The message to write to the log.</param>
        /// <param name="eventID">The application-specific identifier for the event.</param>
        /// <param name="eventType">The type of the event.</param>
        /// <remarks>This will also send a command to log the event on the remote machine if needed.</remarks>
        public static void LogEvent(string message, int eventID, EventLogEntryType eventType = EventLogEntryType.Information)
        {
            try
            {
                ISecurityProvider securityProvider = s_currentPrincipal.Identity.Provider;
                string applicationName;

                if ((object)securityProvider != null)
                    applicationName = securityProvider.ApplicationName;
                else
                    applicationName = FilePath.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

                EventLog.WriteEntry(applicationName, message, eventType, eventID);

                // Queue event for remote logging
                ThreadPool.QueueUserWorkItem(LogEventRemotely, new Tuple<string, EventLogEntryType, int>(message, eventType, eventID));
            }
            catch (Exception ex)
            {
                LogException(null, "LogEvent", new InvalidOperationException(string.Format("Failed to write message \"{0}\" to event log: {1}", message, ex.Message), ex));
            }
        }

        // Send event to be logged on remote machine if manager is not running on local machine
        private static void LogEventRemotely(object state)
        {
            // Make sure service client and remoting client are defined
            if ((object)s_windowsServiceClient != null && (object)s_windowsServiceClient.Helper != null && (object)s_windowsServiceClient.Helper.RemotingClient != null)
            {
                try
                {
                    // Client base may be a normal TCP client or a TLS client - so we check for this
                    ClientBase client = s_windowsServiceClient.Helper.RemotingClient;
                    TlsClient remotingClient = client as TlsClient;
                    string remotingAddress = null;

                    // Get remote client address for remoting client (console) connection
                    if ((object)remotingClient != null)
                        remotingAddress = ((IPEndPoint)remotingClient.Client.RemoteEndPoint).Address.ToString();

                    // If this is not a local address - we will also send event to be logged on the server
                    if (!string.IsNullOrEmpty(remotingAddress) && !Communication.Transport.IsLocalAddress(remotingAddress))
                    {
                        string message;
                        EventLogEntryType eventType;
                        int eventID;

                        Tuple<string, EventLogEntryType, int> parameters = state as Tuple<string, EventLogEntryType, int>;

                        if ((object)parameters != null)
                        {
                            message = parameters.Item1;
                            eventType = parameters.Item2;
                            eventID = parameters.Item3;

                            string command = string.Format("LogEvent -Message=\"{0}\" -Type={1} -ID={2}", message.Replace('"', '\''), eventType, eventID);
                            SendCommandToService(command);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogException(null, "RemoteLogEvent", new InvalidOperationException(string.Format("Failed to send message to remote event log: {0}", ex.Message), ex));
                }
            }
        }

        /// <summary>
        /// Stores exception in the database
        /// </summary>
        /// <param name="connection"><see cref="AdoDataConnection"/> object to connect to database</param>
        /// <param name="source">Source of exception</param>
        /// <param name="ex">Exception to be logged</param>
        public static void LogException(AdoDataConnection connection, string source, Exception ex)
        {
            bool createdConnection = false;
            try
            {
                string query;

                createdConnection = DataModelBase.CreateConnection(ref connection);
                query = connection.ParameterizedQueryString("INSERT INTO ErrorLog (Source, Message, Detail) VALUES ({0}, {1}, {2})", "source", "message", "detail");
                connection.Connection.ExecuteNonQuery(query, DataModelBase.DefaultTimeout, source, ex.Message, ex.ToString());
            }
            catch
            {
                // Do nothing.  Don't worry about it
            }
            finally
            {
                if (createdConnection && connection != null)
                    connection.Dispose();
            }
        }

        /// <summary>
        /// Loads provided user control into the frame control inside main window.
        /// </summary>
        /// <param name="title">Title of the user control to be loaded.</param>
        /// <param name="userControlType">The type of the user control to be loaded.</param>
        /// <param name="constructorArgs">Parameters for the constructor of the user control.</param>
        public static UserControl LoadUserControl(string title, Type userControlType, params object[] constructorArgs)
        {
            Panel panel = Application.Current.MainWindow.FindName("UserControlPanel") as Panel;
            UserControl userControl;
            bool canGoForward;
            bool canGoBack;

            GroupBox groupBox;
            Run run;
            TextBlock txt;

            if ((object)panel != null)
            {
                userControl = Activator.CreateInstance(userControlType, constructorArgs) as UserControl;

                if ((object)userControl != null)
                {
                    // If no page history exists yet, create it
                    if ((object)s_pageHistory == null)
                        s_pageHistory = new LinkedList<Tuple<string, Type, object[]>>();

                    // Display the user control
                    panel.Children.Clear();
                    panel.Children.Add(userControl);

                    if ((object)s_currentPage != null)
                    {
                        canGoForward = CanGoForward;
                        canGoBack = CanGoBack;

                        // Add the new page after the current
                        s_pageHistory.AddAfter(s_currentPage, Tuple.Create(title, userControlType, constructorArgs));
                        s_currentPage = s_currentPage.Next;

                        // Truncate the list to the page that was just added
                        while (s_pageHistory.Last != s_currentPage)
                            s_pageHistory.RemoveLast();

                        // Shorten the history to the maximum number of pages
                        while (s_pageHistory.Count > MaxPageHistory)
                            s_pageHistory.RemoveFirst();

                        // Determine if flags to indicate whether
                        // we can go forward or back have changed
                        if (canGoForward)
                            OnCanGoForwardChanged();

                        if (!canGoBack)
                            OnCanGoBackChanged();
                    }
                    else
                    {
                        // Add this page as the first page in history
                        s_pageHistory.AddFirst(Tuple.Create(title, userControlType, constructorArgs));
                        s_currentPage = s_pageHistory.First;
                    }

                    // Set the header on the group box to the title passed into this method
                    groupBox = Application.Current.MainWindow.FindName("UserControlGroupBox") as GroupBox;

                    if ((object)groupBox != null)
                    {
                        run = new Run();
                        run.FontWeight = FontWeights.Bold;
                        run.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        run.Text = title;

                        txt = new TextBlock();
                        txt.Padding = new Thickness(5.0);
                        txt.Inlines.Add(run);

                        groupBox.Header = txt;
                    }

                    return userControl;
                }
            }

            return null;
        }

        /// <summary>
        /// Goes forward to the next user control in history.
        /// </summary>
        public static void GoForward()
        {
            Panel panel = Application.Current.MainWindow.FindName("UserControlPanel") as Panel;
            UserControl userControl;
            bool canGoBack;

            GroupBox groupBox;
            Run run;
            TextBlock txt;

            if (CanGoForward && (object)panel != null)
            {
                canGoBack = CanGoBack;

                // Get the next page and instantiate the user control
                s_currentPage = s_currentPage.Next;

                if ((object)s_currentPage == null)
                    userControl = null;
                else
                    userControl = Activator.CreateInstance(s_currentPage.Value.Item2, s_currentPage.Value.Item3) as UserControl;

                if ((object)userControl != null)
                {
                    // Display the user control
                    panel.Children.Clear();
                    panel.Children.Add(userControl);

                    // Determine if flags to indicate whether
                    // we can go forward or back have changed
                    if (!CanGoForward)
                        OnCanGoForwardChanged();

                    if (!canGoBack)
                        OnCanGoBackChanged();

                    // Set the header on the group box to the title of the user control
                    groupBox = Application.Current.MainWindow.FindName("UserControlGroupBox") as GroupBox;

                    if ((object)groupBox != null)
                    {
                        run = new Run();
                        run.FontWeight = FontWeights.Bold;
                        run.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        run.Text = s_currentPage.Value.Item1;

                        txt = new TextBlock();
                        txt.Padding = new Thickness(5.0);
                        txt.Inlines.Add(run);

                        groupBox.Header = txt;
                    }
                }
            }
        }

        /// <summary>
        /// Goes back to the previous user control in history.
        /// </summary>
        public static void GoBack()
        {
            Panel panel = Application.Current.MainWindow.FindName("UserControlPanel") as Panel;
            UserControl userControl;
            bool canGoForward;

            GroupBox groupBox;
            Run run;
            TextBlock txt;

            if (CanGoBack && (object)panel != null)
            {
                canGoForward = CanGoForward;

                // Get the previous page and instantiate the user control
                s_currentPage = s_currentPage.Previous;

                if ((object)s_currentPage == null)
                    userControl = null;
                else
                    userControl = Activator.CreateInstance(s_currentPage.Value.Item2, s_currentPage.Value.Item3) as UserControl;

                if ((object)userControl != null)
                {
                    // Display the user control
                    panel.Children.Clear();
                    panel.Children.Add(userControl);

                    // Determine if flags to indicate whether
                    // we can go forward or back have changed
                    if (!canGoForward)
                        OnCanGoForwardChanged();

                    if (!CanGoBack)
                        OnCanGoBackChanged();

                    // Set the header on the group box to the title of the user control
                    groupBox = Application.Current.MainWindow.FindName("UserControlGroupBox") as GroupBox;

                    if ((object)groupBox != null)
                    {
                        run = new Run();
                        run.FontWeight = FontWeights.Bold;
                        run.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                        run.Text = s_currentPage.Value.Item1;

                        txt = new TextBlock();
                        txt.Padding = new Thickness(5.0);
                        txt.Inlines.Add(run);

                        groupBox.Header = txt;
                    }
                }
            }
        }

        private static string GetCurrentUserPassword()
        {
            SecurityPrincipal currentPrincipal = s_currentPrincipal;
            SecurityIdentity identity;
            ISecurityProvider provider;

            if ((object)currentPrincipal == null)
                return null;

            identity = currentPrincipal.Identity as SecurityIdentity;

            if ((object)identity == null)
                return null;

            provider = identity.Provider;

            if ((object)provider == null)
                return null;

            return provider.Password;
        }

        private static bool TryImpersonate(string loginID, string password, out WindowsImpersonationContext impersonationContext)
        {
            string domain;
            string username;
            string[] splitLoginID;

            try
            {
                splitLoginID = loginID.Split('\\');

                if (splitLoginID.Length == 2)
                {
                    domain = splitLoginID[0];
                    username = splitLoginID[1];
                    impersonationContext = UserInfo.ImpersonateUser(domain, username, password);

                    return true;
                }
            }
            catch (InvalidOperationException)
            {
            }

            impersonationContext = null;

            return false;
        }

        private static void OnCurrentPrincipalRefreshed()
        {
            if ((object)CurrentPrincipalRefreshed != null)
                CurrentPrincipalRefreshed(null, EventArgs.Empty);

            OnCanGoForwardChanged();
            OnCanGoBackChanged();
        }

        private static void OnCanGoForwardChanged()
        {
            if ((object)CanGoForwardChanged != null)
                CanGoForwardChanged(null, EventArgs.Empty);
        }

        private static void OnCanGoBackChanged()
        {
            if ((object)CanGoBackChanged != null)
                CanGoBackChanged(null, EventArgs.Empty);
        }

        #endregion
    }
}
