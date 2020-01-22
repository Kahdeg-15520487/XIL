# XIL.Assembler.Preprocessor

Simple macro processing

Example:

precalculated value:
```
;print 2*2
yeet #* 2*2 *#
```

assign variable:
```
;print 2*2
#* i = 2*2 *#
yeet #* i *#
#* i = 3*3 *#
;print 3*3
yeet #* i *#
```