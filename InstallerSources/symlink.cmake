include(GNUInstallDirs)

execute_process(COMMAND cmake -E create_symlink ${CMAKE_INSTALL_PREFIX}/${CMAKE_INSTALL_DATADIR}/Anathema/Anathema anathema) 