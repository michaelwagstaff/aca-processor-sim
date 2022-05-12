LoadI r1 384024 -- a
LoadI r2 19746 -- b
MonitorStart
Compare r3 r1 r2
CondBranch r3 12
CompareLT r4 r1 r2 -- if b bigger than a
CondBranch r4 10
Subtract r1 r2
Branch 11
Subtract r2 r1
Branch 4
End