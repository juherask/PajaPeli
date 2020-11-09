ECHO Asennetaan Jypeli projektimallit
IF NOT EXIST "%USERPROFILE%\My Documents" GOTO NOMYDOC
   XCOPY "C:\sepeli\asennaJypeliVS2010\projektimallit" "%USERPROFILE%\My Documents\Visual Studio 2010\Templates\ProjectTemplates\Visual C#" /e /y /s
   GOTO PROFILESCOPIED
:NOMYDOC
IF NOT EXIST "%USERPROFILE%\Documents" GOTO NODOCS
   XCOPY "C:\sepeli\asennaJypeliVS2010\projektimallit" "%USERPROFILE%\Documents\Visual Studio 2010\Templates\ProjectTemplates\Visual C#" /e /y /s
   GOTO PROFILESCOPIED
:NODOCS
IF NOT EXIST "%USERPROFILE%\Omat Tiedostot" GOTO NOOMATTIEDOT
   XCOPY "C:\sepeli\asennaJypeliVS2010\projektimallit" "%USERPROFILE%\Omat Tiedostot\Visual Studio 2010\Templates\ProjectTemplates\Visual C#" /e /y /s
   GOTO PROFILESCOPIED
:NOOMATTIEDOT
IF NOT EXIST "%USERPROFILE%\Tiedostot" GOTO PROFILESCOPIED
   XCOPY "C:\sepeli\asennaJypeliVS2010\projektimallit" "%USERPROFILE%\Tiedostot\Visual Studio 2010\Templates\ProjectTemplates\Visual C#" /e /y /s
   GOTO PROFILESCOPIED
:PROFILESCOPIED
SLEEP 5
