@Echo off
ECHO Tämä skripti asentaa Jypelin kaikkine oheishilppeineen. Painele Next,next,next aina välillä.
REM Kansiorakenteen tulisi olla seuraavanlainen:

REM Check Windows Version
ver | findstr /i "5\.1\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_XP
ver | findstr /i "6\.0\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Vista
ver | findstr /i "6\.1\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Win7
ver | findstr /i "6\.2\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Win8
ver | findstr /i "6\.3\." > nul
IF %ERRORLEVEL% EQU 0 goto ver_Win8
goto warn_and_exit

:ver_Win8
ECHO Asennetaan Games for Windows
start /wait "" "C:\sepeli\asennaJypeliVS2010\gfwlivesetup_full.exe" /q /norestart
SLEEP 5

:ver_Win7

REM ECHO Asennetaan Paint.NET 3.5
REM #start /wait "" "C:\sepeli\asennaJypeliVS2010\Paint.NET.3.5.11.Install.exe" /auto CHECKFORUPDATES=0
REM #SLEEP 5

goto MAIN_INSTALL_BATCH

:ver_Vista

ECHO Asennetaan Microsoft Installer 4.5
start /wait "" "wusa.exe" "C:\sepeli\asennaJypeliVS2010\Windows6.0-KB942288-v2-x86.msu" /quiet /norestart
SLEEP 5

ECHO Asennetaan Paint.NET 3.5
start /wait "" "C:\sepeli\asennaJypeliVS2010\Paint.NET.3.5.11.Install.exe" /auto CHECKFORUPDATES=0
SLEEP 5

goto MAIN_INSTALL_BATCH

:ver_XP

ECHO Asennetaan Microsoft Installer 4.5
start /wait "" "C:\sepeli\asennaJypeliVS2010\WindowsXP-KB942288-v3-x86.exe" /quiet /norestart
SLEEP 5

ECHO Asennetaan Paint.NET 3.5
start /wait "" "C:\sepeli\asennaJypeliVS2010\Paint.NET.3.5.11.Install.exe" /auto CHECKFORUPDATES=0
SLEEP 5

ECHO Asennetaan Visual C# 2010 Express
start /wait "" "C:\sepeli\asennaJypeliVS2010\VS2010Express1\VCSExpress\setup.exe" /q /norestart
SLEEP 5
REM Jostain syystä edellinen käsky jää kesken XP:ssä. Onneksi tämä uudestaan ajaminen jatkaa ja näyttää prosessin etenemisen. Vaatii tosin käyttäjältä next next naputtelua.
start /wait "" "C:\sepeli\asennaJypeliVS2010\VS2010Express1\VCSExpress\setup.exe"

goto MAIN_INSTALL_BATCH_XP


:MAIN_INSTALL_BATCH

ECHO Asennetaan Visual C# 2010 Express
start /wait "" "C:\sepeli\asennaJypeliVS2010\VS2010Express1\VCSExpress\setup.exe" /q /norestart
SLEEP 5

:MAIN_INSTALL_BATCH_XP

ECHO Asennetaan XNA Game Studio 4.0
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\XLiveRedist.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Redist\XNA FX Redist\xnafx40_redist.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\xnaliveproxy.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\xnags_platform_tools.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\xnags_shared.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\xnags_documentation.msi" /quiet
start /wait "" "C:\sepeli\asennaJypeliVS2010\XNAGS40\Setup\xnags_visualstudio.msi" /quiet
SLEEP 5

ECHO Asennetaan Jypeli
start /wait "" "C:\sepeli\asennaJypeliVS2010\Jypeli_setup.exe" /s /S
SLEEP 5

ECHO Jypeli ja kilkkeet asennettu. Aloita tekemällä "Ensimmäinen peli" opas.
SLEEP 5
start https://trac.cc.jyu.fi/projects/npo/wiki/Pong/Johdanto

goto end
:warn_and_exit
echo Tuntematon käyttöjärjestelmäversio.

:end  

