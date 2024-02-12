#!/bin/sh

echo 'Copying web build files to website wwwroot ...'

cp ./game_client/build_web/Build/* ./website/wwwroot/Build

echo 'Renaming .data files to .bin files workaround...'

mv ./website/wwwroot/Build/build_web.data ./website/wwwroot/Build/build_web.data.bin

echo 'Done!'
