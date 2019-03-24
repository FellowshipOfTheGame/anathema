#!/bin/bash
set -e #Quit if any command fails

SCRIPT=`realpath -s $0`
SCRIPTPATH=`dirname $SCRIPT`
cd $SCRIPTPATH

if [ "$1" != "Windows64" ] && [ "$1" != "Linux64" ] && [ "$1" != "Mac" ] && [ "$1" != "clean" ]; then
    echo "build.sh: Invalid build type"
    exit 1
elif [ $1 = "clean" ]; then
    echo "build.sh: Removing build folder"
    rm -rf build
    exit 0
fi

if [ ! -d build ]; then
    mkdir build
    cd build
    echo "build.sh: Generating build files with CMake"
    cmake -DUNITY_BUILD_TYPE=$1 ..
else
    echo "build.sh: Build folder already exists, assuming CMake has already generated buildfiles"
    cd build
fi

echo "build.sh: Building project"
cmake --build .
echo "build.sh: Packaging project"
cpack .