﻿//******************************************************************************************************
//  GrafanaDataService.cs - Gbtc
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
//  09/12/2016 - Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using GSF;
using GSF.Historian;
using GSF.Historian.DataServices;
using HistorianAdapters;
using Newtonsoft.Json;

namespace GrafanaAdapters
{
    /// <summary>
    /// Represents a REST based API for a simple JSON based Grafana data source for the openHistorian 1.0.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GrafanaDataService : DataService, IGrafanaDataService
    {
        #region [ Members ]

        // Nested Types
        private sealed class HistorianDataSource : GrafanaDataSourceBase
        {
            private readonly GrafanaDataService m_parent;
            private readonly long m_baseTicks;

            public HistorianDataSource(GrafanaDataService parent)
            {
                m_parent = parent;
                m_baseTicks = UnixTimeTag.BaseTicks.Value;
            }

            protected override IEnumerable<DataSourceValue> QueryDataSourceValues(DateTime startTime, DateTime stopTime, string interval, bool decimate, Dictionary<ulong, string> targetMap)
            {
                return m_parent.Archive.ReadData(targetMap.Keys.Select(pointID => (int)pointID), startTime, stopTime, false).Select(dataPoint => new DataSourceValue
                {
                    Target = targetMap[(ulong)dataPoint.HistorianID],
                    Value = dataPoint.Value,
                    Time = (dataPoint.Time.ToDateTime().Ticks - m_baseTicks) / (double)Ticks.PerMillisecond,
                    Flags = dataPoint.Quality.MeasurementQuality()
                });
            }
        }

        // Fields
        private readonly HistorianDataSource m_dataSource;
        private CancellationTokenSource m_cancellationSource;
        private bool m_disposed;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new instance of a <see cref="GrafanaDataService"/>.
        /// </summary>
        public GrafanaDataService()
        {
            m_dataSource = new HistorianDataSource(this);
            m_cancellationSource = new CancellationTokenSource();
            Endpoints = "http.rest://localhost:6057/api/grafana/";
            ServiceEnabled = false;

            // Make sure exceptions are reported in JSON format
            JsonFaultHandlingEnabled = true;
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets a boolean value that indicates whether the web service is currently enabled.
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;

                // Cancel any running queries if web service gets disabled
                if (!value)
                    Interlocked.Exchange(ref m_cancellationSource, new CancellationTokenSource()).Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IArchive"/> used by the web service for its data.
        /// </summary>
        public override IArchive Archive
        {
            get
            {
                return base.Archive;
            }
            set
            {
                if ((object)base.Archive != null)
                    base.Archive.MetadataUpdated -= Archive_MetadataUpdated;

                base.Archive = value;

                if ((object)base.Archive != null)
                    base.Archive.MetadataUpdated += Archive_MetadataUpdated;

                // Update data source metadata when an archive is defined, adapter should exist by then
                if ((object)m_dataSource.Metadata == null && Enabled)
                    Archive_MetadataUpdated(this, EventArgs.Empty);
            }
        }

        #endregion

        #region [ Methods ]

        private void Archive_MetadataUpdated(object sender, EventArgs e)
        {
            LocalOutputAdapter adapter;

            if (LocalOutputAdapter.Instances.TryGetValue(m_dataSource.InstanceName, out adapter))
                m_dataSource.Metadata = adapter.DataSource;

            TargetCaches.ResetAll();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="GrafanaDataService"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                        m_cancellationSource.Dispose();
                }
                finally
                {
                    m_disposed = true;          // Prevent duplicate dispose.
                    base.Dispose(disposing);    // Call base class Dispose().
                }
            }
        }

        /// <summary>
        /// Initializes the web service.
        /// </summary>
        public override void Initialize()
        {
            // Get historian instance name as assigned to settings category by data services host
            m_dataSource.InstanceName = SettingsCategory.Substring(0, SettingsCategory.IndexOf(GetType().Name, StringComparison.OrdinalIgnoreCase));

            base.Initialize();
        }

        /// <summary>
        /// Validates that openHistorian Grafana data source is responding as expected.
        /// </summary>
        public void TestDataSource()
        {
        }

        /// <summary>
        /// Queries openHistorian as a Grafana data source.
        /// </summary>
        /// <param name="request">Query request.</param>
        public async Task<List<TimeSeriesValues>> Query(QueryRequest request)
        {
            // Abort if services are not enabled
            if (!Enabled || (object)Archive == null)
                return null;

            return await m_dataSource.Query(request, m_cancellationSource.Token);
        }


        /// <summary>
        /// Queries openHistorian as a Grafana Metadata source.
        /// </summary>
        /// <param name="request">Query request.</param>
        public Task<string> GetMetadata(Target request)
        {
            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrWhiteSpace(request.target))
                    return string.Empty;

                DataTable table = new DataTable();
                DataRow[] rows = m_dataSource?.Metadata.Tables["ActiveMeasurements"].Select($"PointTag IN ({request.target})") ?? new DataRow[0];

                if (rows.Length > 0)
                    table = rows.CopyToDataTable();

                return JsonConvert.SerializeObject(table);
            });
        }

        /// <summary>
        /// Search openHistorian for a target.
        /// </summary>
        /// <param name="request">Search target.</param>
        public async Task<string[]> Search(Target request)
        {
            return await m_dataSource.Search(request);
        }

        /// <summary>
        /// Queries openHistorian for annotations in a time-range (e.g., Alarms).
        /// </summary>
        /// <param name="request">Annotation request.</param>
        public async Task<List<AnnotationResponse>> Annotations(AnnotationRequest request)
        {
            // Abort if services are not enabled
            if (!Enabled || (object)Archive == null)
                return null;

            return await m_dataSource.Annotations(request, m_cancellationSource.Token);
        }

        #endregion
    }
}