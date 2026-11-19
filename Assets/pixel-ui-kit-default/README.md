# Pixels - Pixel Art UI Kit

Pixel interface pieces for 2D games, drawn at native resolution so they stay
crisp at every scale instead of going soft like scaled vector art.

## What is included

- 37 pieces: panels, buttons, gauges, slots, cursors and icons
- PNG exports at 1x, 2x, 3x and 4x, nearest-neighbour scaled, fully transparent
- JSON atlas data with 9-slice insets so panels and buttons stretch correctly
- CSV metadata listing every piece, its category and its export paths
- Commercial-use licence

## 9-slice panels

`metadata/atlas.json` gives top/right/bottom/left insets for every panel and
button, so you can build any size without redrawing: set the insets once and
stretch the middle.

## Scaling rule

Only use integer scaling with nearest-neighbour. The 1x files are the source of
truth; the 2x/3x/4x files are exact multiples so they never blur.

## Licence

Commercial use in games and apps is allowed, including client work. You may
modify the files. You may not resell or redistribute them as an asset pack, or
ship them inside another pack or template.

Generated procedurally with generative AI assistance. No third-party pixels or
trademarks were used.
