; Script generated by the HM NIS Edit Script Wizard.

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "Clans"
!define PRODUCT_VERSION "0.10.0"
!define PRODUCT_PUBLISHER "JeziL"
!define PRODUCT_WEB_SITE "https://github.com/JeziL/Clans"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Clans.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI2.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "..\Clans\Resources\Clash.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Language Selection Dialog Settings
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE "..\LICENSE"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\Clans.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"
!insertmacro MUI_LANGUAGE "TradChinese"

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "ClansSetup.exe"
InstallDir "$PROGRAMFILES\Clans"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Function .onInit
  !insertmacro MUI_LANGDLL_DISPLAY
FunctionEnd

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite try
  File "..\Clans\bin\Release\Clans.exe"
  CreateDirectory "$SMPROGRAMS\Clans"
  CreateShortCut "$SMPROGRAMS\Clans\Clans.lnk" "$INSTDIR\Clans.exe"
  CreateShortCut "$DESKTOP\Clans.lnk" "$INSTDIR\Clans.exe"
  File "..\Clans\bin\Release\Clans.exe.config"
  File "..\Clans\bin\Release\Clans.pdb"
  File "..\Clans\bin\Release\Newtonsoft.Json.dll"
  File "..\Clans\bin\Release\Newtonsoft.Json.xml"
  SetOutPath "$INSTDIR\Resources"
  File "..\Clans\bin\Release\Resources\clash.exe"
  File "..\Clans\bin\Release\Resources\config.yaml"
  File "..\Clans\bin\Release\Resources\Country.mmdb"
  File "..\Clans\bin\Release\Resources\sysproxy.exe"
  SetOutPath "$INSTDIR"
  File "..\Clans\bin\Release\YamlDotNet.dll"
  File "..\Clans\bin\Release\YamlDotNet.xml"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\Clans\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\Clans.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Clans.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) has been uninstalled."
FunctionEnd

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Uninstall $(^Name)?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\YamlDotNet.xml"
  Delete "$INSTDIR\YamlDotNet.dll"
  Delete "$INSTDIR\Resources\sysproxy.exe"
  Delete "$INSTDIR\Resources\Country.mmdb"
  Delete "$INSTDIR\Resources\config.yaml"
  Delete "$INSTDIR\Resources\clash.exe"
  Delete "$INSTDIR\Newtonsoft.Json.xml"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\Clans.pdb"
  Delete "$INSTDIR\Clans.exe.config"
  Delete "$INSTDIR\Clans.exe"

  Delete "$SMPROGRAMS\Clans\Uninstall.lnk"
  Delete "$DESKTOP\Clans.lnk"
  Delete "$SMPROGRAMS\Clans\Clans.lnk"

  RMDir "$SMPROGRAMS\Clans"
  RMDir "$INSTDIR\Resources"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  DeleteRegValue HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "Clans"
  SetAutoClose true
SectionEnd