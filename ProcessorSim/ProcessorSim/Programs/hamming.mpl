LoadI r1 1
LoadI r2 1
LoadI r3 1
LoadI r4 1
LoadI r5 0
LoadI r6 0
LoadI r7 0
LoadI r8 0
LoadI r9 1
LoadI r10 1
LoadI r11 1
LoadI r12 0
LoadI r13 0
LoadI r14 0
LoadI r15 1
LoadI r16 0
LoadI r17 0
LoadI r18 1
LoadI r19 0
LoadI r20 0
Store r1 1
Store r2 2
Store r3 3
Store r4 4
Store r5 5
Store r6 6
Store r7 7
Store r8 8
Store r9 9
Store r10 10
Store r11 11
Store r12 12
Store r13 13
Store r14 14
Store r15 1
Store r16 16
Store r17 17
Store r18 18
Store r19 19
Store r20 20
MonitorStart
LoadI r5 1
LoadI r3 0 -- Hamming sum
LoadI r2 1 -- Loop Counter
LoadR r1 r2
CompareI r4 r1 1
Add r3 r4
Add r2 r5
CompareI r6 r2 21
Not r6
CondBranch r6 45

Print r3
