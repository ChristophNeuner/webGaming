var socket;
var btnClean;
var webSocket;

function setup() {
    createCanvas(2000, 800);
    background(51);
    btnClean = createButton("clean canvas");
    btnClean.mouseClicked(clean);
   
    $().ready(function () {
        webSocket = new WebSocket("ws://212.227.11.55:6/wsPaintingTogether");
        //webSocket = new WebSocket("ws://localhost:56195/wsPaintingTogether");
        webSocket.onopen = function () {
            $("#spanStatus").text("connected");
        };
        webSocket.onmessage = function (evt) {
            $("#spanStatus").text("connected");
            newDrawing(evt.data);
            
        };
        webSocket.onerror = function (evt) {
            alert(evt.message);
        };
        webSocket.onclose = function () {
            $("#spanStatus").text("disconnected");
        };       
    });   
}

function mouseDragged() {
    console.log('Sending: ' + mouseX + ',' + mouseY);

    var data = Math.round(mouseX).toString() + "," + Math.round(mouseY).toString();

    if (webSocket.readyState === WebSocket.OPEN) {
        webSocket.send(data);
    }
    else {
        $("#spanStatus").text("Connection is closed");
    }

    noStroke();
    fill(255);
    ellipse(mouseX, mouseY, 10, 10);
}

function newDrawing(data) {
    noStroke();
    fill(255, 0, 100);
    var coordinates = data.split(",");
    var x = parseInt(coordinates[0]);
    var y = parseInt(coordinates[1]);
    $("#coordinates").text(x + ", " + y);
    ellipse(x, y, 9, 10);
}



function draw() {

}

function clean() {
    background(51);
}