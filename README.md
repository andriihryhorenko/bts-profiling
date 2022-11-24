# bts-profiling
bts-profiling


Working with BTS:
1. Insert rows.
2. Find -l random element.
3. Delete -l random elements

And printing times of executing of each action:

![image](https://user-images.githubusercontent.com/112312750/203818016-8201433c-aee4-45a3-94bc-de8ac48c88a1.png)

L: number of elements in BTS

## How to run

```bash
cd ./bts-profiling/bts-profiling/

$ dotnet run -l=12 -n=12
```

-n: number of random datasets
-l: size of the first dataset and step for next ones
