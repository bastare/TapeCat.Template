using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
public class Build : NukeBuild
{
	[Parameter ( "Configuration to build - Default is 'Debug' (local) or 'Release' (server)" )]
	public readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Solution] readonly Solution Solution;

	public static AbsolutePath SourceDirectory => RootDirectory / "src";

	public static AbsolutePath TestsDirectory => RootDirectory / "tests";

	public static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

	public static int Main ()
		=> Execute<Build> ( build => build.Compile );

	public Target Clean =>
		_ => _
			.Before ( Restore )
			.Executes ( () =>
			  {
				  SourceDirectory.GlobDirectories ( "**/bin" , "**/obj" ).ForEach ( DeleteDirectory );
				  TestsDirectory.GlobDirectories ( "**/bin" , "**/obj" ).ForEach ( DeleteDirectory );

				  EnsureCleanDirectory ( ArtifactsDirectory );
			  } );

	public Target Restore =>
		_ => _
			.Executes ( () =>
			  {
				  DotNetRestore ( dotNetRestoreSettings => dotNetRestoreSettings
					  .SetProjectFile ( Solution ) );
			  } );

	public Target Compile =>
		_ => _
			.DependsOn ( Restore )
			.Executes ( () =>
			  {
				  DotNetBuild ( dotNetBuildSettings => dotNetBuildSettings
					.SetProjectFile ( Solution )
					.SetConfiguration ( Configuration )
					.EnableNoRestore () );
			  } );
}