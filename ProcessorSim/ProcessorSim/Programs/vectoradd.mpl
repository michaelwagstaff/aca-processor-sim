LoadI r1 1
LoadI r2 2
Store r1 1
Store r1 2
Store r1 3
Store r1 4
Store r1 5
Store r1 6
Store r1 7
Store r1 8
Store r1 9
Store r1 10
Store r2 11
Store r2 12
Store r2 13
Store r2 14
Store r2 15
Store r2 16
Store r2 17
Store r2 18
Store r2 19
Store r2 20

LoadI r1 0 -- Use r1 as loop counter
LoadI r2 1
LoadI r3 11
LoadI r4 21 -- memory addresses for each array respectively
LoadR r5 r2
LoadR r6 r3
LoadI r7 1 -- Increment
LoadI r8 32 -- Address to jump to
Add r5 r6
StoreR r5 r4 -- Store r5 in address in r4
Add r1 r7
Add r2 r7
Add r3 r7
Add r4 r7
CompareI r9 r1 10
CondBranch r9 r8

