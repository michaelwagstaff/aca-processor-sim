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
Store r1 11
Store r1 12
Store r1 13
Store r1 14
Store r1 15
Store r1 16
Store r1 17
Store r1 18
Store r1 19
Store r1 20
Store r2 21
Store r2 22
Store r2 23
Store r2 24
Store r2 25
Store r2 26
Store r2 27
Store r2 28
Store r2 29
Store r2 30
Store r2 31
Store r2 32
Store r2 33
Store r2 34
Store r2 35
Store r2 36
Store r2 37
Store r2 38
Store r2 39
Store r2 40
MonitorStart
LoadI r1 0 -- Use r1 as loop counter
LoadI r2 1
LoadI r3 21
LoadI r4 41 -- memory addresses for each array respectively
LoadR r5 r2
LoadR r6 r3
LoadI r7 1 -- Increment
Add r5 r6
StoreR r5 r4 -- Store r5 in address in r4
Add r1 r7
Add r2 r7
Add r3 r7
Add r4 r7
Print r5
CompareI r9 r1 20
Not r9
CondBranch r9 48

End