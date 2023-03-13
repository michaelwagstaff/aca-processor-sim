LoadI r1 3840923
LoadI r2 19746
LoadI r6 19
LoadI r7 6
MonitorStart
CompareI r3 r2 0
CondBranch r3 r6
Copy r2 r3

Copy r1 r4
Divide r4 r2
Multiply r4 r2
Copy r1 r5
Subtract r5 r4

Copy r5 r2
Copy r3 r1
Branch r7


Print r1

End