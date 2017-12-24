#tool nuget:?package=NUnit.ConsoleRunner&version=3.7.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PROPERTIES
//////////////////////////////////////////////////////////////////////

var BUILD_DIR = Context.Environment.WorkingDirectory.FullPath;
var SOLUTION_FILE_PATH = $@"{BUILD_DIR}\..\Log4Net.Extensions.sln";
var TEST_PROJECT_DIRECTORY = $@"{BUILD_DIR}\..\Tests\Unit\Log4Net.Extensions.Tests.Unit\bin\{configuration}";
var RUNTIME = "netcoreapp2.0";
var UNIT_TEST_DLL = "Walls.Julian.Log4Net.Extensions.Tests.Unit.dll";
   

//////////////////////////////////////////////////////////////////////
// SETUP AND TEARDOWN TASKS
//////////////////////////////////////////////////////////////////////

Setup(context => {
	Information($"BUILD_DIR: {BUILD_DIR}");
	Information($"SOLUTION_FILE_PATH: {SOLUTION_FILE_PATH}");
	Information($"TEST_PROJECT_DIRECTORY: {TEST_PROJECT_DIRECTORY}");
	Information($"RUNTIME: {RUNTIME}");
	Information($"UNIT_TEST_DLL: {UNIT_TEST_DLL}");
});

Teardown(context => CheckForError(ref ErrorDetail));


//////////////////////////////////////////////////////////////////////
// METHODS
//////////////////////////////////////////////////////////////////////
var ErrorDetail = new List<string>();

void RunDotnetCoreTests(FilePath exePath, DirectoryPath workingDir, string framework, ref List<string> errorDetail)
{
    int rc = StartProcess(
        "dotnet",
        new ProcessSettings()
        {
            Arguments = exePath + "",
            WorkingDirectory = workingDir
        });

    if (rc > 0)
        errorDetail.Add(string.Format("{0}: {1} tests failed", framework, rc));
    else if (rc < 0)
        errorDetail.Add(string.Format("{0} returned rc = {1}", exePath, rc));
}

void CheckForError(ref List<string> errorDetail)
{
    if(errorDetail.Count != 0)
    {
        var copyError = new List<string>();
        copyError = errorDetail.Select(s => s).ToList();
        errorDetail.Clear();
        throw new Exception("One or more unit tests failed, breaking the build.\n"
                              + copyError.Aggregate((x,y) => x + "\n" + y));
    }
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreRestore(SOLUTION_FILE_PATH);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var msbuildSettings = new MSBuildSettings
        {
            Verbosity = Verbosity.Minimal,
            Configuration = configuration
        };  
	
	MSBuild(SOLUTION_FILE_PATH, msbuildSettings);

});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
	.OnError(exception => { ErrorDetail.Add(exception.Message); })
    .Does(() =>
{
   RunDotnetCoreTests(TEST_PROJECT_DIRECTORY + $@"/{RUNTIME}/"  + $"{UNIT_TEST_DLL}", TEST_PROJECT_DIRECTORY, RUNTIME, ref ErrorDetail);
});



//TASK TARGETS
Task("Default").IsDependentOn("Build");

Task("Test")
    .Description("Builds and tests")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);