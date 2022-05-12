LoadI r1 5
LoadI r2 19
LoadI r3 2
LoadI r4 1
LoadI r5 20
MonitorStart
LoadI r6 1 -- Loop counter
LoadI r7 0 -- LT flag
CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 14
Copy r12 r1
Copy r1 r2
Copy r2 r12
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 19
Copy r12 r2
Copy r2 r3
Copy r3 r12
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 24
Copy r12 r3
Copy r3 r4
Copy r4 r12
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 29
Copy r12 r4
Copy r4 r5
Copy r5 r12
CompareI r13 r6 5
Not r13
Add r6 r13 -- Add one if we're going to loop, this is like one of those mad compiler optimisations
CondBranch r13 9
End