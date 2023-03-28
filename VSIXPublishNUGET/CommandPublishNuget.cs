using CommonLibrary;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace VSIXPublishNUGET
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CommandPublishNuget
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("da5387c3-810b-4151-8025-5ed4aaa96b1a");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandPublishNuget"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private CommandPublishNuget(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static CommandPublishNuget Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in CommandPublishNuget's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new CommandPublishNuget(package, commandService);
        }

         /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        
        
        private void Execute(object sender, EventArgs e)
        {
            Console.SetOut(new OutWriter());

            IVsMonitorSelection monitorSelection = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));
            monitorSelection.GetCurrentSelection(out IntPtr hierarchyPointer,
                                                  out UInt32 projectItemId,
                                                  out IVsMultiItemSelect multiItemSelect,
                                                  out IntPtr selectionContainerPointer);
            IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                                      hierarchyPointer,
                                      typeof(IVsHierarchy)) as IVsHierarchy;

            if (selectedHierarchy != null)
            {
                ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
                                                  projectItemId,
                                                  (int)__VSHPROPID.VSHPROPID_ExtObject,
                                                  out object selectedObject));
                Project selectedProject = selectedObject as Project;

                string projectPath = selectedProject.FullName;

                ProjectConfig config = new ProjectConfig(projectPath);
                try
                {
                    
                    var nugetKeyPath = config.GetValue<string>("nugetAuthTokenPath");
                    if (nugetKeyPath == null)
                        throw new Exception("The parameter 'nugetAuthTokenPath'  must be defined in file SabatexSettings.json");

                    string[] token = File.ReadAllLines(nugetKeyPath);
                    if (token.Length == 0 || token.Length > 1)
                        throw new Exception("The NUGET TOKEN is wrong!");
                    string nugetAuthToken = token[0];

                    config.RunScript($"del {config.OutputPath}\\*.nupkg");

                    string includeSource = config.IsPreRelease ? "--include-source" : string.Empty;
                    string script = $"dotnet pack --configuration {config.BuildConfiguration} {includeSource} \"{config.ProjectFilePath}\"";
                    if (!config.RunScript(script))
                        throw new Exception("Error build project!");

                    string symbols = config.IsPreRelease ? ".symbols" : string.Empty;
                    script = $"dotnet nuget push \"{config.OutputPath}\\*{symbols}.nupkg\" -k {nugetAuthToken} -s https://api.nuget.org/v3/index.json --skip-duplicate";
                    if (!config.RunScript(script))
                        throw new Exception("Error publish to nuget!");
                    Console.WriteLine("Done!");
                }
                catch (Exception ex) { }
            }


        }
    }
}
