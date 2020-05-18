using UnityEngine;
using UnityEngine.Serialization;
using System;

namespace OmiyaGames.Web
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="PlatformSpecificLink.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2018 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <list type="table">
    /// <listheader>
    /// <term>Revision</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.0.0-preview.1<br/>
    /// <strong>Date:</strong> 9/22/2016<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 5/17/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Converting the file to a package.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A struct that returns a link specific to a platform.
    /// Useful as a <see cref="SerializedField"/> set in the inspector.
    /// </summary>
    [System.Serializable]
    public struct PlatformSpecificLink
    {
        /// <summary>
        /// The platform a link belongs to.
        /// </summary>
        public enum SupportedPlatforms
        {
            /// <summary>
            /// Default website to link to.
            /// </summary>
            WebDefault      = 0,
            /// <summary>
            /// The native listing on iOS Appstore.
            /// </summary>
            NativeIos       = 1,
            /// <summary>
            /// The native listing on Google Play Store.
            /// </summary>
            NativeAndroid   = 2,
            /// <summary>
            /// The native listing on Amazon Appstore.
            /// </summary>
            [Obsolete]
            NativeAmazon    = 3,
            /// <summary>
            /// The native listing on Microsoft Store.
            /// </summary>
            NativeWin10     = 4,
            /// <summary>
            /// The listing on iOS Appstore website.
            /// </summary>
            WebiOS          = 5,
            /// <summary>
            /// The listing on Google Play Store website.
            /// </summary>
            WebAndroid      = 6,
            [Obsolete]
            /// <summary>
            /// The listing on Amazon Appstore website.
            /// </summary>
            WebAmazon       = 7,
            /// <summary>
            /// The listing on Microsoft Store website.
            /// </summary>
            WebWin10        = 8
        }

        /// <summary>
        /// The default link to this game's listing online.
        /// </summary>
        [Header("Web Links")]
        [Tooltip("The default link, in case the rest of the fields are not covered")]
        [FormerlySerializedAsAttribute("webLink")]
        [SerializeField]
        private string defaultWebLink;
#pragma warning disable 0414
        /// <summary>
        /// The native iOS Appstore link to this game's listing.
        /// </summary>
        [SerializeField]
        private string iosWebLink;
        /// <summary>
        /// The native Google Play Store link to this game's listing.
        /// </summary>
        [SerializeField]
        private string androidWebLink;
        /// <summary>
        /// The native Microsoft Store link to this game's listing.
        /// </summary>
        [SerializeField]
        private string windows10WebLink;

        /// <summary>
        /// The link to this game's listing on the iOS Appstore website.
        /// </summary>
        [Header("Native Store Links")]
        [FormerlySerializedAsAttribute("iosLink")]
        [SerializeField]
        private string iosNativeLink;
        /// <summary>
        /// The link to this game's listing on the Google Play Store website.
        /// </summary>
        [FormerlySerializedAsAttribute("androidLink")]
        [SerializeField]
        private string androidNativeLink;
        /// <summary>
        /// The link to this game's listing on the Microsoft Store website.
        /// </summary>
        [FormerlySerializedAsAttribute("windows10Link")]
        [SerializeField]
        private string windows10NativeLink;
#pragma warning restore 0414

        /// <summary>
        /// Gets what native app store this platform supports.
        /// </summary>
        public static SupportedPlatforms Platform
        {
            get
            {
#if UNITY_IOS
                return SupportedPlatforms.NativeIos;
#elif UNITY_ANDROID
                return SupportedPlatforms.NativeAndroid;
#elif UNITY_WSA
                return SupportedPlatforms.NativeWin10;
#else
                return SupportedPlatforms.WebDefault;
#endif
            }
        }

        /// <summary>
        /// Gets the default web link.
        /// </summary>
        /// <seealso cref="defaultWebLink"/>
        public string WebLink
        {
            get
            {
                return defaultWebLink;
            }
        }

        /// <summary>
        /// Gets the link to the native app store.
        /// </summary>
        /// <seealso cref="GetPlatformLink(SupportedPlatforms)"/>
        public string PlatformLink
        {
            get
            {
                // Return the URL
                return GetPlatformLink(Platform);
            }
        }

        /// <summary>
        /// Grabs a link for a specific platform.
        /// </summary>
        public string GetPlatformLink(SupportedPlatforms platform)
        {
            // Check the platform
            string returnUrl = null;
            switch(platform)
            {
                case SupportedPlatforms.NativeIos:
                    returnUrl = iosNativeLink;
                    break;
                case SupportedPlatforms.WebiOS:
                    returnUrl = iosWebLink;
                    break;
                case SupportedPlatforms.NativeAndroid:
                    returnUrl = androidNativeLink;
                    break;
                case SupportedPlatforms.WebAndroid:
                    returnUrl = androidWebLink;
                    break;
                case SupportedPlatforms.NativeWin10:
                    returnUrl = windows10NativeLink;
                    break;
                case SupportedPlatforms.WebWin10:
                    returnUrl = windows10WebLink;
                    break;
            }

            // Check if the string is empty
            if (string.IsNullOrEmpty(returnUrl) == true)
            {
                // Replace with a web URL
                returnUrl = WebLink;
            }

            // Return the URL
            return returnUrl;
        }
    }
}
