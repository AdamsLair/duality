﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Duality.Editor.Plugins.ProjectView.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ProjectViewRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ProjectViewRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Duality.Editor.Plugins.ProjectView.Properties.ProjectViewRes", typeof(ProjectViewRes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        public static System.Drawing.Icon IconProjectView {
            get {
                object obj = ResourceManager.GetObject("IconProjectView", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project View.
        /// </summary>
        public static string MenuItemName_ProjectView {
            get {
                return ResourceManager.GetString("MenuItemName_ProjectView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Folder.
        /// </summary>
        public static string NewFolderName {
            get {
                return ResourceManager.GetString("NewFolderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Doubleclick: {0}.
        /// </summary>
        public static string ProjectFolderView_Help_Doubleclick {
            get {
                return ResourceManager.GetString("ProjectFolderView_Help_Doubleclick", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t move file.
        /// </summary>
        public static string ProjectFolderView_MsgBox_CantMove_Caption {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_CantMove_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t move file to &apos;{0}&apos; because it would overwrite an existing file..
        /// </summary>
        public static string ProjectFolderView_MsgBox_CantMove_Text {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_CantMove_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t rename file.
        /// </summary>
        public static string ProjectFolderView_MsgBox_CantRename_Caption {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_CantRename_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t rename file to &apos;{0}&apos; because it would overwrite an existing file..
        /// </summary>
        public static string ProjectFolderView_MsgBox_CantRename_Text {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_CantRename_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete selected files?.
        /// </summary>
        public static string ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Caption {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Caption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you really want to delete the selected files?.
        /// </summary>
        public static string ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Text {
            get {
                return ResourceManager.GetString("ProjectFolderView_MsgBox_ConfirmDeleteSelectedFiles_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project.
        /// </summary>
        public static string ProjectNameLabel {
            get {
                return ResourceManager.GetString("ProjectNameLabel", resourceCulture);
            }
        }
    }
}
