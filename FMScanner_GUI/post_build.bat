echo ------------------------- START OF post_build.bat

rem ~ strips surrounded quotes if they exist
rem batch file hell #9072: no spaces can exist around = sign for these lines
set ConfigurationName=%~1
set TargetDir=%~2
set ProjectDir=%~3
set SolutionDir=%~4

rem Still copy this for SevenZipSharp's use
"%system%xcopy" "%TargetDir%x86\7z.dll" "%TargetDir%" /y

"%system%xcopy" "%SolutionDir%bin_dependencies\7z32" "%TargetDir%\7z32\" /y /i
"%system%xcopy" "%SolutionDir%bin_dependencies\7z64" "%TargetDir%\7z64\" /y /i

rem Dumb hack to get rid of extraneous dll files because ludicrously
rem xcopy requires you to make an entire file just to list excludes, rather than
rem specifying them on the command line like someone who is not clinically insane
del /F "%TargetDir%JetBrains.Annotations.dll"
del /F "%TargetDir%.gitkeep"