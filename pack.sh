#!/bin/bash

# This script builds and packages everything into a usable structure.

if [[ -d "output" ]]; then
  rm -r output
fi

dotnet build -p:Configuration=Release -o output
mkdir -p output/Addons/Core

cp -r PDD/Levels output

