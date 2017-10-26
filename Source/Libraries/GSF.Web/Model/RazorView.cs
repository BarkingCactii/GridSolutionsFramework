﻿//******************************************************************************************************
//  RazorView.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/13/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using GSF.Collections;
using GSF.Configuration;
using GSF.Data;
using GSF.Web.Hosting;
using GSF.Web.Security;
using Microsoft.Owin;
using RazorEngine.Templating;
using Timer = System.Timers.Timer;

// ReSharper disable StaticMemberInGenericType
namespace GSF.Web.Model
{
    /// <summary>
    /// Defines a view class for data context based Razor template implementations.
    /// </summary>
    public class RazorView
    {
        #region [ Members ]

        // Constants

        /// <summary>
        /// Default value for <see cref="SessionTimeout"/>.
        /// </summary>
        public const double DefaultSessionTimeout = 20.0D;

        /// <summary>
        /// Default value for <see cref="SessionMonitorInterval"/>.
        /// </summary>
        public const double DefaultSessionMonitorInterval = 60000.0D;

        // Fields
        private readonly IRazorEngine m_razorEngine;
        private readonly DynamicViewBag m_viewBag = new DynamicViewBag();
        private Dictionary<string, string> m_parameters;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="RazorView"/>.
        /// </summary>
        /// <param name="razorEngine"><see cref="IRazorEngine"/> instance to use.</param>
        /// <param name="templateName">Name of template file, typically a .cshtml or .vbhtml file.</param>
        /// <param name="exceptionHandler">Delegate to handle exceptions, if any.</param>
        public RazorView(IRazorEngine razorEngine, string templateName, Action<Exception> exceptionHandler = null) : this(razorEngine, templateName, null, null, null, null, null, exceptionHandler)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RazorView"/>.
        /// </summary>
        /// <param name="razorEngine"><see cref="IRazorEngine"/> instance to use.</param>
        /// <param name="templateName">Name of template file, typically a .cshtml or .vbhtml file.</param>
        /// <param name="model">Reference to model to use when rendering template.</param>
        /// <param name="modelType">Type of <paramref name="model"/>.</param>
        /// <param name="pagedViewModelDataType">Type of data class for views based on paged view model, if any.</param>
        /// <param name="pagedViewModelHubType">Type of SignalR hub for views based on paged view model, if any.</param>
        /// <param name="database"><see cref="AdoDataConnection"/> to use, if any.</param>
        /// <param name="exceptionHandler">Delegate to handle exceptions, if any.</param>
        /// <param name="webServerOptions">Web server options currently in use.</param>
        public RazorView(IRazorEngine razorEngine, string templateName, object model = null, Type modelType = null, Type pagedViewModelDataType = null, Type pagedViewModelHubType = null, AdoDataConnection database = null, Action<Exception> exceptionHandler = null, ReadonlyWebServerOptions webServerOptions = null)
        {
            m_razorEngine = razorEngine;
            TemplateName = templateName;
            Model = model;
            ModelType = modelType;
            PagedViewModelDataType = pagedViewModelDataType;
            PagedViewModelHubType = pagedViewModelHubType;
            Database = database;
            ExceptionHandler = exceptionHandler;
            WebServerOptions = webServerOptions;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets name of template file.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets reference to model to use when rendering template.
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Gets or sets type of <see cref="Model"/>.
        /// </summary>
        public Type ModelType { get; set; }

        /// <summary>
        /// Gets or sets type of data model for views based on paged view model.
        /// </summary>
        public Type PagedViewModelDataType { get; set; }

        /// <summary>
        /// Gets or sets type of SignalR hub for views based on paged view model.
        /// </summary>
        public Type PagedViewModelHubType { get; set; }

        /// <summary>
        /// Gets reference to view bag used when rendering template.
        /// </summary>
        public dynamic ViewBag => m_viewBag;

        /// <summary>
        /// Gets query string parameter specified by <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Name of query string parameter to retrieve.</param>
        /// <returns>Query string parameter specified by <paramref name="key"/>.</returns>
        public string this[string key] => Parameters[key];

        /// <summary>
        /// Gets a dictionary of query string parameters passed to rendered view.
        /// </summary>
        public Dictionary<string, string> Parameters
        {
            get
            {
                if ((object)m_parameters == null)
                {
                    HttpRequestMessage request = ViewBag.Request;
                    m_parameters = HttpUtility.ParseQueryString(request.RequestUri.Query).ToDictionary();
                }

                return m_parameters;
            }
        }

        /// <summary>
        /// Gets reference to <see cref="IRazorEngine"/> used by this <see cref="RazorView"/>.
        /// </summary>
        public IRazorEngine RazorEngine => m_razorEngine;

        /// <summary>
        /// Gets or sets database connection to provide to <see cref="DataContext"/>, if any.
        /// </summary>
        public AdoDataConnection Database { get; set; }

        /// <summary>
        /// Gets or sets delegate used to handle exceptions.
        /// </summary>
        public Action<Exception> ExceptionHandler { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IRazorEngine"/> to use for the <see cref="DataContext"/> provided to the view; defaults to <c>null</c>.
        /// </summary>
        /// <remarks>
        /// This should typically not be changed. When value is <c>null</c>, the data context will use the default
        /// <see cref="RazorEngine{TLanguage}"/> instance for <see cref="CSharpEmbeddedResource"/> which is used
        /// to generate template based HTML input fields for a view. The default HTML input templates are defined
        /// as embedded resources in GSF.Web.
        /// </remarks>
        public IRazorEngine DataContextEngine { get; set; }

        /// <summary>
        /// Gets or sets the web server options currently in use.
        /// </summary>
        public ReadonlyWebServerOptions WebServerOptions { get; set; }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Compiles and executes view template.
        /// </summary>
        /// <returns>Rendered result.</returns>
        public string Execute()
        {
            using (DataContext dataContext = new DataContext(Database, razorEngine: DataContextEngine, exceptionHandler: ExceptionHandler))
            {
                if ((object)PagedViewModelDataType != null && (object)PagedViewModelHubType != null)
                    dataContext.ConfigureView(PagedViewModelDataType, PagedViewModelHubType, null as string, m_viewBag);

                m_viewBag.AddValue("DataContext", dataContext);
                return m_razorEngine.RunCompile(TemplateName, ModelType, Model, m_viewBag);
            }
        }

        /// <summary>
        /// Compiles and executes view template for specified request message and post data.
        /// </summary>
        /// <param name="request">HTTP request message.</param>
        /// <param name="response">HTTP response message.</param>
        /// <param name="isPost"><c>true</c>if <paramref name="request"/> is HTTP post; otherwise, <c>false</c>.</param>
        /// <returns>Rendered result.</returns>
        public string Execute(HttpRequestMessage request, HttpResponseMessage response, bool isPost)
        {
            using (DataContext dataContext = new DataContext(Database, razorEngine: DataContextEngine, exceptionHandler: ExceptionHandler))
            {
                // Need to add the security principal to the view bag before configuring the view
                m_viewBag.AddValue("SecurityPrincipal", request.GetRequestContext().Principal);

                if ((object)PagedViewModelDataType != null && (object)PagedViewModelHubType != null)
                    dataContext.ConfigureView(PagedViewModelDataType, PagedViewModelHubType, request, m_viewBag);

                m_viewBag.AddValue("Application", s_applicationCache);
                m_viewBag.AddValue("DataContext", dataContext);
                m_viewBag.AddValue("Request", request);
                m_viewBag.AddValue("Response", response);
                m_viewBag.AddValue("IsPost", isPost);
                m_viewBag.AddValue("WebServerOptions", WebServerOptions);
                m_viewBag.AddValue("AuthenticationOptions", ReadonlyAuthenticationOptions.GetAuthenticationOptions(request));

                // See if a client session identifier has been defined for this execution request
                Guid sessionID;
                
                if (SessionHandler.TryGetSessionID(request, WebServerOptions?.SessionToken, out sessionID))
                {
                    Tuple<DynamicViewBag, Ticks> session;
                    DynamicViewBag sessionState;

                    // Make sure session state is restored for this request, creating it if necessary
                    if (!s_sessionCache.TryGetValue(sessionID, out session) || (object)session?.Item1 == null)
                        sessionState = new DynamicViewBag();
                    else
                        sessionState = session.Item1;

                    // Update the last access time for the session state
                    s_sessionCache[sessionID] = new Tuple<DynamicViewBag, Ticks>(sessionState, DateTime.UtcNow.Ticks);
                    s_sessionCacheMonitor.Enabled = true;

                    // Provide session state to view
                    m_viewBag.AddValue("Session", sessionState);
                }

                return m_razorEngine.RunCompile(TemplateName, ModelType, Model, m_viewBag);
            }
        }

        /// <summary>
        /// Asynchronously compiles and executes view template for specified request message and post data.
        /// </summary>
        /// <param name="request">HTTP request message.</param>
        /// <param name="response">HTTP response message.</param>
        /// <param name="isPost"><c>true</c>if <paramref name="request"/> is HTTP post; otherwise, <c>false</c>.</param>
        /// <param name="cancellationToken">Propagates notification from client that operations should be canceled.</param>
        /// <returns>Task that will provide rendered result.</returns>
        public Task<string> ExecuteAsync(HttpRequestMessage request, HttpResponseMessage response, bool isPost, CancellationToken cancellationToken)
        {
            return Task.Run(() => Execute(request, response, isPost), cancellationToken);
        }

        #endregion

        #region [ Static ]

        // Static Events

        /// <summary>
        /// Raised when a client session is being expired.
        /// </summary>
        public static event EventHandler<EventArgs<Guid, DynamicViewBag>> SessionExpired;

        // Static Fields
        private static readonly DynamicViewBag s_applicationCache;
        private static readonly Timer s_sessionCacheMonitor;
        private static readonly ConcurrentDictionary<Guid, Tuple<DynamicViewBag, Ticks>> s_sessionCache;

        // Static Constructor
        static RazorView()
        {
            CategorizedSettingsElementCollection settings = ConfigurationFile.Current.Settings["systemSettings"];
            settings.Add("SessionTimeout", DefaultSessionTimeout, "The timeout, in minutes, for which inactive client sessions will be expired and removed from the cache.");
            settings.Add("SessionMonitorInterval", DefaultSessionMonitorInterval, "The interval, in milliseconds, over which the client session cache will be evaluated for expired sessions.");

            SessionTimeout = settings["SessionTimeout"].ValueAs(DefaultSessionTimeout);
            SessionMonitorInterval = settings["SessionMonitorInterval"].ValueAs(DefaultSessionMonitorInterval);

            s_applicationCache = new DynamicViewBag();
            s_sessionCacheMonitor = new Timer(SessionMonitorInterval);
            s_sessionCacheMonitor.Elapsed += s_sessionCacheMonitor_Elapsed;
            s_sessionCacheMonitor.Enabled = false;
            s_sessionCache = new ConcurrentDictionary<Guid, Tuple<DynamicViewBag, Ticks>>();
        }

        // Static Properties

        /// <summary>
        /// Gets timeout, in minutes, for which inactive client sessions will be expired and removed from the cache.
        /// </summary>
        public static double SessionTimeout { get; }

        /// <summary>
        /// Gets interval, in milliseconds, over which the client session cache will be evaluated for expired sessions.
        /// </summary>
        public static double SessionMonitorInterval { get; }

        // Static Methods

        /// <summary>
        /// Clears any cached session for the specified <paramref name="sessionID"/>.
        /// </summary>
        /// <param name="sessionID">Identifier of session to clear.</param>
        /// <returns><c>true</c> if session was found and cleared; otherwise, <c>false</c>.</returns>
        public static bool ClearSessionCache(Guid sessionID)
        {
            Tuple<DynamicViewBag, Ticks> session;

            if (s_sessionCache.TryRemove(sessionID, out session))
            {
                SessionExpired?.Invoke(null, new EventArgs<Guid, DynamicViewBag>(sessionID, session.Item1));
                return true;
            }

            return false;
        }

        private static void s_sessionCacheMonitor_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check for expired client sessions
            foreach (KeyValuePair<Guid, Tuple<DynamicViewBag, Ticks>> clientSession in s_sessionCache)
            {
                Ticks lastAccessTime = clientSession.Value.Item2;

                if ((DateTime.UtcNow.Ticks - lastAccessTime).ToMinutes() > SessionTimeout)
                    ClearSessionCache(clientSession.Key);
             }

            s_sessionCacheMonitor.Enabled = s_sessionCache.Count > 0;
        }

        #endregion    
    }
}