﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.EventQueue;
using DotNetNuke.Services.Installer.Dependencies;
using DotNetNuke.Services.Installer.Packages;

#endregion

namespace DotNetNuke.Services.Installer.Installers
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The PackageInstaller class is an Installer for Packages
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class PackageInstaller : ComponentInstallerBase
    {
		#region Private Members
	
        private readonly SortedList<int, ComponentInstallerBase> _componentInstallers = new SortedList<int, ComponentInstallerBase>();
        private PackageInfo _installedPackage;
        private EventMessage _eventMessage;
		
		#endregion

		#region Constructors
        
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// This Constructor creates a new PackageInstaller instance
        /// </summary>
        /// <param name="package">A PackageInfo instance</param>
        /// -----------------------------------------------------------------------------
        public PackageInstaller(PackageInfo package)
        {
            IsValid = true;
            DeleteFiles = Null.NullBoolean;
            Package = package;
            if (!string.IsNullOrEmpty(package.Manifest))
            {
				//Create an XPathDocument from the Xml
                var doc = new XPathDocument(new StringReader(package.Manifest));
                XPathNavigator nav = doc.CreateNavigator().SelectSingleNode("package");
                ReadComponents(nav);
            }
            else
            {
                ComponentInstallerBase installer = InstallerFactory.GetInstaller(package.PackageType);
                if (installer != null)
                {
					//Set package
                    installer.Package = package;

                    //Set type
                    installer.Type = package.PackageType;
                    _componentInstallers.Add(0, installer);
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// This Constructor creates a new PackageInstaller instance
        /// </summary>
        /// <param name="info">An InstallerInfo instance</param>
        /// <param name="packageManifest">The manifest as a string</param>
        /// -----------------------------------------------------------------------------
        public PackageInstaller(string packageManifest, InstallerInfo info)
        {
            IsValid = true;
            DeleteFiles = Null.NullBoolean;
            Package = new PackageInfo(info);
            Package.Manifest = packageManifest;

            if (!string.IsNullOrEmpty(packageManifest))
            {
				//Create an XPathDocument from the Xml
                var doc = new XPathDocument(new StringReader(packageManifest));
                XPathNavigator nav = doc.CreateNavigator().SelectSingleNode("package");
                ReadManifest(nav);
            }
        }
		
		#endregion

		#region Public Properties

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets and sets whether the Packages files are deleted when uninstalling the
        /// package
        /// </summary>
        /// <value>A Boolean value</value>
        /// -----------------------------------------------------------------------------
        public bool DeleteFiles { get; set; }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Gets whether the Package is Valid
        /// </summary>
        /// <value>A Boolean value</value>
        /// -----------------------------------------------------------------------------
        public bool IsValid { get; private set; }
		
		#endregion

		#region Private Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The CheckSecurity method checks whether the user has the appropriate security
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void CheckSecurity()
        {
            PackageType type = PackageController.Instance.GetExtensionPackageType(t => t.PackageType.Equals(Package.PackageType, StringComparison.OrdinalIgnoreCase));
            if (type == null)
            {
                //This package type not registered
				Log.Logs.Clear();
                Log.AddFailure(Util.SECURITY_NotRegistered + " - " + Package.PackageType);
                IsValid = false;
            }
            else
            {
                if (type.SecurityAccessLevel > Package.InstallerInfo.SecurityAccessLevel)
                {
                    Log.Logs.Clear();
                    Log.AddFailure(Util.SECURITY_Installer);
                    IsValid = false;
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The ReadComponents method reads the components node of the manifest file.
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void ReadComponents(XPathNavigator manifestNav)
        {
            foreach (XPathNavigator componentNav in manifestNav.CreateNavigator().Select("components/component"))
            {
                //Set default order to next value (ie the same as the size of the collection)
				int order = _componentInstallers.Count;
				
                string type = componentNav.GetAttribute("type", "");
                if (InstallMode == InstallMode.Install)
                {
                    string installOrder = componentNav.GetAttribute("installOrder", "");
                    if (!string.IsNullOrEmpty(installOrder))
                    {
                        order = int.Parse(installOrder);
                    }
                }
                else
                {
                    string unInstallOrder = componentNav.GetAttribute("unInstallOrder", "");
                    if (!string.IsNullOrEmpty(unInstallOrder))
                    {
                        order = int.Parse(unInstallOrder);
                    }
                }
                if (Package.InstallerInfo != null)
                {
                    Log.AddInfo(Util.DNN_ReadingComponent + " - " + type);
                }
                ComponentInstallerBase installer = InstallerFactory.GetInstaller(componentNav, Package);
                if (installer == null)
                {
                    Log.AddFailure(Util.EXCEPTION_InstallerCreate);
                }
                else
                {
                    _componentInstallers.Add(order, installer);
                    Package.InstallerInfo.AllowableFiles += ", " + installer.AllowableFiles;
                }
            }
        }

        private string ReadTextFromFile(string source)
        {
            string strText = Null.NullString;
            if (Package.InstallerInfo.InstallMode != InstallMode.ManifestOnly)
            {
				//Load from file
                strText = FileSystemUtils.ReadFile(Package.InstallerInfo.TempInstallFolder + "\\" + source);
            }
            return strText;
        }

		#endregion

	    #region Public Methods

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The Commit method commits the package installation
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Commit()
        {
            for (int index = 0; index <= _componentInstallers.Count - 1; index++)
            {
                ComponentInstallerBase compInstaller = _componentInstallers.Values[index];
                if (compInstaller.Version >= Package.InstalledVersion && compInstaller.Completed)
                {
                    compInstaller.Commit();
                }
            }

            //Add Event Message
            if (_eventMessage != null && !String.IsNullOrEmpty(_eventMessage.Attributes["UpgradeVersionsList"]))
            {
                _eventMessage.Attributes.Set("desktopModuleID", Null.NullInteger.ToString());
                EventQueueController.SendMessage(_eventMessage, "Application_Start");
            }

            if (Log.Valid)
            {
                Log.AddInfo(Util.INSTALL_Committed);
            }
            else
            {
                Log.AddFailure(Util.INSTALL_Aborted);
            }
            Package.InstallerInfo.PackageID = Package.PackageID;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The Install method installs the components of the package
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Install()
        {
            bool isCompleted = true;
            try
            {
				//Save the Package Information
                if (_installedPackage != null)
                {
                    Package.PackageID = _installedPackage.PackageID;
                }
				
                //Save Package
                PackageController.Instance.SaveExtensionPackage(Package);

                //Iterate through all the Components
                for (int index = 0; index <= _componentInstallers.Count - 1; index++)
                {
                    ComponentInstallerBase compInstaller = _componentInstallers.Values[index];
                    if ((_installedPackage == null) || (compInstaller.Version > Package.InstalledVersion) || (Package.InstallerInfo.RepairInstall))
                    {
                        Log.AddInfo(Util.INSTALL_Start + " - " + compInstaller.Type);
                        compInstaller.Install();
                        if (compInstaller.Completed)
                        {
                            if (compInstaller.Skipped)
                            {
                                Log.AddInfo(Util.COMPONENT_Skipped + " - " + compInstaller.Type);
                            }
                            else
                            {
                                Log.AddInfo(Util.COMPONENT_Installed + " - " + compInstaller.Type);
                            }
                        }
                        else
                        {
                            Log.AddFailure(Util.INSTALL_Failed + " - " + compInstaller.Type);
                            isCompleted = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.AddFailure(Util.INSTALL_Aborted + " - " + Package.Name + ":" + ex.Message);
            }
            if (isCompleted)
            {
				//All components successfully installed so Commit any pending changes
                Commit();
            }
            else
            {
				//There has been a failure so Rollback
                Rollback();
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The ReadManifest method reads the manifest file and parses it into components.
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void ReadManifest(XPathNavigator manifestNav)
        {
            //Get Name Property
            Package.Name = Util.ReadAttribute(manifestNav, "name", Log, Util.EXCEPTION_NameMissing);

            //Get Type
            Package.PackageType = Util.ReadAttribute(manifestNav, "type", Log, Util.EXCEPTION_TypeMissing);

            //If Skin or Container then set PortalID
            if (Package.PackageType.Equals("Skin", StringComparison.OrdinalIgnoreCase) || Package.PackageType.Equals("Container", StringComparison.OrdinalIgnoreCase))
            {
                Package.PortalID = Package.InstallerInfo.PortalID;
            }
            CheckSecurity();
            if (!IsValid)
            {
                return;
            }

            //Get IsSystem
            Package.IsSystemPackage = bool.Parse(Util.ReadAttribute(manifestNav, "isSystem", false, Log, "", bool.FalseString));

            //Get Version
            string strVersion = Util.ReadAttribute(manifestNav, "version", Log, Util.EXCEPTION_VersionMissing);
            if (string.IsNullOrEmpty(strVersion))
            {
                IsValid = false;
            }
            if (IsValid)
            {
                Package.Version = new Version(strVersion);
            }
            else
            {
                return;
            }

            //Attempt to get the Package from the Data Store (see if its installed)
            var packageType = PackageController.Instance.GetExtensionPackageType(t => t.PackageType.Equals(Package.PackageType, StringComparison.OrdinalIgnoreCase));

            if (packageType.SupportsSideBySideInstallation)
            {
                _installedPackage = PackageController.Instance.GetExtensionPackage(Package.PortalID, p => p.Name.Equals(Package.Name, StringComparison.OrdinalIgnoreCase)
                                                                                                            && p.PackageType.Equals(Package.PackageType, StringComparison.OrdinalIgnoreCase)
                                                                                                            && p.Version == Package.Version);                
            }
            else
            {
                _installedPackage = PackageController.Instance.GetExtensionPackage(Package.PortalID, p => p.Name.Equals(Package.Name, StringComparison.OrdinalIgnoreCase) 
                                                                                                            && p.PackageType.Equals(Package.PackageType, StringComparison.OrdinalIgnoreCase));
            }

            if (_installedPackage != null)
            {
                Package.InstalledVersion = _installedPackage.Version;
                Package.InstallerInfo.PackageID = _installedPackage.PackageID;

                if (Package.InstalledVersion > Package.Version)
                {
                    Log.AddFailure(Util.INSTALL_Version + " - " + Package.InstalledVersion.ToString(3));
                    IsValid = false;
                }
                else if (Package.InstalledVersion == Package.Version)
                {
                    Package.InstallerInfo.Installed = true;
                    Package.InstallerInfo.PortalID = _installedPackage.PortalID;
                }
            }

            Log.AddInfo(Util.DNN_ReadingPackage + " - " + Package.PackageType + " - " + Package.Name);
            Package.FriendlyName = Util.ReadElement(manifestNav, "friendlyName", Package.Name);
            Package.Description = Util.ReadElement(manifestNav, "description");

            XPathNavigator foldernameNav = null;
            Package.FolderName = String.Empty;
            switch (Package.PackageType)
            {
                case "Module":
                    //In Dynamics moduels, a component:type=File can have a basePath pointing to the App_Conde folder. This is not a correct FolderName
                    //To ensure that FolderName is DesktopModules...
                    var folderNameValue = PackageController.GetSpecificFolderName(manifestNav, "components/component/files|components/component/resourceFiles", "basePath", "DesktopModules");
                    if (!String.IsNullOrEmpty(folderNameValue)) Package.FolderName = folderNameValue.Replace('\\', '/');
                    break;
                case "Auth_System":
                    foldernameNav = manifestNav.SelectSingleNode("components/component/files");
                    if (foldernameNav != null) Package.FolderName = Util.ReadElement(foldernameNav, "basePath").Replace('\\', '/');
                    break;
                case "Container":
                    foldernameNav = manifestNav.SelectSingleNode("components/component/containerFiles");
                    if (foldernameNav != null) Package.FolderName = Globals.glbContainersPath +  Util.ReadElement(foldernameNav, "containerName").Replace('\\', '/');
                    break;
                case "Skin":
                    foldernameNav = manifestNav.SelectSingleNode("components/component/skinFiles");
                    if (foldernameNav != null) Package.FolderName = Globals.glbSkinsPath + Util.ReadElement(foldernameNav, "skinName").Replace('\\', '/');
                    break;
                default:
                    //copied from "Module" without the extra OR condition
                    folderNameValue = PackageController.GetSpecificFolderName(manifestNav, "components/component/resourceFiles", "basePath", "DesktopModules");
                    if (!String.IsNullOrEmpty(folderNameValue)) Package.FolderName = folderNameValue.Replace('\\', '/');
                    break;
            }

            _eventMessage = ReadEventMessageNode(manifestNav);

            //Get Icon
            XPathNavigator iconFileNav = manifestNav.SelectSingleNode("iconFile");
            if (iconFileNav != null)
            {
                if (iconFileNav.Value != string.Empty)
                {
                    if (iconFileNav.Value.StartsWith("~/"))
                    {
                        Package.IconFile = iconFileNav.Value;
                    }
                    else if (iconFileNav.Value.StartsWith("DesktopModules", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Package.IconFile = string.Format("~/{0}", iconFileNav.Value);
                    }
                    else
                    {
                        Package.IconFile = (String.IsNullOrEmpty(Package.FolderName) ? "" :  Package.FolderName + "/") + iconFileNav.Value;
                        Package.IconFile = (!Package.IconFile.StartsWith("~/")) ? "~/" + Package.IconFile : Package.IconFile;
                    }
                }
            }
			//Get Author
            XPathNavigator authorNav = manifestNav.SelectSingleNode("owner");
            if (authorNav != null)
            {
                Package.Owner = Util.ReadElement(authorNav, "name");
                Package.Organization = Util.ReadElement(authorNav, "organization");
                Package.Url = Util.ReadElement(authorNav, "url");
                Package.Email = Util.ReadElement(authorNav, "email");
            }
			
            //Get License
            XPathNavigator licenseNav = manifestNav.SelectSingleNode("license");
            if (licenseNav != null)
            {
                string licenseSrc = Util.ReadAttribute(licenseNav, "src");
                if (string.IsNullOrEmpty(licenseSrc))
                {
					//Load from element
                    Package.License = licenseNav.Value;
                }
                else
                {
                    Package.License = ReadTextFromFile(licenseSrc);
                }
            }
            if (string.IsNullOrEmpty(Package.License))
            {
				//Legacy Packages have no license
                Package.License = Util.PACKAGE_NoLicense;
            }
			
            //Get Release Notes
            XPathNavigator relNotesNav = manifestNav.SelectSingleNode("releaseNotes");
            if (relNotesNav != null)
            {
                string relNotesSrc = Util.ReadAttribute(relNotesNav, "src");
                if (string.IsNullOrEmpty(relNotesSrc))
                {
					//Load from element
                    Package.ReleaseNotes = relNotesNav.Value;
                }
                else
                {
                    Package.ReleaseNotes = ReadTextFromFile(relNotesSrc);
                }
            }
            if (string.IsNullOrEmpty(Package.ReleaseNotes))
            {
				//Legacy Packages have no Release Notes
				Package.ReleaseNotes = Util.PACKAGE_NoReleaseNotes;
            }

            //Parse the Dependencies
            var packageDependencies = Package.Dependencies;
            foreach (XPathNavigator dependencyNav in manifestNav.CreateNavigator().Select("dependencies/dependency"))
            {
			    var dependency = DependencyFactory.GetDependency(dependencyNav);
		        var packageDependecy = dependency as IManagedPackageDependency;

                if (packageDependecy != null)
                {
                    packageDependencies.Add(packageDependecy.PackageDependency);
                }

                if (!dependency.IsValid)
                {
                    Log.AddFailure(dependency.ErrorMessage);
                    return;
                }
            }

            //Read Components
            ReadComponents(manifestNav);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The Rollback method rolls back the package installation
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void Rollback()
        {
            for (int index = 0; index <= _componentInstallers.Count - 1; index++)
            {
                ComponentInstallerBase compInstaller = _componentInstallers.Values[index];
                if (compInstaller.Version > Package.InstalledVersion && compInstaller.Completed)
                {
                    Log.AddInfo(Util.COMPONENT_RollingBack + " - " + compInstaller.Type);
                    compInstaller.Rollback();
                    Log.AddInfo(Util.COMPONENT_RolledBack + " - " + compInstaller.Type);
                }
            }
			
            //If Previously Installed Package exists then we need to update the DataStore with this 
            if (_installedPackage == null)
            {
				//No Previously Installed Package - Delete newly added Package
                PackageController.Instance.DeleteExtensionPackage(Package);
            }
            else
            {
				//Previously Installed Package - Rollback to Previously Installed
                PackageController.Instance.SaveExtensionPackage(_installedPackage);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// The Uninstall method uninstalls the components of the package
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UnInstall()
        {
			//Iterate through all the Components
            for (int index = 0; index <= _componentInstallers.Count - 1; index++)
            {
                ComponentInstallerBase compInstaller = _componentInstallers.Values[index];
                var fileInstaller = compInstaller as FileInstaller;
                if (fileInstaller != null)
                {
                    fileInstaller.DeleteFiles = DeleteFiles;
                }
                Log.ResetFlags();
                Log.AddInfo(Util.UNINSTALL_StartComp + " - " + compInstaller.Type);
                compInstaller.UnInstall();
                Log.AddInfo(Util.COMPONENT_UnInstalled + " - " + compInstaller.Type);
                if (Log.Valid)
                {
                    Log.AddInfo(Util.UNINSTALL_SuccessComp + " - " + compInstaller.Type);
                }
                else
                {
                    Log.AddWarning(Util.UNINSTALL_WarningsComp + " - " + compInstaller.Type);
                }
            }
			
            //Remove the Package information from the Data Store
            PackageController.Instance.DeleteExtensionPackage(Package);
        }
		
		#endregion
    }
}
