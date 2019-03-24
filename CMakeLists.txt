cmake_minimum_required(VERSION 3.12)
project(Anathema VERSION 1.0.1 LANGUAGES NONE)

set(CMAKE_PROJECT_HOMEPAGE_URL "https://fog-icmc.itch.io/anathema")

set(CPACK_PACKAGE_NAME Anathema)
set(CPACK_PACKAGE_VENDOR "Fellowship of the Game")
set(CPACK_PACKAGE_CONTACT "fog@icmc.usp.br")

# set(UNITY_VERSION "2018.2.10f1")
# set(UNITY_VERSION "2018.2.12f1")
set(UNITY_VERSION "2018.2.20f1")
# set(UNITY_ROOT_PATH "C:\\Program Files\\Unity\\Hub\\Editor")
# set(UNITY_ROOT_PATH "/Applications/Unity/Hub/Editor")
set(UNITY_ROOT_PATH "$ENV{HOME}/Unity/Hub/Editor")
set(UNITY_PATH ${UNITY_ROOT_PATH}/${UNITY_VERSION}/Editor)
# set(UNITY_PATH ${UNITY_ROOT_PATH}/${UNITY_VERSION}/Unity.app/Contents/MacOS)
set(UNITY_EXECUTABLE ${UNITY_PATH}/Unity)
set(INSTALLER_SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/InstallerSources)

if(${UNITY_BUILD_TYPE} STREQUAL "Windows64")

    add_custom_target(Windows64
        ALL
        COMMENT "Building for Windows64"
        COMMAND ${UNITY_EXECUTABLE}.exe -projectpath ${CMAKE_CURRENT_SOURCE_DIR}/Anathema -buildWindows64Player ${CMAKE_BINARY_DIR}/Win64/Anathema.exe -batchmode -quit
    )
    
    set(CPACK_GENERATOR WIX ZIP)
    set(CPACK_WIX_UPGRADE_GUID F8DC732F-72EF-4951-B571-16FF0D6F2B48)
    set(CPACK_WIX_PRODUCT_GUID *)
    set(CPACK_WIX_LICENSE_RTF ${INSTALLER_SOURCES}/LICENSE.rtf)
    set(CPACK_WIX_PRODUCT_ICON ${INSTALLER_SOURCES}/Anathema.ico)
    set(CPACK_WIX_UI_BANNER ${INSTALLER_SOURCES}/Banner.bmp)
    set(CPACK_WIX_UI_DIALOG ${INSTALLER_SOURCES}/Dialog.bmp)
    set(CPACK_SYSTEM_NAME win64)
    set(CPACK_WIX_ROOT_FOLDER_ID ProgramFilesFolder)
    
    install(DIRECTORY ${CMAKE_BINARY_DIR}/Win64/ DESTINATION bin)
    set_property(INSTALL "Anathema.exe"
        PROPERTY CPACK_START_MENU_SHORTCUTS "Anathema"
    )
    set_property(INSTALL "Anathema.exe"
        PROPERTY CPACK_STARTUP_SHORTCUTS "Anathema"
    )

elseif(${UNITY_BUILD_TYPE} STREQUAL "Mac")

    add_custom_target(Mac
        ALL
        COMMENT "Building for Mac"
        COMMAND ${UNITY_EXECUTABLE} -projectpath ${CMAKE_CURRENT_SOURCE_DIR}/Anathema -buildOSXUniversalPlayer ${CMAKE_BINARY_DIR}/Anathema.app -batchmode -quit -logFile /dev/stdout
    )
    
    set(CPACK_GENERATOR DragNDrop ZIP)
    set(CPACK_DMG_DS_STORE_SETUP_SCRIPT ${INSTALLER_SOURCES}/CMakeDMGSetup.scpt)
    set(CPACK_DMG_BACKGROUND_IMAGE ${INSTALLER_SOURCES}/CMakeDMGBackground.tif)
    
    install(DIRECTORY ${CMAKE_BINARY_DIR}/Anathema.app DESTINATION . USE_SOURCE_PERMISSIONS)
    
elseif(${UNITY_BUILD_TYPE} STREQUAL "Linux64")

    include(GNUInstallDirs)

    add_custom_target(symlink
        COMMAND cmake -E create_symlink ${CMAKE_INSTALL_FULL_DATADIR}/Anathema/Anathema anathema
    )
    add_custom_target(Linux64
        ALL
        DEPENDS symlink
        COMMENT "Building for Linux64"
        COMMAND ${UNITY_EXECUTABLE} -projectpath ${CMAKE_CURRENT_SOURCE_DIR}/Anathema -buildLinux64Player ${CMAKE_BINARY_DIR}/Anathema -batchmode -quit -logFile /dev/stdout
    )

    set(CPACK_GENERATOR DEB RPM ZIP)

    message(${CMAKE_INSTALL_PREFIX})
    message(${CMAKE_INSTALL_DATAROOTDIR})
    
    install(SCRIPT ${INSTALLER_SOURCES}/symlink.cmake)
    install(PROGRAMS ${CMAKE_BINARY_DIR}/Anathema DESTINATION ${CMAKE_INSTALL_DATADIR}/Anathema)
    install(PROGRAMS ${CMAKE_BINARY_DIR}/anathema DESTINATION ${CMAKE_INSTALL_BINDIR})
    install(FILES ${INSTALLER_SOURCES}/Anathema.desktop DESTINATION ${CMAKE_INSTALL_DATADIR}/applications)
    install(DIRECTORY ${INSTALLER_SOURCES}/hicolor DESTINATION ${CMAKE_INSTALL_DATADIR}/icons)
    install(DIRECTORY ${CMAKE_BINARY_DIR}/Anathema_Data DESTINATION ${CMAKE_INSTALL_DATADIR}/Anathema/)

endif()

set(CPACK_PACKAGE_DESCRIPTION_FILE ${INSTALLER_SOURCES}/Description.txt)
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Anathema brings back the classic Metroidvania feel of exploration, backtracking and overall progression in a grim, futuristic gothic setting.")
# set(CPACK_PACKAGE_INSTALL_DIRECTORY "Anathema")
set(CPACK_PACKAGE_HOMEPAGE_URL "https://fog-icmc.itch.io/anathema")
set(CPACK_PACKAGE_ICON ${INSTALLER_SOURCES}/Anathema.ico)
set(CPACK_RESOURCE_FILE_LICENSE ${CMAKE_CURRENT_SOURCE_DIR}/LICENSE)
set(CPACK_RESOURCE_FILE_README ${CMAKE_CURRENT_SOURCE_DIR}/README.md)
set(CPACK_PACKAGE_EXECUTABLES Anathema;Anathema)


include(CPack)