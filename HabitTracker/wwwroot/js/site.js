var positions = ["left","center","right"];
var selected = Math.floor(Math.random()*3);
document.body.style.backgroundPosition = positions[selected];

setInterval(function () {
    var a = Math.floor(Math.random()*100);
    var b = Math.floor(Math.random()*100);
    document.body.style.backgroundPosition = a+"% " + b+"%";
}, 5000)
