# WatermarkDatasetBuilder

This is a .NET core project that allow to create a dataset with watermarked and not watermarked pictures in order to use for classifing images according to that.
The watermarked pictures are randomly synthetically generated with random meaningless words, random colors, size and positioning.

The image are downloaded thanks to [Pexels](https://www.pexels.com/) API. If you want to run the code to download a dataset you first need to obtain an API key from [here](https://www.pexels.com/api/documentation/#authorization).

## How to build
This program requires the [NET Core](https://dotnet.microsoft.com/download) SDK **3.1** or better to compile.
Clone the project.
when SDK is installed just run this command in the root folder of the project
```
dotnet buil
```
then you can launch the downloading by using:
```
dotnet run <YOUR API KEY> <output folder> <searchitem1> optsearchitem2 optsearchitem3 ... optsearchitemN
```
