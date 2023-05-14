#!/bin/sh
# Requires Inkscape and ImageMagick

inkscape --export-filename="splashscreen.png" splashscreen.svg

for size in '2048x2732' '1668x2224' '1536x2048' '1125x2436' '1242x2208' '750x1334' '640x1136'
do
  convert splashscreen.png -resize $size^ -gravity center -extent $size "${size}.png"
done

rm -f splashscreen.png
