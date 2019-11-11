.phony: all

all: mono.csv dotnet3.csv dotnet2.csv

mono.csv: Benchmarks.exe
	mono Benchmarks.exe mono > mono.csv

Benchmarks.exe: *.cs
	mcs *.cs

dotnet3.csv: *.cs *.csproj
	dotnet run --framework=netcoreapp3.0 dotnet3 > dotnet3.csv

dotnet2.csv: *.cs *.csproj
	dotnet run --framework=netcoreapp2.2 dotnet2 > dotnet2.csv

plot: mono.csv dotnet3.csv dotnet2.csv
	python plot.py *.csv

clean:
	rm *.exe *.csv
	dotnet clean