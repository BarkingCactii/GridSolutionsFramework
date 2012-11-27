﻿//******************************************************************************************************
//  UserDataCache.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  04/06/2011 - J. Ritchie Carroll
//       Generated original version of source code.
//  04/14/2011 - Pinal C. Patel
//       Updated to use new serialization and deserialization methods in GSF.Serialization class.
//  06/10/2011 - Pinal C. Patel
//       Renamed RetryDelayInterval and MaximumRetryAttempts settings persisted to the config file 
//       to CacheRetryDelayInterval and CacheMaximumRetryAttempts for clarity.
//  08/12/2011 - J. Ritchie Carroll
//       Modifed static GetCurrentCache to accept settings category of host security provider
//       implementation in case the category has been changed from the default value by the consumer.
//  08/16/2011 - Pinal C. Patel
//       Modified GetCurrentCache() to just set the FileName property and not the RetryDelayInterval, 
//       MaximumRetryAttempts, ReloadOnChange and AutoSave properties.
//  10/8/2012 - Danyelle Gilliam
//        Modified Header
//
//******************************************************************************************************

using GSF.Collections;
using GSF.IO;
using GSF.Security.Cryptography;
using GSF.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace GSF.Security
{
    /// <summary>
    /// Represents a secured interprocess cache for a <see cref="Dictionary{T1,T2}"/> of serialized <see cref="UserData"/>.
    /// </summary>
    public class UserDataCache : InterprocessCache
    {
        #region [ Members ]

        // Constants

        // Default key and initialization vector cache file name
        private const string DefaultCacheFileName = "UserDataCache.bin";

        // Fields
        private Dictionary<string, UserData> m_userDataTable;   // Internal dictionary of serialized user data
        private object m_userDataTableLock;                     // Lock object
        private int m_providerID;                               // Unique provider ID used to distinguish cached user data that may be different based on provider

        #endregion

        /// <summary>
        /// Creates a new instance of the <see cref="UserDataCache"/>.
        /// </summary>
        /// <param name="providerID">Unique provider ID used to distinguish cached user data that may be different based on provider.</param>
        public UserDataCache(int providerID = LdapSecurityProvider.ProviderID)
            : this(providerID, InterprocessReaderWriterLock.DefaultMaximumConcurrentLocks)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="UserDataCache"/> with the specified number of <paramref name="maximumConcurrentLocks"/>.
        /// </summary>
        /// <param name="maximumConcurrentLocks">Maximum concurrent reader locks to allow.</param>
        /// <param name="providerID">Unique provider ID used to distinguish cached user data that may be different based on provider.</param>
        public UserDataCache(int providerID, int maximumConcurrentLocks)
            : base(maximumConcurrentLocks)
        {
            m_providerID = providerID;
            m_userDataTable = new Dictionary<string, UserData>();
            m_userDataTableLock = new object();
        }

        #region [ Properties ]

        /// <summary>
        /// Gets or sets <see cref="UserData"/> for given <paramref name="loginID"/>.
        /// </summary>
        /// <param name="loginID">Login ID of associated <see cref="UserData"/> to load or save.</param>
        /// <returns>Reference to <see cref="UserData"/> for given <paramref name="loginID"/> if found; otherwise <c>null</c>.</returns>
        public UserData this[string loginID]
        {
            get
            {
                UserData userData;
                TryGetUserData(loginID, out userData);
                return userData;
            }
            set
            {
                SaveUserData(loginID, value);
            }
        }

        /// <summary>
        /// Gets ot sets unique provider ID used to distinguish cached user data that may be different based on provider.
        /// </summary>
        public int ProviderID
        {
            get
            {
                return m_providerID;
            }
            set
            {
                m_providerID = value;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Attempts to retrieve <see cref="UserData"/> for given <paramref name="loginID"/>.
        /// </summary>
        /// <param name="loginID">Login ID of associated <see cref="UserData"/> to retrieve.</param>
        /// <param name="userData">Reference to <see cref="UserData"/> object to populate if found.</param>
        /// <returns><c>true</c> if <see cref="UserData"/> for given <paramref name="loginID"/> was retrieved; otherwise <c>false</c>.</returns>
        public bool TryGetUserData(string loginID, out UserData userData)
        {
            string hash = HashLoginID(loginID);
            bool result;

            // We wait until the cache is loaded before attempting to access it
            WaitForDataReady();

            // Wait for thread level lock on user data table
            lock (m_userDataTableLock)
            {
                // Attempt to lookup persisted user data based on hash of login ID
                result = m_userDataTable.TryGetValue(hash, out userData);
            }

            return result;
        }

        /// <summary>
        /// Serializes the <paramref name="userData"/> for the given <paramref name="loginID"/> into the <see cref="UserDataCache"/>.
        /// </summary>
        /// <param name="loginID">Login ID of associated <see cref="UserData"/> to retrieve.</param>
        /// <param name="userData">Reference to <see cref="UserData"/> object to serialize into <see cref="UserDataCache"/>.</param>
        /// <remarks>
        /// <para>
        /// This will add an entry into the user data cache for <paramref name="loginID"/> if it doesn't exist;
        /// otherwise existing entry will be updated.
        /// </para>
        /// <para>
        /// Updates are automatically queued up for serialization so user does not need to call <see cref="Save"/>.
        /// </para>
        /// </remarks>
        public void SaveUserData(string loginID, UserData userData)
        {
            string hash = HashLoginID(loginID);

            // We wait until the cache is loaded before attempting to access it
            WaitForDataReady();

            // Wait for thread level lock on user data table
            lock (m_userDataTableLock)
            {
                // Assign new user information to user data table
                m_userDataTable[hash] = userData;
            }

            // Queue up a serialization for this new user information
            Save();
        }

        /// <summary>
        /// Initiates interprocess synchronized save of user data cache.
        /// </summary>
        public override void Save()
        {
            byte[] serializedUserDataTable;

            // Wait for thread level lock on key table
            lock (m_userDataTableLock)
            {
                serializedUserDataTable = Serialization.Serialize(m_userDataTable, SerializationFormat.Binary);
            }

            // File data is the serialized user data table, assigmnent will initiate auto-save if needed
            FileData = serializedUserDataTable;
        }

        /// <summary>
        /// Handles serialization of file to disk; virtual method allows customization (e.g., pre-save encryption and/or data merge).
        /// </summary>
        /// <param name="fileStream"><see cref="FileStream"/> used to serialize data.</param>
        /// <param name="fileData">File data to be serialized.</param>
        /// <remarks>
        /// Consumers overriding this method should not directly call <see cref="InterprocessCache.FileData"/> property to avoid potential dead-locks.
        /// </remarks>
        protected override void SaveFileData(FileStream fileStream, byte[] fileData)
        {
            // Encrypt data local to this machine (this way user cannot copy user data cache to another machine)
            base.SaveFileData(fileStream, ProtectedData.Protect(fileData, null, DataProtectionScope.LocalMachine));
        }

        /// <summary>
        /// Handles deserialization of file from disk; virtual method allows customization (e.g., pre-load decryption and/or data merge).
        /// </summary>
        /// <param name="fileStream"><see cref="FileStream"/> used to deserialize data.</param>
        /// <returns>Deserialized file data.</returns>
        /// <remarks>
        /// Consumers overriding this method should not directly call <see cref="InterprocessCache.FileData"/> property to avoid potential dead-locks.
        /// </remarks>
        protected override byte[] LoadFileData(FileStream fileStream)
        {
            // Decrypt data that was encrypted local to this machine
            byte[] serializedUserDataTable = ProtectedData.Unprotect(fileStream.ReadStream(), null, DataProtectionScope.LocalMachine);
            Dictionary<string, UserData> userDataTable = Serialization.Deserialize<Dictionary<string, UserData>>(serializedUserDataTable, SerializationFormat.Binary);

            // Wait for thread level lock on user data table
            lock (m_userDataTableLock)
            {
                // Merge new and existing key tables since new user information may have been queued for serialization, but not saved yet
                m_userDataTable = userDataTable.Merge(m_userDataTable);
            }

            return serializedUserDataTable;
        }

        /// <summary>
        /// Calculates the hash of the <paramref name="loginID"/> used as the key for the user data cache.
        /// </summary>
        /// <param name="loginID">Login ID to hash.</param>
        /// <returns>The Base64 encoded calculated SHA-2 hash of the <paramref name="loginID"/> used as the key for the user data cache.</returns>
        /// <remarks>
        /// For added security, a hash of the <paramref name="loginID"/> is used as the key for <see cref="UserData"/> in the
        /// user data cache instead of the actual <paramref name="loginID"/>. This method allows the
        /// consumer to properly calculate this hash when directly using the user data cache.
        /// </remarks>
        protected string HashLoginID(string loginID)
        {
            return Cipher.GetPasswordHash(loginID.ToLower(), m_providerID);
        }

        // Waits until the cache is loaded before attempting to access it
        private void WaitForDataReady()
        {
            try
            {
                // Just wrapping this method to provide a more detailed exception message if there is an issue loading cache
                WaitForLoad();
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("User data access failure: timeout while attempting to load user data cache.", ex);
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        /// <summary>
        /// Loads the <see cref="UserDataCache"/> for the current local user.
        /// </summary>
        /// <param name="providerID">Unique security provider ID used to distinguish cached user data that may be different based on provider.</param>
        /// <returns>Loaded instance of the <see cref="UserDataCache"/>.</returns>
        public static UserDataCache GetCurrentCache(int providerID)
        {
            // By default user data cache is stored in a path where user will have rights
            UserDataCache userDataCache;
            string userCacheFolder = FilePath.GetApplicationDataFolder();
            string userCacheFileName = Path.Combine(userCacheFolder, FilePath.GetFileName(DefaultCacheFileName));

            // Make sure user directory exists
            if (!Directory.Exists(userCacheFolder))
                Directory.CreateDirectory(userCacheFolder);

            // Initialize user data cache for current local user
            userDataCache = new UserDataCache(providerID);
            userDataCache.FileName = userCacheFileName;

            return userDataCache;
        }

        #endregion

    }
}
