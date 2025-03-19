#!/usr/bin/env bash
# oh yes gipiti so clever
# Usage: ./auto-painting.sh input.jpg output.png
#  1) Resizes input so that the longer side is 50 px
#  2) Rotates 90 degrees
#  3) Applies the perspective warp
#  4) Masks with mask.png
#  5) Overlays this masked result "on top" of background.jpg
#  6) Saves final result to output.png

INFILE="$1"       # e.g. "original.jpg"
MASKFILE="mask.png"     # e.g. "mask.png"
BGFILE="bg.png"       # e.g. "background.jpg"
OUTFILE="$2"      # e.g. "final.png"

########################
# 1) Resize (longer side = 50)
########################
convert "$INFILE" \
  -resize 50x50^ \
  -gravity center \
  -extent 50x50 \
  step1_resized.png

########################
# 2) Rotate 90 degrees
########################
convert step1_resized.png \
  -rotate 90 \
  step2_rotated.png

########################
# 3) Dynamic perspective warp
########################
#  - We grab the width & height of step2_rotated.png.
#  - Then use these as "from" corners, mapping them to your specified "to" coords:
#    "0,0 9,14   ${width},0 40,10   0,${height} 9,34   ${width},${height} 40,38"
width=$(identify -format "%w" step2_rotated.png)
height=$(identify -format "%h" step2_rotated.png)

convert step2_rotated.png \
  -matte \
  -virtual-pixel transparent \
  -distort Perspective \
    "0,0 9,14  \
     ${width},0 40,10  \
     0,${height} 9,34  \
     ${width},${height} 40,38" \
  step3_warped.png

########################
# 4) Mask with another image
########################
#   We assume mask.png has an alpha channel or shape to define the visible region.
convert step3_warped.png "$MASKFILE" \
  -alpha off \
  -compose copy_opacity -composite \
  step4_masked.png

########################
# 5) Combine "on top" of a background image
########################
#   By default, -composite places the second image over the first at the given gravity.
#   Adjust gravity or offsets if you need a specific alignment.
convert "$BGFILE" step4_masked.png \
  -gravity center \
  -composite \
  step5_combined.png

########################
# 6) Save final result
########################
mv step5_combined.png "$OUTFILE"

# Optional: cleanup intermediate steps
rm step1_resized.png step2_rotated.png step3_warped.png step4_masked.png

