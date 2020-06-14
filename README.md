# README

Sticky Finger Odyssey



#### 相机溶解添加说明 6.14

`NoiseTextureGenerator:`噪声纹理生成器，场景中有一个就🆗了

`OcclusionDissolve:`实现溶解的shader，其中的参数`DissolveMap`用来填写噪声纹理生成器生成的纹理图

`method:`为想要溶解的物体挂载上`ObjectDissolve.cs`，并将当前物体材质的shader更换为`OcclusionDissolve`。对于导入的模型，直接新建一个材质并更换shader之后，更换对应物体材质。调整颜色直到与原模型材质颜色类似。