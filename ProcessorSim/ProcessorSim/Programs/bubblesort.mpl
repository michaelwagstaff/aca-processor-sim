LoadI r1 5
LoadI r2 19
LoadI r3 2
LoadI r4 1
LoadI r5 20

LoadI r6 1 -- Loop counter
LoadI r7 0 -- LT flag
LoadI r8 19
LoadI r9 24
LoadI r10 29
LoadI r11 35
LoadI r14 14
CompareLT r7 r1 r2 -- See if in the right order, if so jump flag = true
CondBranch r7 r8
Copy r1 r12
Copy r2 r1
Copy r12 r2
CompareLT r7 r2 r3 -- See if in the right order, if so jump flag = true
CondBranch r7 r9
Copy r2 r12
Copy r3 r2
Copy r12 r3
CompareLT r7 r3 r4 -- See if in the right order, if so jump flag = true
CondBranch r7 r10
Copy r3 r12
Copy r4 r3
Copy r12 r4
CompareLT r7 r4 r5 -- See if in the right order, if so jump flag = true
CondBranch r7 r11
Copy r4 r12
Copy r5 r4
Copy r12 r5

CompareI r13 r6 5
Not r13
Add r6 r13 -- Add one if we're going to loop, this is like one of those mad compiler optimisations
Print r1
Print r2
Print r3
Print r4
Print r5
CondBranch r13 r14

End