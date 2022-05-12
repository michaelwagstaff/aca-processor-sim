LoadI r1 384024 -- a
LoadI r2 19746 -- b
MonitorStart
LoadI r5 20
LoadI r6 15
LoadI r7 17
LoadI r8 8

Compare r3 r1 r2
CondBranch r3 21

CompareLT r4 r1 r2 -- if b bigger than a
CondBranch r4 16
Subtract r1 r2
Branch 17
Subtract r2 r1



Branch 8


Print r1

End