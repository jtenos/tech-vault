```html
<!DOCTYPE html>
<html>
<head>
    <title>Test Zip</title>
    </head>
    <body>
        <textarea id="inputText" style="width: 90%; height: 300px;"></textarea>
        <textarea id="outputText" style="width: 90%; height: 300px;"></textarea>
        <br>
        <input type="button" value="Go" onclick="go();">
        <script src="https://cdn.jsdelivr.net/pako/1.0.5/pako.min.js"></script>
        <script>
            function go() {
                console.log(document.getElementById("inputText").value.length);
                var compressedBytes = pako.gzip(document.getElementById("inputText").value);
 
                console.log("compressedBytes: " + compressedBytes.length);
 
                try {
                   var decompressedText = pako.ungzip(compressedBytes, { to: "string" });
                   document.getElementById("outputText").value = decompressedText;
                } catch (ex) {
                    console.log(ex);
                }
            }
        </script>
    </body>
</html>
```
```javascript
const pako = require("pako");
 
const inputText = `-団'健載婚。西住姿要臣市未検成録止悪転団。稿代江型集対'-側込円号乳前爆下集広。稿界版陽更工振火国張"指天"-自果圧見。判社安団自外面"売神農更-策現管野。線'破面経速"-止満行直。集禁未化捕併投購加--録規開各在造能訴。断否界駅小安計注必式面柴。会継内欺-方添条武風復不断'楽""。
 
Hello world, this is a test!
 
知系締描文'法芸機各共作請始紹論禁。者注省警権宅急今変埼問何文力。陣済"名屋康慣""前更測説類無。-医教護'野'綱混着特購加遅代逸通口戦。-再賀期木多合作就勢城析車人出済。訪任人"重江賞需農神書関。市理-"礼場事現運違有指的。心秋雄式作意切加県米外限竹真法鮮。足"漁性倍何買真購話注私紙中際。`;
 
console.log(inputText);
 
const compressedBytes = pako.gzip(inputText);
 
console.log(`compressedBytes: ${compressedBytes.length}`);
 
try {
   const decompressedText = pako.ungzip(compressedBytes, { to: "string" });
   console.log(decompressedText);
} catch (ex) {
    console.log(ex);
}
```