## Problem Description
Write a program that, given a list of courses and their pre-requisites, produce a possible order in which a student can complete as many of the provided course units as possible, adhering to the pre-requisite requirements.  

The program reads from two input files, “courses.csv” and “prerequisites.csv”, which can be found in *SampleInputFiles/* folder.

## Problem Analysis
The problem involves the “number of ways” a student can complete a course. Without the given pre-requisites, this problem is reduced to a simple permutation algorithm. With pre-requisites imposed, the solution is to take a subset of the permutation result between all course ids, or better, generating the permutation in such a way that adheres to the pre-requisites imposed.

### Challenges
There are two main challenges when it comes to solving this problem.

#### 1.	When no course completion is possible
This happens when there is a circular dependency between each course. For instance, if we have 3 courses: { 1, 2, 3 }, where course 2 and 3 require course 1 to be completed and course 1 requires course 2 or 3 to be completed.

**Solution**: The algorithm has to be able to identify when the course pre-requisite imposed is not valid (completion of all courses is impossible).

#### 2.	When no course pre-requisite is imposed – worst case scenario
Even though not given as the sample input, we need to think of the worst-case scenario while designing the algorithm. This happens when student is free to do any course in any they like (i.e. no course pre-requisite is imposed). In this case, the number of possibilities reach n!.

Given an input size of 12 courses and no pre-requisite imposed, this will generate more than 479 million possibilities. With each course id stored as short integer (2 bytes), this requires memory of 11.5 GB if the list of possibilities is to be returned.

**Solution**: We can solve this by printing the output to console directly without returning the list, but this might not be ideal in real world scenario (i.e. if we are making a web application to facilitate students in finding this answer). In this scenario, the ideal way is to calculate the results per page (e.g. 50 results per page).

## Sample Test Cases
Given the sample test case, there is no valid way to complete all the courses as there is circular dependency between course 4, 5, and 9. Given this test case, the application should be able to run this test case and expects the right exception to be thrown – “Invalid pre-requisites. No way to complete all the courses”.

Additionally, I am interested to run through several more test cases. While the challenge can be considered complete at this stage, I I’d like to verify that my solution works across different input data.

The following test cases can be verified by running the program.

### Test Case 1
Default test case supplied with the coding challenge.

**Expected Output**

Exception – invalid pre-requisite - no way to complete this course.

### Test Case 2
A simple test case designed with courses 1-4 with courses 2-4 requiring completion of course 1.

**Expected Output**

Starts with 1, followed with permutation of { 2, 3, 4 }, as follows:
{ 1, 2, 3, 4 }, { 1, 2, 4, 3 }, { 1, 3, 2, 4 }, { 1, 3, 4, 2 }, { 1, 4, 2, 3 }, { 1, 4, 3, 2 }

### Test Case 3
A simple test case designed with courses 1-4 where student is free to do all courses in any order they like (i.e. no pre-requisite is imposed). This test case is covered to measure the worst-case scenario, with low value of n.

**Expected Output**

All permutation of { 1, 2, 3, 4 }.

### Test Case 4
This test case is designed to check expected behavior when no courses/pre-requisites are imposed.

**Expected Output**

Empty Result.

### Test Case 5
This test case is designed to check expected behavior when one of the course ids in the pre-requisites is invalid.

**Expected Output**

Exception – Invalid pre-requisite.

### Test Case 6
Finally, this test case is a modified version of test case 1, where the pre-requisite of course 9 is changed from 5 to 1 in order to remove circular dependency.

**Expected Output**

1500 results, outlined in the expected-output.txt of TestCases/Test6 folder.


## Unit Tests
This project also comes with a UnitTest project, designed to ensure that each logical function works as expected with different mockup data.
