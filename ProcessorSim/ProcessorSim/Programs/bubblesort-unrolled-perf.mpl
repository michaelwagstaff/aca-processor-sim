LoadI r1 5
LoadI r2 19
LoadI r3 2
LoadI r4 1
LoadI r5 20
MonitorStart
LoadI r7 0 -- LT flag
CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 13
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 18
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 23
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 29
Copy r12 r4
Copy r4 r5
Copy r5 r12

CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 34
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 39
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 44
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 49
Copy r12 r4
Copy r4 r5
Copy r5 r12

CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 55
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 60
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 65
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 70
Copy r12 r4
Copy r4 r5
Copy r5 r12

CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 77
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 82
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 87
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 92
Copy r12 r4
Copy r4 r5
Copy r5 r12

CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 97
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 102
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 107
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 112
Copy r12 r4
Copy r4 r5
Copy r5 r12

End