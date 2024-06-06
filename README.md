# SystemWakeUp
service for sending wol magic packets to ur pc,when your phone appearing in local network.
## Instructions

1. **Move Scripts folder to your Release folder**
2. **Create masterpc.txt and put the IP of your pc**
3. **Create masterip.txt and put the IP of your device**
4. **Create dbconfig.txt and put username in the first line and password in the second**
5. **Run `dotnet run` or `dotnet SystemWakeUp.dll --urls "http://*:5005/"`**

## Usage

Replace `masterip.txt` with the IP address of your device.

Replace `dbconfig.txt` with your database username on the first line and password on the second line.

Run the application using `dotnet run` or `dotnet SystemWakeUp.dll --urls "http://*:5005/"`.

