﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace Dnn.PersonaBar.Extensions.Components.Editors
{
    using System;

    using Dnn.PersonaBar.Extensions.Components.Dto;
    using Dnn.PersonaBar.Extensions.Components.Dto.Editors;
    using DotNetNuke.Abstractions;
    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Instrumentation;
    using DotNetNuke.Services.Installer.Packages;
    using DotNetNuke.Services.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public class CoreLanguagePackageEditor : IPackageEditor
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(JsLibraryPackageEditor));

        public CoreLanguagePackageEditor()
        {
            this.NavigationManager = Globals.DependencyProvider.GetRequiredService<INavigationManager>();
        }

        protected INavigationManager NavigationManager { get; }

        public PackageInfoDto GetPackageDetail(int portalId, PackageInfo package)
        {
            var languagePack = LanguagePackController.GetLanguagePackByPackage(package.PackageID);
            var languagesTab = TabController.GetTabByTabPath(portalId, "//Admin//Languages", Null.NullString);

            var detail = new CoreLanguagePackageDetailDto(portalId, package)
            {
                Locales = Utility.GetAllLanguagesList(),
                LanguageId = languagePack.LanguageID,
                EditUrlFormat = this.NavigationManager.NavigateURL(languagesTab, "", "Locale={0}")
            };

            if (languagePack.PackageType == LanguagePackType.Extension)
            {
                //Get all the packages but only bind to combo if not a language package
                detail.Packages = Utility.GetAllPackagesListExceptLangPacks();
            }

            return detail;
        }

        public bool SavePackageSettings(PackageSettingsDto packageSettings, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                string value;
                var changed = false;
                var languagePack = LanguagePackController.GetLanguagePackByPackage(packageSettings.PackageId);
                if (packageSettings.EditorActions.TryGetValue("languageId", out value)
                    && !string.IsNullOrEmpty(value) && value != languagePack.LanguageID.ToString())
                {
                    languagePack.LanguageID = Convert.ToInt32(value);
                    changed = true;
                }

                if (changed) LanguagePackController.SaveLanguagePack(languagePack);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}