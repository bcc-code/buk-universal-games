#!/bin/sh
# Requires Inkscape
for size in 192 512
do
  inkscape --export-filename="${size}.png" --export-width $size icon.svg
done

inkscape --export-filename="../favicon.png" --export-width 32 icon.svg
