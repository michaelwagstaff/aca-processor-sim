MonitorStart
LoadI r1 1
LoadI r2 1
LoadI r3 1
LoadI r4 1
Add r2 r1
Add r1 r2
Add r3 r4
CompareI r5 r3 10
Not r5
CondBranch r5 5

End