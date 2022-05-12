all:
	wget -O  net6.tar.gz "https://uob-my.sharepoint.com/:u:/g/personal/bj18895_bristol_ac_uk/Ec2CJALTirdCvJXBGA58yOMBy5roEp1gp2fH9u0ncXZrhQ?download=1"
	mkdir ./net6
	tar -xvf net6.tar.gz -C ./net6
	mkdir ./Programs
	cp -r ./ProcessorSim/ProcessorSim/Programs/* ./Programs
	./net6/dotnet run --project ./ProcessorSim/ProcessorSim/ProcessorSim.csproj
