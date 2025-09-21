#!/bin/bash

if [ $# -ne 1 ]; then
  echo "Usage: $0 {-c|-b}"
  exit 1
fi

operation="$1"

case "$operation" in
  "-c")
    verb="Cleaning"
    cmd="clean"
    ;;
  "-b")
    verb="Building"
    cmd="build"
    ;;
  *)
    echo "Invalid operation: $operation. Must be 'clean' or 'build'."
    exit 1
    ;;
esac

solutions=(
  "CatalogService/CatalogService.sln"
  "CatalogManagementService/CatalogManagementService.sln"
)

for sln in "${solutions[@]}"; do
  if [ -f "$sln" ]; then
    echo "$verb $sln ..."
    dotnet $cmd "$sln"
    echo $'\n'
  else
    echo "Solution file $sln not found!"
  fi
done
