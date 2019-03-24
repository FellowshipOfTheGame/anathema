cmake_minimum_required(VERSION 3.14)
project(Anathema VERSION 1.0.1 LANGUAGES NONE)

set(CMAKE_PROJECT_HOMEPAGE_URL "https://fog-icmc.itch.io/anathema")

set(CPACK_PACKAGE_NAME Anathema)
set(CPACK_PACKAGE_VENDOR "Fellowship of the Game")
set(CPACK_PACKAGE_CONTACT "fog@icmc.usp.br")

# set(UNITY_VERSION "2018.2.10f1")
set(UNITY_VERSION "2018.2.12f1")
# set(UNITY_ROOT_PATH "C:\\Program Files\\Unity\\Hub\\Editor")
set(UNITY_ROOT_PATH "/Applications/Unity/Hub/Editor")
# set(UNITY_PATH ${UNITY_ROOT_PATH}/${UNITY_VERSION}/Editor)
set(UNITY_PATH ${UNITY_ROOT_PATH}/${UNITY_VERSION}/Unity.app/Contents/MacOS)
set(UNITY_EXECUTABLE ${UNITY_PATH}/Unity)
set(INSTALLER_SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/InstallerSources)

if(${UNITY_BUILD_TYPE} STREQUAL "Windows64")
    add_custom_target(Windows64
        ALL
        COMMENT "Building for Windows64"
        COMMAND ${UNITY_EXECUTABLE}.exe -projectpath ${CMAKE_CURRENT_SOURCE_DIR}/Anathema -buildWindows64Player ${CMAKE_BINARY_DIR}/Win64/Anathema.exe -batchmode -quit
    )
    set(CPACK_GENERATOR WIX)
    set(CPACK_WIX_UPGRADE_GUID F8DC732F-72EF-4951-B571-16FF0D6F2B48)
    set(CPACK_WIX_PRODUCT_GUID *)
    set(CPACK_WIX_LICENSE_RTF ${INSTALLER_SOURCES}/LICENSE.rtf)
    set(CPACK_WIX_PRODUCT_ICON ${INSTALLER_SOURCES}/Anathema.ico)
    set(CPACK_WIX_UI_BANNER ${INSTALLER_SOURCES}/Banner.bmp)
    set(CPACK_WIX_UI_DIALOG ${INSTALLER_SOURCES}/Dialog.bmp)
    set(CPACK_SYSTEM_NAME win64)
    set(CPACK_WIX_ROOT_FOLDER_ID ProgramFilesFolder)
elseif(${UNITY_BUILD_TYPE} STREQUAL "Mac")
    add_custom_target(Mac
        ALL
        COMMENT "Building for Mac"
        COMMAND ${UNITY_EXECUTABLE} -projectpath ${CMAKE_CURRENT_SOURCE_DIR}/Anathema -buildOSXUniversalPlayer ${CMAKE_BINARY_DIR}/Anathema.app -batchmode -quit -logFile /dev/stdout
    )
    set(CPACK_GENERATOR DragNDrop)
    set(CPACK_DMG_DS_STORE_SETUP_SCRIPT ${INSTALLER_SOURCES}/CMakeDMGSetup.scpt)
    set(CPACK_DMG_BACKGROUND_IMAGE ${INSTALLER_SOURCES}/CMakeDMGBackground.tif)
endif()

set(CPACK_PACKAGE_DESCRIPTION_FILE ${CMAKE_CURRENT_SOURCE_DIR}/README.md)
# set(CPACK_PACKAGE_DESCRIPTION_SUMMARY)
set(CPACK_PACKAGE_INSTALL_DIRECTORY "Anathema")
set(CPACK_PACKAGE_ICON ${INSTALLER_SOURCES}/Anathema.ico)
set(CPACK_RESOURCE_FILE_LICENSE ${CMAKE_CURRENT_SOURCE_DIR}/LICENSE)
set(CPACK_RESOURCE_FILE_README ${CMAKE_CURRENT_SOURCE_DIR}/README.md)
# set(CPACK_RESOURCE_FILE_WELCOME)
set(CPACK_PACKAGE_EXECUTABLES Anathema;Anathema)
# set(CMAKE_INSTALL_PREFIX /opt)


include(CPack)

if(${UNITY_BUILD_TYPE} STREQUAL "Windows64")
    install(DIRECTORY ${CMAKE_BINARY_DIR}/Win64/ DESTINATION bin)
    set_property(INSTALL "Anathema.exe"
        PROPERTY CPACK_START_MENU_SHORTCUTS "Anathema"
    )
    set_property(INSTALL "Anathema.exe"
        PROPERTY CPACK_STARTUP_SHORTCUTS "Anathema"
    )
elseif(${UNITY_BUILD_TYPE} STREQUAL "Mac")
    install(DIRECTORY ${CMAKE_BINARY_DIR}/Anathema.app DESTINATION . USE_SOURCE_PERMISSIONS)
endif()