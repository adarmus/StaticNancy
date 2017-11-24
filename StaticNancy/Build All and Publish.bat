@echo off
echo Building Web
"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" /m "StaticNancy.sln" /p:platform="Any CPU" /p:configuration="Release" /p:DeployOnBuild=true /p:PublishProfile=FileSystem /fileLogger1 /P:Framework40Dir=c:\windows\microsoft.net\framework\v4.0.30319

rem msbuild MyWebProject.csproj /T:Package /P:PublishProfile=MyProfile /P:Framework40Dir=c:\windows\microsoft.net\framework\v4.0.30319
rem /m BuildNotification.sln /p:platform="x86" /p:configuration="Release" /p:DeployOnBuild=true /p:PublishProfile=PublishToFile /fileLogger1

PUSHD StaticNancy.Web.Deploy
"C:\Program Files\7-Zip\7z.exe" a -tzip "..\StaticNancy.zip" * -x!*.locked -x!*.pdb -x!*.xml -x!*.log -x!*.vshost.*
POPD

ECHO.
ECHO Copy to Dropbox ?
ECHO.

PAUSE

ECHO Copying...

COPY "StaticNancy.zip" C:\Users\adam.blair\Dropbox

ECHO Copying...done

PAUSE
