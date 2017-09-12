function Ball(x, y, radius, speed)
{
 this.x = x;
 this.y = y;
 this.speedX = speed;
 this.speedY = speed;
 this.radius = radius;
 this.update = function () {
     if (this.y < this.radius || this.y > height - radius)
         this.speedY = this.speedY * -1;
     if (this.x - this.radius === leftLine.x && this.y >= leftLine.y1 && this.y <= leftLine.y2 || this.x + this.radius === rightLine.x && this.y >= rightLine.y1 && this.y <= rightLine.y2)
         this.speedX = this.speedX * -1;

     if (this.x < -radius || this.x > width + radius) {
         this.x = width / 2;
         this.y = height / 2;
     }

     this.y += this.speedY;
     this.x += this.speedX;
 };
}

function Line(x, y1, upKey, downKey)
{
  this.lineSpeed = 14;
  this.x = x;
  this.y1 = y1;
  this.y2 = y1+150;
  this.upKey = upKey;
  this.downKey = downKey;
  this.update = function () {
      if (keyIsDown(this.upKey) && this.y1 > 0) {
          this.y1 -= this.lineSpeed;
          this.y2 -= this.lineSpeed;
      }

      if (keyIsDown(this.downKey) && this.y2 < height) {
          this.y1 += this.lineSpeed;
          this.y2 += this.lineSpeed;
      }
  };
}

var leftLine;
var rightLine;
var ball;

function setup() {
  createCanvas(800,800);
  ball = new Ball(width/2, height/2, 20, 5);
  leftLine = new Line(30, 350, 87, 83);
  rightLine = new Line(770, 350, 38, 40);
}

function draw() {
 background(0);
 stroke(255);
 line(leftLine.x, leftLine.y1, leftLine.x, leftLine.y2);
 line(rightLine.x, rightLine.y1, rightLine.x, rightLine.y2);

 noStroke();
 fill(255);
 ellipse(ball.x, ball.y, ball.radius, ball.radius);

 leftLine.update();
 rightLine.update();
 ball.update();
}
