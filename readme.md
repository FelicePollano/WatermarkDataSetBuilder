# WatermarkDatasetBuilder

This is a .NET core project that allow to create a dataset with watermarked and not watermarked pictures in order to use for classifing images according to that.
The project is to support my custom experiment while attending the Cousera course [Convolutional Neural Networks in TensorFlow Couse](https://www.coursera.org/learn/convolutional-neural-networks-tensorflow/home/welcome)
The watermarked pictures are randomly synthetically generated with random meaningless words, random colors, size and positioning.

The image are downloaded thanks to [Pexels](https://www.pexels.com/) API. 

[Pexels](https://www.pexels.com/) *provides high quality and completely free stock photos licensed under the Pexels license. All photos are nicely tagged, searchable and also easy to discover through our discover pages.*


You can find better detail about the **license** [here](https://www.pexels.com/license/)

If you want to run the code to download a dataset you first need to obtain an API key from [here](https://www.pexels.com/api/documentation/#authorization).

## How to build
This program requires the [NET Core](https://dotnet.microsoft.com/download) SDK **3.1** or better to compile.
Clone the project.
when SDK is installed just run this command in the root folder of the project
```
dotnet build
```
## How to run
then you can launch the downloading by using:
```
dotnet run <YOUR API KEY> <output folder> <searchitem1> optsearchitem2 optsearchitem3 ... optsearchitemN
```
As download starts you can interrupt it by breaking it ```Ctrl-C``` otherwise it will continue until it reach the API limitation.


## Dataset structure
The dataset is structured as below:
```
/output-folder
|-----train
|    |------no-watermark
|    |------watermark
|-----valid
|    |------no-watermark
|    |------watermark
| .checkpoint

```
This should help to load the image by using the Keras `ImageDataGenerator`

Data are splitted across *train* and *valid* with a proportion of 80/20. Watermark/not watermark ratio is supposed to be 50/50, but can sligthy change due to image processor errors.

Please note the (hidden) file ```.checkpoint``` which pourpose is to restart from where the download left in case of any kind of stop ( even API limitation). If, for some reason, you want to start from scratch, just remove this file.

# Examples

## Easy to recognize watermarks

<img align="left" src="https://user-images.githubusercontent.com/73569/94658283-e747ea80-0302-11eb-9df1-21787ca76ad6.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658286-e8791780-0302-11eb-9e3a-06301422fc7e.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658289-e8791780-0302-11eb-9473-393e3a37453a.jpg">

<img align="left" src="https://user-images.githubusercontent.com/73569/94658292-e911ae00-0302-11eb-9c5a-f11d26147ce4.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658293-e911ae00-0302-11eb-8001-b7b36475dcf8.jpg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658294-e9aa4480-0302-11eb-8cab-9e1fa343d67a.jpg">

<img align="left" src="https://user-images.githubusercontent.com/73569/94658296-e9aa4480-0302-11eb-80eb-ab1ee4084309.jpg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658299-ea42db00-0302-11eb-9f84-713aba7daf50.jpg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94658303-eadb7180-0302-11eb-87f2-b80739df1e71.jpeg">

<img src="https://user-images.githubusercontent.com/73569/94658306-eb740800-0302-11eb-89df-1d37eec1c5ad.jpeg">


## Difficult to recognize watermarks
Can you spot it?

<img align="left" src="https://user-images.githubusercontent.com/73569/94659198-35a9b900-0304-11eb-94fe-e7cdbc4d3b3a.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659200-36424f80-0304-11eb-9ac1-698987787e5a.jpg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659202-36dae600-0304-11eb-8707-d17b23b4b829.jpeg">

<img align="left" src="https://user-images.githubusercontent.com/73569/94659204-37737c80-0304-11eb-94a4-6ce92de879c3.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659205-380c1300-0304-11eb-94a6-941ac9d888e2.jpg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659208-380c1300-0304-11eb-92e6-c474aac5d6f6.jpeg">

<img align="left" src="https://user-images.githubusercontent.com/73569/94659211-38a4a980-0304-11eb-9d98-348ae8e5209f.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659215-393d4000-0304-11eb-84bf-0b4187e26fa7.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659217-393d4000-0304-11eb-81b1-df763d5bcb0e.jpeg">

<img align="left" src="https://user-images.githubusercontent.com/73569/94659218-39d5d680-0304-11eb-874d-663dc4eeeb18.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659220-3a6e6d00-0304-11eb-9a5b-b0b40b0f6cdf.jpeg">
<img align="left" src="https://user-images.githubusercontent.com/73569/94659221-3b070380-0304-11eb-9923-3a0e6cde1664.jpeg">



